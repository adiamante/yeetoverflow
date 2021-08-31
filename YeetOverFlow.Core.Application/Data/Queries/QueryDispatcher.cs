using System;
//using System.Threading.Tasks;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Data.Queries;

namespace YeetOverFlow.Core.Application.Data.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TResult Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : IQuery
            where TResult : IResult
        {
            //Look up the correct QueryHandler in our IoC container and invoke the retrieve method

            IQueryHandler<TParameter, TResult> _handler = (IQueryHandler<TParameter, TResult>)_serviceProvider.GetService(typeof(IQueryHandler<TParameter, TResult>));
            return _handler.Handle(query);
        }

        //public async Task<TResult> DispatchAsync<TParameter, TResult>(TParameter query)
        //    where TParameter : IQuery
        //    where TResult : IResult
        //{
        //    //Look up the correct QueryHandler in our IoC container and invoke the retrieve method

        //    var _handler = _serviceProvider.GetService<IQueryHandler<TParameter, TResult>>();
        //    return await _handler.RetrieveAsync(query);
        //}
    }
}
