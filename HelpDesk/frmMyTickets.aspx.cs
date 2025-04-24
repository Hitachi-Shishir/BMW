using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmMyTickets : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    public enum MessageType { success, error, info, warning };
    protected void ShowMessage(MessageType type, string Message)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
    }
    protected override void OnInit(EventArgs e)
    {
        try
        {
            //Change your condition here
            if (Session["Popup"] != null)
            {

                if (Session["Popup"].ToString() == "Error")
                {
                    ShowMessage(MessageType.error, "Choose Ticket First!!");


                }
                if (Session["Popup"].ToString() == "Pickup")
                {
                    ShowMessage(MessageType.success, "Ticket has been Assigned");
                }
                if (Session["Popup"].ToString() == "Delete")
                {
                    ShowMessage(MessageType.success, "Ticket has been Deleted");
                }
                Session.Remove("Popup");
            }
        }

        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            ExceptionLogging.SendErrorToText(ex);

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] != null & Session["LoginName"] != null && Session["UserScope"] != null && Session["EmpID"] != null)
            {
                if (!IsPostBack)
                {
                    FillUserWiseTickets();
                }
            }
            else
            {
                Response.Redirect("/Default.aspx");
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
    private void Modal()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#CategoryModal').modal('show');");
        //  sb.Append("$('#basicModal').modal.appendTo('body').show('show')");
        sb.Append("$('body').removeClass('modal-open');");
        sb.Append("$('.modal-backdrop').remove();");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);


    }
    protected void ShowPopup(object sender, EventArgs e)
    {
        try
        {
            string title = "Greetings";
            string body = "Welcome to ASPSnippets.com";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
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
    protected void FillUserWiseTickets()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spGetTicket", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TotalRow", "0");
      cmd.Parameters.AddWithValue("@SubmitterEmail", Convert.ToString(Session["EmailID"]));
                  //  cmd.Parameters.AddWithValue("@SubmitterID", Session["EmpID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "UserWise");
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                DataView dv = dt.DefaultView;
                                dv.Sort = "CreationDate DESC";
                                gvAllTickets.DataSource = dv;
                                gvAllTickets.DataBind();
                                GridFormat(dv);
                            }
                            else
                            {
                                gvAllTickets.DataSource = null;
                                gvAllTickets.DataBind();
                            }
                        }
                    }
                }
            }
        }
        catch(Exception ex)
        {

        }
    }

    protected void gvAllTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        FillUserWiseTickets();
        gvAllTickets.PageIndex = e.NewPageIndex;
        gvAllTickets.DataBind();
    }
    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvAllTickets.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvAllTickets.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=Holiday.xlsx");
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
    protected void GridFormat(DataView dt)
    {
        gvAllTickets.UseAccessibleHeader = true;
        gvAllTickets.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvAllTickets.TopPagerRow != null)
        {
            gvAllTickets.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvAllTickets.BottomPagerRow != null)
        {
            gvAllTickets.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        //if (dt.Rows.Count > 0)
        //    gvAllTickets.FooterRow.TableSection = TableRowSection.TableFooter;
    }
}