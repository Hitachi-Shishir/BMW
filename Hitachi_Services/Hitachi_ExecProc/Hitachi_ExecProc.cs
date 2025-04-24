using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Hitachi_ExecProc
{
	public partial class Hitachi_ExecProc : ServiceBase
	{
		System.Timers.Timer timer = new System.Timers.Timer();
		InsertLogs inEr = new InsertLogs();
		public Hitachi_ExecProc()
		{
			InitializeComponent();
		}

	
	

		protected override void OnStart(string[] args)
		{
			timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
			timer.Interval = 30000; //number in milisecinds  
			timer.Enabled = true;
		}
		private void OnElapsedTime(object source, ElapsedEventArgs e)
		{
			ExecuteProc();
		}
		private void ExecuteProc()
		{

			try
			{
				//Sendmail(body);
				using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
				{

					using (SqlCommand cmd = new SqlCommand("sp_AllSDProc", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
					}
				}
			}
			catch (ThreadAbortException e2)
			{
				Console.WriteLine("Exception message: {0}", e2.Message);
				Thread.ResetAbort();
			}
			catch (Exception ex)
			{
				if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
				{

				}
				else
				{
					var st = new StackTrace(ex, true);
					// Get the top stack frame
					var frame = st.GetFrame(0);
					// Get the line number from the stack frame
					var line = frame.GetFileLineNumber();
					inEr.InsertErrorLogsF("System Generated",
		  "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());


				}
			}

		}


		protected override void OnStop()
		{
		}
	}
}
