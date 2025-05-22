using MassTransit;
using Shop.Server.Messages;

namespace Shop.Server.Services;

public class ClientService :
    IConsumer<StartOrder>
{
    public async Task Consume(ConsumeContext<StartOrder> context)
    {
        Console.WriteLine($"🧑‍🛒 Klient: Otrzymano zamówienie na {context.Message.Quantity} szt.");

        // symuluj zachowanie klienta (np. random)
        var random = new Random();
        var confirm = random.Next(2) == 0;

        await Task.Delay(500); // zasymuluj czas reakcji

        if (confirm)
        {
            Console.WriteLine("👍 Klient: Potwierdzam");
            await context.Publish(new ConfirmOrder(context.Message.OrderId, context.Message.Quantity));
        }
        else
        {
            Console.WriteLine("👎 Klient: Odrzucam");
            await context.Publish(new RejectOrder(context.Message.OrderId, context.Message.Quantity));
        }
    }
}
