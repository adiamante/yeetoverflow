using System;

namespace YeetOverFlow.Core.Application.Data.Core
{
    //https://www.dotnetcurry.com/patterns-practices/1461/command-query-separation-cqs
    //https://github.com/timsommer/cqs-dotnetcurry-sample
    public interface IRequest
    {
        Guid CorrelationId { get; set; }
    }
}
