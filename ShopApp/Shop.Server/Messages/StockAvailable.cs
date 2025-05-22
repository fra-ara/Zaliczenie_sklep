namespace Shop.Server.Messages;

public record StockAvailable(Guid OrderId, int Quantity);
