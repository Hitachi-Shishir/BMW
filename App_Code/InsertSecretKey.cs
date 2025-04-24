using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for InsertSecretKey
/// </summary>
public class InsertSecretKey
{
    public void InsertKeyForUser(string UserID, string secretKey)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("SD_spMFA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@SecretKey", secretKey);
                cmd.Parameters.AddWithValue("@Option", "InsertKey");
                con.Open();
                int res = cmd.ExecuteNonQuery();



            }
        }
    }
}