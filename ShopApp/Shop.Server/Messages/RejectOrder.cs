namespace Shop.Server.Messages;

public record RejectOrder(Guid OrderId, int Quantity);
