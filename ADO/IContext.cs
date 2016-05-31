namespace Your.Business.ADO
{
    public interface IContext
    {
        IDbCommand CreateCommand();
        //IUnitOfWork CreateUnitOfWork();
        void AddParameter(IDbCommand command, string name, DbType dbType, object value, ParameterDirection direction);
        void Dispose();
    }
}
