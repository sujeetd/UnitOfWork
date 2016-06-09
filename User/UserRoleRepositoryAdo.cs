using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;

namespace Your.Business
{
    public class UserRoleRepositoryAdo : IUserRoleRepository
    {
        #region Constructor
        private readonly ILog _log;
        private readonly IContext _context;
        private List<UserRole> _repository;
        public UserRoleRepositoryAdo(ILog log, IContext context)
        {
            _log = log;
            _context = context;
            _repository = new List<UserRole>();
        }
        #endregion

        #region GetAll
        public IEnumerable<UserRole> GetAll()
        {
            using (_log.BeginScope())
            {
                try
                {
                    _log.Write("Retrieving all UserRoles");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"SELECT id, firstname, middlename, lastname  FROM UserRoles";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new UserRole();
                                Map(reader, item);
                                _repository.Add(item);
                            }
                            return _repository;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw;
                }
            }
        }

        private void Map(IDataReader dr, UserRole entity)
        {
            int idxid, idxuserid, idxroleid, idxrole;
            idxid = dr.GetOrdinal("id");
            idxuserid = dr.GetOrdinal("userid");
            idxroleid = dr.GetOrdinal("roleid");
            idxrole = dr.GetOrdinal("role");
            entity.ID = dr.IsDBNull(idxid) ? 0 : dr.GetInt32(idxid);
            entity.UserId = dr.IsDBNull(idxuserid) ? 0 : dr.GetInt32(idxuserid);
            entity.RoleId = dr.IsDBNull(idxroleid) ? 0 : dr.GetInt32(idxroleid);
            entity.Role = dr.IsDBNull(idxrole) ? string.Empty : dr.GetString(idxrole);
        }
        #endregion

        #region Get
        public UserRole Get(int id)
        {
            using (_log.BeginScope())
            {
                try
                {
                    _log.Write("Retrieving all UserRoles");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"SELECT id, firstname, middlename, lastname  FROM UserRoles Where id= @id";
                        _context.AddParameter(command, "id", System.Data.DbType.Int32, id, System.Data.ParameterDirection.Input);
                        using (var reader = command.ExecuteReader())
                        {
                            UserRole UserRole = null;
                            if (reader.Read())
                            {
                                UserRole = new UserRole();
                                //map fields 
                                Map(reader, UserRole);

                            }
                            return UserRole;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw;
                }
            }
        }


        #endregion

        public IEnumerable<UserRole> Find(Expression<Func<UserRole, bool>> predicate)
        {
            using (_log.BeginScope())
            {
                _log.Write("Find all UserRoles using predicate");
                //if (_repository == null || _repository.Count <= 0)
                //    GetAll();                                
                //return _repository.AsQueryable().Where(predicate);

                //other possible way for pure sql based predicate
                var repo = new List<UserRole>();
                try
                {
                    _log.Write("Retrieving all UserRoles");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"SELECT id, userid, roleid, role  FROM UserRoles ";// + GetWhereFromPredicate(predicate);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new UserRole();
                                Map(reader, item);
                                repo.Add(item);
                                _repository.Add(item);
                            }
                            return repo.AsQueryable().Where(predicate).ToList(); 
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw;
                }
                
            }
        }

        public static string GetWhereFromPredicate(Expression<Func<UserRole, bool>> exp)
        {
            //Expression<Func<Product, bool>> exp = (x) => (x.Id > 5 && x.Warranty != false);

            string expBody = ((LambdaExpression)exp).Body.ToString();
            // Gives: ((x.Id > 5) AndAlso (x.Warranty != False))

            var paramName = exp.Parameters[0].Name;
            var paramTypeName = exp.Parameters[0].Type.Name;

            // You could easily add "OrElse" and others...
            expBody = expBody.Replace(paramName + ".", paramTypeName + ".")
                             .Replace("AndAlso", "and").Replace("OrElse", "or");

            return expBody;

        }

        public UserRole Add(UserRole entity)
        {
            using(_log.BeginScope())
            {
                try
                {
                    _log.Write("Adding UserRole");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO UserRoles (userid, roleid, role, editdate, createdate)  VALUES( @userid, @roleid, @role, SYSDATETIME(), SYSDATETIME());  SELECT @id = SCOPE_IDENTITY();";
                        _context.AddParameter(command, "userid", System.Data.DbType.Int32, entity.UserId, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "roleid", System.Data.DbType.Int32, entity.RoleId, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "role", System.Data.DbType.String, entity.Role, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "id", System.Data.DbType.Int32, entity.ID, System.Data.ParameterDirection.Output);
                        int ctr = command.ExecuteNonQuery();
                        var outparam = command.Parameters["id"] as IDbDataParameter;
                        entity.ID = Convert.ToInt32(outparam.Value);
                        return entity;
                    }
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw;
                }
            }
        }

        public UserRole Remove(UserRole entity)
        {
            using(_log.BeginScope())
            {
                try
                {
                    _log.Write("Retrieving all UserRoles");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"DELETE FROM UserRoles Where id= @id";
                        _context.AddParameter(command, "id", System.Data.DbType.Int32, entity.ID, System.Data.ParameterDirection.Input);
                        int ctr = command.ExecuteNonQuery();
                        entity.ID = 0;
                        return entity;
                    }
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw;
                }
            }
        }

        public UserRole Update(UserRole entity)
        {
            using (_log.BeginScope())
            {
                try
                {
                    _log.Write("Adding UserRole");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"UPDATE UserRoles set userid=@userid, roleid=@roleid, role=@role, editdate= SYSDATETIME() WHERE id= @id ";
                        _context.AddParameter(command, "userid", System.Data.DbType.Int32, entity.UserId, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "roleid", System.Data.DbType.Int32, entity.RoleId, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "role", System.Data.DbType.String, entity.Role, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "id", System.Data.DbType.Int32, entity.ID, System.Data.ParameterDirection.Input);
                        int ctr = command.ExecuteNonQuery();
                        return entity;
                    }
                }
                catch (Exception ex)
                {
                    _log.Write(ex);
                    throw;
                }
            }
        }
    }
}
