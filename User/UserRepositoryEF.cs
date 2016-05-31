using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business
{
    public class UserRepositoryEF: RepositoryEF<User, int>,IUserRepository
    {
        public UserRepositoryEF(AppContext context) 
            : base(context)
        {
        }
    }
}
