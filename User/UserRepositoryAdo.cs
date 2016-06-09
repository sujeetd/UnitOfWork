using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;

namespace Your.Business
{
     public class UserRepositoryAdo : IUserRepository
    {
        #region Constructor
        private readonly ILog _log;
        private readonly IContext _context;
        private List<User> _repository;
        public UserRepositoryAdo(ILog log, IContext context)
        {
            _log = log;
            _context = context;
            _repository = new List<User>();
        }
        #endregion

        #region GetAll
        public IEnumerable<User> GetAll()
        {
            using (_log.BeginScope())
            {
                try
                {
                    _log.Write("Retrieving all users");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"SELECT id, firstname, middlename, lastname, editdate, createdate, isactive  FROM Users";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new User();
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

        private void Map(IDataReader dr, User entity)
        {
            int idxid, idxfirstname, idxmiddlename, idxlastname, idxeditdate, idxcreatedate, idxisactive;
            idxid = dr.GetOrdinal("id");
            idxfirstname = dr.GetOrdinal("firstname");
            idxmiddlename = dr.GetOrdinal("middlename");
            idxlastname = dr.GetOrdinal("lastname");
            idxeditdate = dr.GetOrdinal("editdate");
            idxcreatedate = dr.GetOrdinal("createdate");
            idxisactive = dr.GetOrdinal("isactive");
            entity.ID = dr.IsDBNull(idxid) ? 0 : dr.GetInt32(idxid);
            entity.FirstName = dr.IsDBNull(idxfirstname) ? string.Empty: dr.GetString(idxfirstname);
            entity.MiddleName = dr.IsDBNull(idxmiddlename) ? string.Empty : dr.GetString(idxmiddlename);
            entity.LastName = dr.IsDBNull(idxlastname) ? string.Empty : dr.GetString(idxlastname);
            entity.EditDate = dr.IsDBNull(idxeditdate) ? DateTime.MinValue : dr.GetDateTime(idxeditdate);
            entity.CreateDate = dr.IsDBNull(idxcreatedate) ? DateTime.MinValue : dr.GetDateTime(idxcreatedate);
            entity.IsActive = dr.IsDBNull(idxisactive) ? 0 : dr.GetInt32(idxisactive);

        }
        #endregion

        #region Get
        public User Get(int id)
        {
            using (_log.BeginScope())
            {
                try
                {
                    _log.Write("Retrieving all users");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"SELECT id, firstname, middlename, lastname, editdate, createdate, isactive  FROM Users Where id= @id";
                        _context.AddParameter(command, "id", System.Data.DbType.Int32, id, System.Data.ParameterDirection.Input);
                        using (var reader = command.ExecuteReader())
                        {
                            User user = null;
                            if (reader.Read())
                            {
                                user = new User();
                                //map fields 
                                Map(reader, user);

                            }
                            return user;
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

        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate)
        {
            using (_log.BeginScope())
            {
                _log.Write("Find all users using predicate");
                //if (_repository == null || _repository.Count <= 0)
                //    GetAll();                                
                //return _repository.AsQueryable().Where(predicate);

                //other possible way for pure sql based predicate
                var repo = new List<User>();
                try
                {
                    _log.Write("Retrieving all users");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"SELECT id, firstname, middlename, lastname, editdate, createdate, isactive  FROM Users ";//  Where " + GetWhereFromPredicate(predicate);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new User();
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

        public static string GetWhereFromPredicate(Expression<Func<User, bool>> exp)
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

        public User Add(User entity)
        {
            using(_log.BeginScope())
            {
                try
                {
                    _log.Write("Adding user");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Users (firstname, middlename, lastname, age, editdate, createdate)  VALUES( @firstname, @middlename, @lastname, @age, SYSDATETIME(), SYSDATETIME());  SELECT @id = SCOPE_IDENTITY();";
                        _context.AddParameter(command, "@firstname", System.Data.DbType.String, entity.FirstName, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "@middlename", System.Data.DbType.String, entity.MiddleName, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "@lastname", System.Data.DbType.String, entity.LastName, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "@age", System.Data.DbType.Int32, entity.Age, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "@id", System.Data.DbType.Int32, entity.ID, System.Data.ParameterDirection.Output);
                        int ctr = command.ExecuteNonQuery();
                        var outparam = command.Parameters["@id"] as IDbDataParameter; 
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

        public User Remove(User entity)
        {
            using(_log.BeginScope())
            {
                try
                {
                    _log.Write("Retrieving all users");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"DELETE FROM Users Where id= @id";
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

        public User Update(User entity)
        {
            using (_log.BeginScope())
            {
                try
                {
                    _log.Write("Adding user");
                    using (var command = _context.CreateCommand())
                    {
                        command.CommandText = @"UPDATE Users set firstname=@firstname, middlename=@middlename, lastname=@lastname, age=@age, editdate= SYSDATETIME() WHERE id= @id ";
                        _context.AddParameter(command, "firstname", System.Data.DbType.String, entity.FirstName, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "middlename", System.Data.DbType.String, entity.MiddleName, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "lastname", System.Data.DbType.String, entity.LastName, System.Data.ParameterDirection.Input);
                        _context.AddParameter(command, "age", System.Data.DbType.Int32, entity.Age, System.Data.ParameterDirection.Input);
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
