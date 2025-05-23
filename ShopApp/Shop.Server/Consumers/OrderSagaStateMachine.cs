using MassTransit;
using Shop.Server.Messages;
using System;

namespace Shop.Server.Consumers;

public class OrderSagaStateMachine : MassTransitStateMachine<OrderState>
{
    public State AwaitingStockCheck { get; private set; } = default!;
    public State AwaitingClientConfirmation { get; private set; } = default!;

    public Event<StartOrder> OrderStarted { get; private set; } = default!;
    public Event<StockAvailable> StockConfirmed { get; private set; } = default!;
    public Event<StockUnavailable> StockRejected { get; private set; } = default!;
    public Event<ClientConfirmed> ClientConfirmed { get; private set; } = default!;
    public Event<ClientRejected> ClientRejected { get; private set; } = default!;

    public Schedule<OrderState, OrderTimeoutExpired> OrderTimeoutSchedule { get; private set; } = default!;

    public OrderSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderStarted, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => StockConfirmed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => StockRejected, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => ClientConfirmed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => ClientRejected, x => x.CorrelateById(ctx => ctx.Message.OrderId));

        Schedule(() => OrderTimeoutSchedule, x => x.TimeoutTokenId, s =>
        {
            s.Delay = TimeSpan.FromSeconds(20);
            s.Received = e => e.CorrelateById(ctx => ctx.Message.OrderId);
        });

        Initially(
            When(OrderStarted)
                .Then(context =>
                {
                    context.Saga.Quantity = context.Message.Quantity;
                    Console.WriteLine($"🛒 Order started with quantity {context.Saga.Quantity}, requesting stock check.");
                })
                .Send(new Uri("queue:warehouse-service"), context => new CheckStock(context.Saga.CorrelationId, context.Saga.Quantity))
                .TransitionTo(AwaitingStockCheck)
        );

        During(AwaitingStockCheck,
            When(StockConfirmed)
                .Then(context =>
                {
                    Console.WriteLine("✅ Stock confirmed, asking client for confirmation.");
                })
                .Send(new Uri("queue:client-confirmation"), context => new RequestClientConfirmation(context.Saga.CorrelationId, context.Saga.Quantity))
                .Schedule(OrderTimeoutSchedule, context => new OrderTimeoutExpired { OrderId = context.Saga.CorrelationId })
                .TransitionTo(AwaitingClientConfirmation),

            When(StockRejected)
                .Then(context =>
                {
                    Console.WriteLine("❌ Stock unavailable, cancelling order.");
                })
                // Możesz wysłać klientowi info o braku stocku, opcjonalne:
                //.Send(new Uri("queue:client-confirmation"), context => new NotifyClientStockUnavailable(context.Saga.CorrelationId))
                .Finalize()
        );

        During(AwaitingClientConfirmation,
            When(ClientConfirmed)
                .Unschedule(OrderTimeoutSchedule)
                .Then(context =>
                {
                    Console.WriteLine("👍 Client confirmed order. Order completed successfully.");
                })
                .Finalize(),

            When(ClientRejected)
                .Unschedule(OrderTimeoutSchedule)
                .Then(context =>
                {
                    Console.WriteLine("👎 Client rejected the order. Cancelling.");
                })
                .Finalize(),

            When(OrderTimeoutSchedule.Received)
                .Then(context =>
                {
                    Console.WriteLine("⏰ Timeout waiting for client confirmation, cancelling order.");
                })
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
