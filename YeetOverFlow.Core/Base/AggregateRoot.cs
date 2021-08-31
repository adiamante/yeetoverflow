using System;

namespace YeetOverFlow.Core
{
    //https://app.pluralsight.com/library/courses/domain-driven-design-in-practice/table-of-contents
    public abstract class AggregateRoot : Entity
    {
        public AggregateRoot() : base()
        {

        }
        public AggregateRoot(Guid guid) : base(guid)
        {

        }

        //private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        //public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        //protected virtual void AddDomainEvent(IDomainEvent newEvent)
        //{
        //    _domainEvents.Add(newEvent);
        //}

        //public virtual void ClearEvents()
        //{
        //    _domainEvents.Clear();
        //}
    }
}
