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
        protected string _connectionalias;

        public UnitOfWorkEF(string connectionalias = "default")
        {
            _connectionalias = connectionalias;
            _context = new AppContext(AppConfigConnectionFactory.GetConnectionString(_connectionalias, Settings.New()));
            _context.Configuration.LazyLoadingEnabled = false;
            _transaction = _context.Database.BeginTransaction();

        }

        public void BeginTransaction()
        {

           
        }

        public void Commit()
        {
            _context.SaveChanges();
            if (_transaction != null)
            {
                _transaction.Commit();
                //start a new transaction
                _transaction = _context.Database.BeginTransaction();
            }
        }

        public void Dispose()
        {
            //if (_transaction != null)
            //{
            //    _transaction.Rollback();
            //    _transaction.Dispose();
            //}
            //_transaction = null;
            //for singleton instance keep context open
            _context.Dispose();
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                //start a new transaction
                _transaction = _context.Database.BeginTransaction();
            }
        }
    }
}
