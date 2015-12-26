using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Aggregator.Dal
{
    public class Dao<TKey, TDomainEntity> : IDisposable
    {
        private readonly ISession _dbSession;

        private readonly ITransaction _dbTransaction;

        public Dao()
        {
            _dbSession = DatabaseManager.SessionFactory.OpenSession();
            _dbTransaction = _dbSession.BeginTransaction();
        }

        public void Dispose()
        {
            _dbTransaction.Commit();
            _dbTransaction.Dispose();
            _dbSession.Dispose();
        }

        public TDomainEntity GetById(TKey key)
        {
            return _dbSession.Get<TDomainEntity>(key);
        }

        public TDomainEntity Save(TDomainEntity entity)
        {
            _dbSession.SaveOrUpdate(entity);
            return entity;
        }

        public void Delete(TDomainEntity entity)
        {
            _dbSession.Delete(entity);
        }

        public IQueryable<TDomainEntity> Query()
        {
            return _dbSession.Query<TDomainEntity>();
        }

        public IQueryable<TDomainEntity> Select(Func<TDomainEntity, bool> selector)
        {
            return Query().Where(selector).AsQueryable();
        }

        public TDomainEntity GetFirst(Func<TDomainEntity, bool> selector)
        {
            return Select(selector).FirstOrDefault();
        }
    }
}
