using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace YeetOverFlow.Core.Application.Persistence
{
    //https://codewithshadman.com/repository-pattern-csharp/
    //https://www.youtube.com/watch?v=rtXpYpZdOzM&feature=youtu.be
    //Benefits - Minimized duplicate query logic and decouples your application from persistance frameworks
    //Queries only exist in the Repository
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);
        void Delete(object id);
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>> includePropertyExpression = null);
        TEntity GetById(object id, Expression<Func<TEntity, object>> includePropertyExpression = null);
        //IEnumerable<TEntity> GetWithRawSql(string query,
        //    params object[] parameters);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        //IEnumerable<TEntity> GetRecursiveCollection(Expression<Func<TEntity, object>> collectionPropertyExpression, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, object>> includePropertyExpression = null);
        void RecursiveLoadCollection(TEntity entity, Expression<Func<TEntity, object>> propertyExpression);
        //void Save();
    }
}
