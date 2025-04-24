using System.Data.SqlClient;

/// <summary>
/// Summary description for IDatabaseConnection
/// </summary>
public interface IDatabaseConnection
{
    SqlConnection GetConnection();
    SqlConnection CloseConnection();
}