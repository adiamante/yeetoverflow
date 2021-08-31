using System;

namespace YeetOverFlow.Core
{
    //https://app.pluralsight.com/library/courses/domain-driven-design-in-practice/table-of-contents
    public abstract class Entity
    {
        public virtual Guid Guid { get; }

        public Entity()
        {
            Guid = Guid.NewGuid();
        }

        public Entity(Guid guid)
        {
            Guid = guid;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            //if (GetRealType() != other.GetRealType())
            //    return false;

            return Guid == other.Guid;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            //return (GetRealType().ToString() + Id).GetHashCode();
            return Guid.GetHashCode();
        }

        //NHibernate
        //private Type GetRealType()
        //{
        //    return NHibernateProxyHelper.GetClassWithoutInitializingProxy(this);
        //}

        //Entity Framework
        //private Type GetRealType()
        //{
        //    Type type = GetType();

        //    if (type.ToString().Contains("Castle.Proxies."))
        //        return type.BaseType;

        //    return type;
        //}
    }
}
