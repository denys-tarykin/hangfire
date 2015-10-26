using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Hangfire.Dao.Common;
using Hangfire.Domain.Api.Common;
using Hangfire.Domain.EntityFrameworkImpl.Common;
using Microsoft.Practices.Unity;

namespace Hangfire.Dao.EntityFrameworkImpl.Common
{
    public class GenericEntityFrameworkDaoIdentifiableSupport<I, T> : GenericEntityFrameworkDao<I, T>,
         IGenericIdentifiableDao<I>
        where T : BaseIdentifiableDbDomain, I
        where I : class, IDbDomain, IIdentifiable
    {
        public GenericEntityFrameworkDaoIdentifiableSupport(IUnityContainer unityContainer)
        {
           
        }

        protected virtual DbQuery<T> Find(bool onlyActiveRecords = true)
        {
            DbQuery<T> res = GetDbSet();
            if (onlyActiveRecords)
            {
                res = res.Where(obj => obj.EntityStatus != EntityStatus.Deleted) as DbQuery<T>;
            }
            return res;
        }

        public virtual I GetById(long id)
        {
            return Find().FirstOrDefault(I => I.Id.Equals(id));
        }

        public virtual void Fetch<Q>(T entity, Expression<Func<T, ICollection<Q>>> navigationProperty) where Q : class, IIdentifiable
        {
            if (entity == null) return;
            FetchQuery(entity, navigationProperty).Load();
        }

        public virtual IQueryable<Q> FetchQuery<Q, R>(R entity, Expression<Func<R, ICollection<Q>>> navigationProperty)
            where Q : class, IIdentifiable
            where R : BaseIdentifiableDbDomain
        {
            return GetDbContext()
                .Entry(entity)
                .Collection(navigationProperty)
                .Query()
                .Where(ar => ar.EntityStatus != EntityStatus.Deleted);
        }

        public virtual I AddOrUpdate(I entity, bool flush = false)
        {
            if (entity.Id == 0)
            {
                return Add(entity, flush);
            }
            else
            {
                return Update(entity, flush);
            }
        }
    }
}