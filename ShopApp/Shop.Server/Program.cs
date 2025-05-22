using MassTransit;
using Shop.Server.Consumers;
using Shop.Server.Services;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("localhost", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });

    cfg.ReceiveEndpoint("order-saga", e =>
    {
        e.StateMachineSaga(new OrderSagaStateMachine(), new InMemorySagaRepository<OrderState>());
    });

    cfg.ReceiveEndpoint("warehouse-service", e =>
    {
        e.Consumer<WarehouseService>();
    });

    cfg.ReceiveEndpoint("client-service", e =>
    {
        e.Consumer<ClientService>();
    });
});

await busControl.StartAsync();
Console.WriteLine("🚀 Shop.Server is running...");
await Task.Delay(-1);
