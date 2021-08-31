using YeetOverFlow.Core.Application.Events;

namespace YeetOverFlow.Core.Application.Persistence
{
    public interface IYeetUnitOfWork<TParent, TChild> : IUnitOfWork
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        IRepository<YeetLibrary<TParent>> YeetLibraries { get; }
        IRepository<TParent> YeetLists { get; }
        IRepository<TChild> YeetItems { get; }
        IRepository<YeetEvent<TChild>> Events { get; }
    }
}
