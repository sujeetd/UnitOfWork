using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business
{
    public class UserUnitOfWorkAdo : UnitOfWorkAdoNet , IUserUnitOfWork
    {

        public UserUnitOfWorkAdo( ILog log, string connectionalias = "default")            
            :base(connectionalias)
        {
            Users = new UserRepositoryAdo(log, _context);
            UserRoles = new UserRoleRepositoryAdo(log, _context);
            //add related child entities here like UserRoles
        }

        public IUserRepository Users { get; private set; }
        public IUserRoleRepository UserRoles { get; private set; }
    }

}
