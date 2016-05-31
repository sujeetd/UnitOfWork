using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business.EF
{
    public class UnitOfWorkEF : IUnitOfWork
    {
        protected AppContext _context;
        protected DbContextTransaction _transaction;

        public UnitOfWorkEF(string connectionalias = "default")
        {
            _context = new AppContext(AppConfigConnectionFactory.GetConnectionString(connectionalias, Settings.New()));
            _context.Configuration.LazyLoadingEnabled = false;
            //_transaction = _context.Database.BeginTransaction();            
        }

        public void BeginTransaction()
        {

            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.SaveChanges();
            if (_transaction != null)
                _transaction.Commit();
            
        }

        public void Dispose()
        {
            _context.Dispose();
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
            _transaction = null;
        }

        public void Rollback()
        {
            if (_transaction != null)
                _transaction.Rollback();
        }
    }
}
