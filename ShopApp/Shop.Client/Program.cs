using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shop.Client.Services;
using Shop.Client.ViewModels;
using Shop.Client.Views;

var services = new ServiceCollection();

services.AddMassTransit(x =>
{
    x.AddConsumer<ClientService>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("client-confirmation", e =>
        {
            e.ConfigureConsumer<ClientService>(context);
        });
    });
});

var provider = services.BuildServiceProvider();
var busControl = provider.GetRequiredService<IBusControl>();

await busControl.StartAsync();
Console.WriteLine("🚀 Shop.Client is running...");

var viewModel = new OrderViewModel(busControl);
var view = new ConsoleView(viewModel);
await view.RunAsync();  // tutaj w konsoli możesz wpisywać ilości zamówienia i wysyłać

await busControl.StopAsync();
