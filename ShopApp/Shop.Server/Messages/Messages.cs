using System;

namespace Shop.Server.Messages
{
    public record StartOrder(Guid OrderId, int Quantity);
    public record CheckStock(Guid OrderId, int Quantity);
    public record StockAvailable(Guid OrderId, int Quantity);
    public record StockUnavailable(Guid OrderId, int Quantity);
    public record RequestClientConfirmation(Guid OrderId, int Quantity);
    public record ClientConfirmed(Guid OrderId, int Quantity);
    public record ClientRejected(Guid OrderId, int Quantity);
    public record NotifyClientStockUnavailable(Guid OrderId);
    public class OrderTimeoutExpired
    {
        public Guid OrderId { get; set; }
    }
}
