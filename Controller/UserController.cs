using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Your.Business;
using Your.Entity;
using Microsoft.AspNet.Mvc.Filters;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Your.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        IUserBL _userBL;
        ILog _log;
        public UserController(ILog log, IUserBL userBL)
        {
            _userBL = userBL;
            _userBL.UseAdo = true;
            _log = log;            
        }
       // GET: api/values
       [HttpGet]
        public IEnumerable<User> Get()
        {
            using (_log.BeginScope())
            {
                try
                {
                    return _userBL.Get();
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }
            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            using (_log.BeginScope())
            {
                try
                {                   
                    return _userBL.Get(id);
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }
            
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]User value)
        {
            using (_log.BeginScope())
            {
                try
                {
                    if (!this.ModelState.IsValid)
                    {
                        _log.Write("The model is not valid");
                        return;
                    }
                    _userBL.Post(value);
                    return;
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]User value)
        {
            using (_log.BeginScope())
            {
                try
                {
                    if (!this.ModelState.IsValid)
                    {
                        _log.Write("The model is not valid");
                        return;
                    }
                    if (value != null && id == value.ID)
                        _userBL.Put(id, value);
                    return;
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }

           
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (_log.BeginScope())
            {
                try
                {
                    if (!this.ModelState.IsValid)
                    {
                        _log.Write("The model is not valid");
                        return;
                    }
                    _userBL.Delete(id);
                    return;
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw new CustomException(ex, "An error occurred while attempting to post the specified sample");
                }
            }
                      
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _userBL.OnExecuting();
            //_uow.BeginTransaction();
            //UnitOfWork = filterContext.Request.GetDependencyScope().GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            //UnitOfWork.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _userBL.OnExecuted(filterContext.Exception != null);
            //if (filterContext.Exception == null)
            //    _uow.Commit();
            //else
            //   _uow.Rollback();
            ////if not singleton DI then use dispose
            //_uow.Dispose();  
        }
    }

}
