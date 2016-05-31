using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business.EF
{
    public class RepositoryEF<TModelType, TKey> : IRepository2<TModelType, TKey> where TModelType : IEntity<TKey>
    {
        protected readonly AppContext Context;

        public RepositoryEF(AppContext context)
        {
            Context = context;
        }
        public TModelType Add(TModelType entity)
        {
           return Context.Set<TModelType>().Add(entity);
        }

        public IEnumerable<TModelType> Find(System.Linq.Expressions.Expression<Func<TModelType, bool>> predicate)
        {
            return Context.Set<TModelType>().Where(predicate);
        }

        public TModelType Get(TKey id)
        {
            return Context.Set<TModelType>().Find(id);
        }

        public IEnumerable<TModelType> GetAll()
        {
            return Context.Set<TModelType>().ToList();
        }

        public TModelType Remove(TModelType entity)
        {
            return Context.Set<TModelType>().Remove(entity);
        }

        public TModelType Update(TModelType entity)
        {
            return entity; //update occurs on unit of work commit
        }
    }

    public class AppContext : DbContext
    {
        public AppContext(string conn)
            : base(conn)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
