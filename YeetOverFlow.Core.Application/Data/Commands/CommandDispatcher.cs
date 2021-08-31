using System;
//using System.Threading.Tasks;
using YeetOverFlow.Core.Application.Data.Core;

namespace YeetOverFlow.Core.Application.Data.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TResult Dispatch<TParameter, TResult>(TParameter command) where TParameter : ICommand where TResult : IResult
        {
            //Look up the correct CommandHandler in our IoC container and invoke the Handle method

            ICommandHandler<TParameter, TResult> _handler = (ICommandHandler<TParameter, TResult>)_serviceProvider.GetService(typeof(ICommandHandler<TParameter, TResult>));
            return _handler.Handle(command);
        }

        //public async Task<TResult> DispatchAsync<TParameter, TResult>(TParameter command) where TParameter : ICommand where TResult : IResult
        //{
        //    //Look up the correct CommandHandler in our IoC container and invoke the async Handle method

        //    var _handler = _serviceProvider.GetService<ICommandHandler<TParameter, TResult>>();
        //    return await _handler.HandleAsync(command);
        //}
    }
}
