namespace Shop.Server.Messages;

public record OrderRejected(Guid OrderId, int Quantity);
