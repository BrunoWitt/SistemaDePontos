// Data/Database.cs
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
public static class Database
{
    public static SqlConnection GetConnection()
    {
        return new SqlConnection("Server=localhost;Database=PontoEletronico;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}
