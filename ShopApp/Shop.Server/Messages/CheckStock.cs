namespace Shop.Server.Messages;

public record CheckStock(Guid OrderId, int Quantity);
