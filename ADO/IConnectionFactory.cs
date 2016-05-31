using System.Data;

namespace Your.Business.ADO
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}
