using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business.EF
{
    public class UserUnitOfWorkEF : UnitOfWorkEF, IUserUnitOfWork
    {
        
        public UserUnitOfWorkEF(string connectionalias="default")
            :base(connectionalias)
        {
            Users = new UserRepositoryEF(_context);
            UserRoles = new UserRoleRepositoryEF(_context);
            //add related child entities here like UserRoles
        }

        public IUserRepository Users { get; private set; }
        public IUserRoleRepository UserRoles { get; private set; }
    }
}
