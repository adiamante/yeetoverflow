using System;

namespace YeetOverFlow.Core.Application.Data.Core
{
    public abstract class Request : IRequest
    {
        public Guid CorrelationId { get; set; }

        protected Request()
        {
            CorrelationId = Guid.NewGuid();
        }
    }
}
