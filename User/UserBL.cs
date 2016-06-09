using Your.Business.EF;
using Your.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Business
{
    public class UserBL : IUserBL
    {
        IUserUnitOfWork _uow;
        ILog _log;
        public bool UseAdo { get; set; }
        public UserBL(ILog log)
        {            
            _log = log;
        }

        public IEnumerable<User> Get()
        {
            using (_log.BeginScope())
            {
                try
                {
                    return _uow.Users.GetAll();
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }

        }

        public User Get(int id)
        {
            using (_log.BeginScope())
            {
                try
                {
                    return _uow.Users.Get(id);
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }

        }

        public void Post(User value)
        {
            using (_log.BeginScope())
            {
                try
                {
                    _uow.Users.Add(value);
                    //commit changes to get new user id and use that for roles
                    _uow.Commit();
                    //let us add roles for this user 
                    UserRole adminrole = new UserRole() { UserId = value.ID, RoleId = 1, Role = "Admin" };
                    UserRole userrole = new UserRole() { UserId = value.ID, RoleId = 2, Role = "User" };
                    _uow.UserRoles.Add(adminrole);
                    _uow.UserRoles.Add(userrole);
                    return;
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }
        }

        public void Put(int id, User value)
        {
            using (_log.BeginScope())
            {
                try
                {                    
                    if (value != null && id == value.ID)
                        _uow.Users.Update(value);
                    return;
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }

        }

        public void Delete(int id)
        {
            using (_log.BeginScope())
            {
                try
                {                    
                    var user = _uow.Users.Get(id);
                    var userroles = _uow.UserRoles.Find(u => u.UserId == id);
                    foreach (var role in userroles)
                    {
                        if (role != null) _uow.UserRoles.Remove(role);
                    }
                    if (user != null)
                        _uow.Users.Remove(user);
                    return;
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }

        }

        public void OnExecuting()
        {
            if (UseAdo)
                _uow = new UserUnitOfWorkAdo(_log);
            else
                _uow = new UserUnitOfWorkEF();
            _uow.BeginTransaction();
        }

        public void OnExecuted(bool isRollback)
        {
            if(!isRollback)
                _uow.Commit();
            else
                _uow.Rollback();
            //if not singleton DI then use dispose
            _uow.Dispose();
        }
    }
}
