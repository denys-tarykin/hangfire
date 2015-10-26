using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Hangfire.Dao.Common;
using Hangfire.Domain.Api.Common;

namespace Hangfire.Dao.EntityFrameworkImpl.Common
{
    public class GenericEntityFrameworkDao<I, T> : EntityFrameworkDao, IGenericDao<I>
        where I : class, IDbDomain
        where T : class, I
    {

        protected virtual DbSet<T> GetDbSet()
        {
            return GetDbContext().Set<T>();
        }

        protected virtual bool InjectClientCheck()
        {
            return false;
        }

        public virtual I Add(I entity, bool flush = false)
        {
            T res = GetDbSet().Add(entity as T);
            if (flush)
            {
                GetDbContext().SaveChanges();
            }
            return res;
        }

        public virtual void Delete(I entity, bool flush = false)
        {
            if (entity is IIdentifiable)
            {
                ((IIdentifiable)entity).EntityStatus = EntityStatus.Deleted;
                GetDbContext().Entry(entity).State = EntityState.Modified;
            }
            else
            {
                GetDbSet().Remove(entity as T);
            }
            if (flush)
            {
                GetDbContext().SaveChanges();
            }
        }

        public virtual void DeleteRange(IEnumerable<I> entity, bool flush = false)
        {
            if (entity is IIdentifiable)
            {
                ((IIdentifiable)entity).EntityStatus = EntityStatus.Deleted;
                GetDbContext().Entry(entity).State = EntityState.Modified;
            }
            else
            {
                GetDbSet().Remove(entity as T);
            }
            if (flush)
            {
                GetDbContext().SaveChanges();
            }
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual I Update(I entity, bool flush = false)
        {
            GetDbContext().Entry(entity).State = EntityState.Modified;
            if (flush)
            {
                GetDbContext().SaveChanges();
            }
            return entity;
        }
    }
}