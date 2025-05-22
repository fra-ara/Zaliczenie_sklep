using MassTransit;
using Shop.Server.Messages;

namespace Shop.Server.Services;

public class WarehouseService :
    IConsumer<CheckStock>,
    IConsumer<OrderRejected>
{
    private static int FreeUnits = 10;
    private static int ReservedUnits = 0;

    public Task Consume(ConsumeContext<CheckStock> context)
    {
        var qty = context.Message.Quantity;
        Console.WriteLine($"🧮 Magazyn: Sprawdzam dostępność dla {qty} szt.");

        if (FreeUnits >= qty)
        {
            FreeUnits -= qty;
            ReservedUnits += qty;
            Console.WriteLine($"✅ Magazyn: Zarezerwowano {qty}, wolne: {FreeUnits}, zarezerwowane: {ReservedUnits}");
            return context.Publish(new StockAvailable(context.Message.OrderId, context.Message.Quantity));
        }
        else
        {
            Console.WriteLine($"❌ Magazyn: Brak zasobów.");
            return context.Publish(new StockUnavailable(context.Message.OrderId, context.Message.Quantity));
        }
    }

    public Task Consume(ConsumeContext<OrderRejected> context)
    {
        Console.WriteLine($"↩️ Magazyn: Zamówienie odrzucone, zwracam zasoby");
        ReservedUnits += 0;
        return Task.CompletedTask;
    }
}
