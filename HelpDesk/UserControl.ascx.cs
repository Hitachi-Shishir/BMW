using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl : System.Web.UI.UserControl
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {

        FillOrganization();

    }
    Random r = new Random();
    private void FillOrganization()
    {

        try
        {

            DataTable SD_Org = new FillSDFields().FillOrganization(); ;

            ddlOrg.DataSource = SD_Org;
            ddlOrg.DataTextField = "OrgName";
            ddlOrg.DataValueField = "Org_ID";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Organization----------", "0"));


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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void SaveData()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spRequestType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@ReqTypeRef ", txtRequestType.Text);
                    cmd.Parameters.AddWithValue("@ReqTypeDef", txtReqDescription.Text);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                    //   cmd.Parameters.AddWithValue("@IsActive", '1');
                    cmd.Parameters.AddWithValue("@Option", "AddRequestType");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Insert";
                        //Response.Redirect(Request.Url.AbsoluteUri + "?ShowPanelAdd=true");
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('~/frmEsclationMaster.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        //Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
   $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                    }
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
   $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void btnSaveReqType_Click(object sender, EventArgs e)
    {
        SaveData();

    }
}