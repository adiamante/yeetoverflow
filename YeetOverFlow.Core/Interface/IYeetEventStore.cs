namespace YeetOverFlow.Core.Interface
{
    //https://medium.com/ingeniouslysimple/command-vs-event-in-domain-driven-design-be6c45be52a9
    public interface IYeetEventStore<TEvent, TChild>
        where TEvent : IYeetEvent<TChild>
    {
        void DispatchEvent(TEvent domainEvent);
    }
}
