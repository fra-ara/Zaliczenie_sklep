using MassTransit;
using Shop.Server.Messages;
using System;

namespace Shop.Server.Consumers;

public class OrderTimeoutExpired
{
    public Guid OrderId { get; set; }
}

public class OrderSagaStateMachine : MassTransitStateMachine<OrderState>
{
    public State AwaitingConfirmation { get; private set; } = default!;
    public Event<StartOrder> OrderStarted { get; private set; } = default!;
    public Event<ConfirmOrder> ClientConfirmed { get; private set; } = default!;
    public Event<StockAvailable> StockConfirmed { get; private set; } = default!;
    public Event<RejectOrder> ClientRejected { get; private set; } = default!;
    public Event<StockUnavailable> StockRejected { get; private set; } = default!;
    
    public Schedule<OrderState, OrderTimeoutExpired> OrderTimeoutSchedule { get; private set; } = default!;

    public OrderSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderStarted, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => ClientConfirmed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => StockConfirmed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => ClientRejected, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => StockRejected, x => x.CorrelateById(ctx => ctx.Message.OrderId));
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
                    Console.WriteLine($"🛒 Order started with quantity {context.Saga.Quantity}");
                })
                .Schedule(OrderTimeoutSchedule, context => new OrderTimeoutExpired { OrderId = context.Saga.CorrelationId })
                .TransitionTo(AwaitingConfirmation)
        );

        During(AwaitingConfirmation,
            When(ClientConfirmed)
                .Then(context =>
                {
                    Console.WriteLine("👍 Client confirmed order.");
                    context.Saga.ClientConfirmed = true;
                })
                .IfElse(context => context.Saga.StockConfirmed,
                    then => then
                        .Unschedule(OrderTimeoutSchedule)
                        .Finalize(),
                    @else => @else
                ),

            When(StockConfirmed)
                .Then(context =>
                {
                    Console.WriteLine("✅ Stock confirmed order.");
                    context.Saga.StockConfirmed = true;
                })
                .IfElse(context => context.Saga.ClientConfirmed,
                    then => then
                        .Unschedule(OrderTimeoutSchedule)
                        .Finalize(),
                    @else => @else
                ),

            When(ClientRejected)
                .Then(context =>
                {
                    Console.WriteLine("👎 Client rejected the order.");
                })
                .Unschedule(OrderTimeoutSchedule)
                .Finalize(),

            When(StockRejected)
                .Then(context =>
                {
                    Console.WriteLine("❌ Stock unavailable for the order.");
                })
                .Unschedule(OrderTimeoutSchedule)
                .Finalize(),

            When(OrderTimeoutSchedule.Received)
                .Then(context =>
                {
                    Console.WriteLine("⏰ Order timed out, cancelling.");
                })
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
