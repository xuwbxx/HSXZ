
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Query()
        {
            return dbSet.ToList();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            return this.dbSet.Where(filter).AsQueryable();
        }

        public TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void AddList(IEnumerable<TEntity> instancelist)
        {
            this.dbSet.AddRange(instancelist);
        }

        public void Delete(object id)
        {
            TEntity entity = dbSet.Find(id);
            if (context.Entry(entity).State == EntityState.Deleted)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void DeleteEntity(TEntity instance)
        {
            this.dbSet.Attach(instance);
            this.context.Entry(instance).State = EntityState.Deleted;
        }

        public void DeleteList(IEnumerable<TEntity> instancelist)
        {
            this.dbSet.RemoveRange(instancelist);
            
        }



        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }


    }
}
