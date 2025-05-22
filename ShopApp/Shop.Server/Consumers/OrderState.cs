using MassTransit;

namespace Shop.Server.Consumers;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public string CurrentState { get; set; } = "";

    public bool StockConfirmed { get; set; } = default!;
    public bool ClientConfirmed { get; set; } = default!;

    public DateTime? OrderDate { get; set; } = default!;

    public Guid? TimeoutTokenId { get; set; } = default!;

}
