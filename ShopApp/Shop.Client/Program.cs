using MassTransit;
using Shop.Client.ViewModels;
using Shop.Client.Views;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("localhost", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
});

await busControl.StartAsync();

var viewModel = new OrderViewModel(busControl);
var view = new ConsoleView(viewModel);
await view.RunAsync();
