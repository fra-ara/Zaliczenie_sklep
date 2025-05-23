using MassTransit;
using Shop.Server.Messages;

namespace Shop.Client.Services;

public class ClientService : IConsumer<RequestClientConfirmation>
{
    public async Task Consume(ConsumeContext<RequestClientConfirmation> context)
    {
        var quantity = context.Message.Quantity;
        var orderId = context.Message.OrderId;

        Console.WriteLine($"🧑‍🛒 Klient: Proszę o potwierdzenie zamówienia na {quantity} szt.");

        // Losowo potwierdza lub odrzuca
        bool confirm = new Random().Next(2) == 0;

        if (confirm)
        {
            Console.WriteLine("👍 Klient: Potwierdzam");
            await context.Publish(new ClientConfirmed(orderId, quantity));
        }
        else
        {
            Console.WriteLine("👎 Klient: Odrzucam");
            await context.Publish(new ClientRejected(orderId, quantity));
            Console.WriteLine("❌ Zamówienie zostało odrzucone przez klienta.");
        }
    }
}
