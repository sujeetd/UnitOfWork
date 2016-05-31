using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business
{
    public class UserRoleRepositoryEF : RepositoryEF<UserRole, int>, IUserRoleRepository
    {
        public UserRoleRepositoryEF(AppContext context)
            : base(context)
        {
        }
    }

}
