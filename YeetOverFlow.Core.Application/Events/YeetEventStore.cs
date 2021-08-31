using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.Application.Events
{
    public class YeetEventStore<TChild> : IYeetEventStore<YeetEvent<TChild>, TChild>
        where TChild : YeetItem
    {
        IRepository<YeetEvent<TChild>> _eventRepo;
        public YeetEventStore(IRepository<YeetEvent<TChild>> eventRepo)
        {
            _eventRepo = eventRepo;
        }
        public void DispatchEvent(YeetEvent<TChild> domainEvent)
        {
            _eventRepo.Insert(domainEvent);
        }
    }
}
