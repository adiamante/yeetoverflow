using System.Threading.Tasks;
using YeetOverFlow.Core.Application.Data.Core;

namespace YeetOverFlow.Core.Application.Data.Queries
{
    /// <summary>
    /// Base interface for query handlers
    /// </summary>
    /// <typeparam name="TParameter">Request type</typeparam>
    /// <typeparam name="TResult">Request Result type</typeparam>
    public interface IQueryHandler<in TParameter, TResult> where TResult : IResult where TParameter : IQuery
    {
        /// <summary>
        /// Retrieve a query result from a query
        /// </summary>
        /// <param name="query">Request</param>
        /// <returns>Retrieve Request Result</returns>
        TResult Handle(TParameter query);

        /// <summary>
        /// Retrieve a query result async from a query
        /// </summary>
        /// <param name="query">Request</param>
        /// <returns>Retrieve Request Result</returns>
        //Task<TResult> HandleAsync(TParameter query);
    }
}
