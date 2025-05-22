namespace Shop.Server.Messages;

public record ConfirmOrder(Guid OrderId, int Quantity);
