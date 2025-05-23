using MassTransit;
using Shop.Server.Messages;
using System;
using System.Threading.Tasks;

namespace Shop.Server.Services;

public class WarehouseService : IConsumer<CheckStock>
{
    public async Task Consume(ConsumeContext<CheckStock> context)
    {
        Console.WriteLine($"🏭 Magazyn: Sprawdzam dostępność {context.Message.Quantity} szt.");

        var random = new Random();
        var available = random.Next(2) == 0;

        try
        {
            await Task.Delay(500, context.CancellationToken);

            if (available)
            {
                Console.WriteLine("✅ Magazyn: Towar dostępny");

                await context.Publish(new StockAvailable(context.Message.OrderId, context.Message.Quantity));
            }
            else
            {
                Console.WriteLine("❌ Magazyn: Towar niedostępny");

                await context.Publish(new StockUnavailable(context.Message.OrderId, context.Message.Quantity));
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("⚠️ WarehouseService: Operacja została anulowana");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ WarehouseService: Błąd - {ex.Message}");
            throw;
        }
    }
}
