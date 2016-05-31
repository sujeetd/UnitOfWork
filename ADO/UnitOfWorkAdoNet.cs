using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business.ADO
{
    public class UnitOfWorkAdoNet : IUnitOfWork
    {
        private IDbTransaction _transaction;
        protected AdoNetContext _context;
        public UnitOfWorkAdoNet(string connectionalias = "default")
        {
            var factory = new AppConfigConnectionFactory(connectionalias, Settings.New());
            _context = new AdoNetContext(factory);
            //_transaction = _context.Connection.BeginTransaction();
            //_context.Transaction = _transaction;            
        }

        public IDbTransaction Transaction { get { return _transaction; }}
        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _context.Connection.BeginTransaction();
                _context.Transaction = _transaction;
            }
        }

        public void Dispose()
        {
            if (_transaction == null)
                return;

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public void Commit()
        {
            if (_transaction == null)
                return;

            _transaction.Commit();
            //_transaction = null;
        }

        
        public void Rollback()
        {
            if (_transaction == null)
                return;

            _transaction.Rollback();
            //_transaction = null;
        }
    }
}
