using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business
{
    public interface IUserUnitOfWork : IUnitOfWork
    {
         IUserRepository Users { get;  }
         IUserRoleRepository UserRoles { get;  }
    }
}
