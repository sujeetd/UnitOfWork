using System.Collections.Generic;
using Your.Entity;

namespace Your.Business
{
    public interface IUserBL
    {
        void Delete(int id);
        IEnumerable<User> Get();
        User Get(int id);
        void Post(User value);
        void Put(int id, User value);

        void OnExecuting();
        void OnExecuted(bool isRollback);

        bool UseAdo { get; set; }
    }
}
