using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Entity
{
    public class UserRole : IEntity<int>
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }

    }
}
