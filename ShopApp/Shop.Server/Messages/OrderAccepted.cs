namespace Shop.Server.Messages;

public record OrderAccepted(Guid OrderId, int Quantity);
