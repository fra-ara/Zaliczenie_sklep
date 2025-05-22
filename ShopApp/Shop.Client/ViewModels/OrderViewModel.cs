using MassTransit;
using Shop.Client.Models;
using Shop.Server.Messages;

namespace Shop.Client.ViewModels;

public class OrderViewModel
{
    private readonly IBus _bus;

    public OrderViewModel(IBus bus)
    {
        _bus = bus;
    }

    public async Task SubmitOrderAsync(int quantity)
    {
        var order = new OrderModel
        {
            OrderId = Guid.NewGuid(),
            Quantity = quantity
        };

        Console.WriteLine($"📦 Wysyłam zamówienie {order.OrderId} na {order.Quantity} szt.");
        await _bus.Publish(new StartOrder(order.OrderId, order.Quantity));
    }
}
