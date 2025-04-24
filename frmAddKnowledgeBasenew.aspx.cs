using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmAddKnowledgeBasenew : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    public enum MessageType { success, error, info, warning };
    protected void ShowMessage(MessageType type, string Message)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserName"] != null)
            {
                if (!IsPostBack)
                {
                    FillShowResolution();
                    lblWelcomeMessage.Text = GetWelcomeMessage();
                }
            }
            else
            {
                Response.Redirect("/Default.aspx");
            }

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

    public string GetWelcomeMessage()
    {
        var currentTime = DateTime.Now.TimeOfDay;

        if (currentTime >= TimeSpan.FromHours(6) && currentTime < TimeSpan.FromHours(12))
        {
            return "Good Morning !";
        }
        else if (currentTime >= TimeSpan.FromHours(12) && currentTime < TimeSpan.FromHours(17))
        {
            return "Good Afternoon !";
        }
        else if (currentTime >= TimeSpan.FromHours(17) && currentTime < TimeSpan.FromHours(21))
        {
            return "Good Evening !";
        }
        else
        {
            return "Good AM !";
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }


    private void FillShowResolution()
    {

        try
        {
            string tech = "";
            string user = "";
            if (Session["UserRole"].ToString().ToLower() == "technician")
            {
                tech = "1";
            }
            else if (Session["UserRole"].ToString().ToLower() == "sduser")
            {
                user = "1";
            }
            DataTable SD_Priority = new FillSDFields().FillKnowledgeResolution(Convert.ToString(Session["SD_OrgID"]), user, tech);

            if (SD_Priority.Rows.Count > 0)
            {
                this.gvResolution.DataSource = (object)SD_Priority;
                this.gvResolution.DataBind();
                GridFormat(SD_Priority);
            }
            else
            {
                this.gvResolution.DataSource = (object)null;
                this.gvResolution.DataBind();
            }


        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
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
    protected void GridFormat(DataTable dt)
    {
        gvResolution.UseAccessibleHeader = true;
        gvResolution.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvResolution.TopPagerRow != null)
        {
            gvResolution.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvResolution.BottomPagerRow != null)
        {
            gvResolution.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvResolution.FooterRow.TableSection = TableRowSection.TableFooter;
    }

    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvResolution.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvResolution.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Priority.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmUserlanding.aspx");
    }
    protected void btnGO_Click(object sender, EventArgs e)
    {

    }
}