using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YeetOverFlow.Reflection;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.EntityFramework
{
    //https://codewithshadman.com/repository-pattern-csharp/
    public class EfRepository<TContext, TEntity> : IRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        protected TContext context;
        internal DbSet<TEntity> dbSet;

        public EfRepository(TContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        //public virtual IEnumerable<TEntity> GetWithRawSql(string query,
        //    params object[] parameters)
        //{
        //    return dbSet.FromSqlRaw(query, parameters).ToList();
        //}

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>> includePropertyExpression = null)
        {
            
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includePropertyExpression != null)
            {
                List<PropertyInfo> includeProperties = ReflectionHelper.GetMultiPropertyInfo<TEntity, object>(includePropertyExpression);
                foreach (PropertyInfo propertyInfo in includeProperties)
                {
                    query = query.Include(propertyInfo.Name);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public void RecursiveLoadCollection(TEntity entity, Expression<Func<TEntity, object>> propertyExpression)
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetPropertyInfo<TEntity, object>(propertyExpression);
            PropertyInfo prop = ReflectionHelper.PropertyInfoCollection[entity.GetType()][propertyInfo.Name];

            if (prop != null && context.Entry(entity).Member(propertyInfo.Name) is CollectionEntry)
            {
                context.Entry(entity).Collection(propertyInfo.Name).Load();
                IEnumerable col = (IEnumerable)prop.GetValue(entity);
                foreach (var item in col)
                {
                    if (item is TEntity subEntity)
                    {
                        RecursiveLoadCollection(subEntity, propertyExpression);
                    }
                }
            }
        }

        public virtual TEntity GetById(object id, Expression<Func<TEntity, object>> includePropertyExpression = null)
        {
            IQueryable<TEntity> query = dbSet;
            var entity = dbSet.Find(id);

            if (entity != null && includePropertyExpression != null)
            {
                List<PropertyInfo> includeProperties = ReflectionHelper.GetMultiPropertyInfo<TEntity, object>(includePropertyExpression);
                if (includePropertyExpression != null && includeProperties.Count > 0)
                {
                    foreach (PropertyInfo propertyInfo in includeProperties)
                    {
                        switch (context.Entry(entity).Member(propertyInfo.Name))
                        {
                            case ReferenceEntry refEntry:
                                context.Entry(entity).Reference(propertyInfo.Name).Load();
                                break;
                            case CollectionEntry colEntry:
                                context.Entry(entity).Collection(propertyInfo.Name).Load();
                                break;
                        }
                    }
                }
            }

            return entity;
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            //dbSet.Attach(entityToUpdate);     //maybe only attach if detached
            var entry = context.Entry(entityToUpdate);
            if (entry.State != EntityState.Added)
            {
                entry.State = EntityState.Modified;
            }
        }

        //public virtual void Attach(TEntity entityToAttach)
        //{
        //    dbSet.Attach(entityToAttach);
        //}

        //public virtual void Detach(TEntity entityToDetach)
        //{
        //    context.Entry(entityToDetach).State = EntityState.Detached;
        //}

        //public virtual void Save()
        //{
        //    context.SaveChanges();
        //}
    }
}
