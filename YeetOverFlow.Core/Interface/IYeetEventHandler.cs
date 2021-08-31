namespace YeetOverFlow.Core
{
    public interface IYeetEventHandler<T>
            where T : IYeetEvent<T>
    {
        void Handle(T domainEvent);
    }
}
