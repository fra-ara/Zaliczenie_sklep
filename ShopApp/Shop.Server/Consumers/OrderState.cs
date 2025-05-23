using MassTransit;
using System;

namespace Shop.Server.Consumers
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = default!;
        public int Quantity { get; set; }
        public Guid? TimeoutTokenId { get; set; }
    }
}
