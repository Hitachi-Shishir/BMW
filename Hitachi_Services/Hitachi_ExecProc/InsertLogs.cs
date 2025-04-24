using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hitachi_ExecProc
{
	public class InsertLogs
	{
		Random r = new Random();
		public void InsertErrorLogsF(string adminName, string Desc)
		{
			using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SD_spAddLogs", con))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@Id", r.Next());
					cmd.Parameters.AddWithValue("@adminName", adminName);
					cmd.Parameters.AddWithValue("@description", Desc);
					cmd.Parameters.AddWithValue("@Option", "InsertErrorLogs");
					con.Open();
					int res = cmd.ExecuteNonQuery();



				}
			}
		}
	}
}
