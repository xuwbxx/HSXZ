using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Query();

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter);

        TEntity GetByID(object id);

        void Insert(TEntity entity);
        void Delete(object id);
        void Update(TEntity entity);

        void AddList(IEnumerable<TEntity> instancelist);

        void DeleteEntity(TEntity instance);

        void DeleteList(IEnumerable<TEntity> instancelist);


    }
}
