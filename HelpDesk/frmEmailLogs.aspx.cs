using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HelpDesk_frmEmailLogs : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] != null & Session["LoginName"] != null && Session["UserScope"] != null && Session["EmpID"] != null)
            {
                if (!IsPostBack)
                {
                    FillEmail();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }

    protected void FillEmail()
    {
        try
        {

            DataTable SD_Scope = new FillSDFields().FillEmail();
            if (SD_Scope.Rows.Count > 0)
            {
                this.gvAllTickets.DataSource = (object)SD_Scope;
                this.gvAllTickets.DataBind();
                GridFormat(SD_Scope);
            }
            else
            {
                this.gvAllTickets.DataSource = (object)null;
                this.gvAllTickets.DataBind();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void GridFormat(DataTable dt)
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
        if (dt.Rows.Count > 0)
            gvAllTickets.FooterRow.TableSection = TableRowSection.TableFooter;
    }

    protected void lnkView_Click(object sender, EventArgs e)
    {
        try
        {
            mp1.Show();
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            HiddenField hdnbodyContent = (HiddenField)gvr.FindControl("hdnbodyContent");
            if (hdnbodyContent != null && !string.IsNullOrEmpty(hdnbodyContent.Value))
            {
                string body = HttpUtility.HtmlDecode(hdnbodyContent.Value);
                lblBody.Text = body;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "ShowModal()", true);
            }
            else
            {
                lblBody.Text = "Content not available.";
            }


            FillEmail();
            DataTableScript();
        }
        catch (Exception ex)
        {
            lblBody.Text = $"Error: {ex.Message}";
        }
    }
    public void DataTableScript()
    {
        string jqueryScript = "<script src='assetsdata/js/jquery.min.js'></script>";
        string dataTableScript = "<script src='assetsdata/plugins/datatable/js/jquery.dataTables.min.js'></script>";
        string dataTableBootstrapScript = "<script src='assetsdata/plugins/datatable/js/dataTables.bootstrap5.min.js'></script>";

        ClientScript.RegisterStartupScript(this.GetType(), "jqueryScript", jqueryScript, false);
        ClientScript.RegisterStartupScript(this.GetType(), "dataTableScript", dataTableScript, false);
        ClientScript.RegisterStartupScript(this.GetType(), "dataTableBootstrapScript", dataTableBootstrapScript, false);

        string script = @"
    <script type='text/javascript'>
        $(document).ready(function () {
            $('.data-table1').DataTable();
        });
    </script>";

        // Use ScriptManager for partial postbacks or ClientScript for full postbacks
        ScriptManager.RegisterStartupScript(this, GetType(), "initializeDataTable", script, false);
    }
    protected void OpenModal()
    {
        string script = "$('#ScrollableModal').modal('show');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", script, true);
    }

    protected void CloseModal()
    {
        string script = "$('#ScrollableModal').modal('hide');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", script, true);
    }

    protected void btnclose_Click(object sender, EventArgs e)
    {
        Response.Redirect("/HelpDesk/frmEmailLogs.aspx");
    }
}