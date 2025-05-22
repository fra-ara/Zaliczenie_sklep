namespace Shop.Server.Messages;

public record StockUnavailable(Guid OrderId, int Quantity);
