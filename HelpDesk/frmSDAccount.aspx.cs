using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HelpDesk_frmSDAccount : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserScope"] != null)
        {
            if (!IsPostBack)
            {
                FillSDAccount();
            }
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    protected void FillSDAccount()
    {
        DataTable SD_SDAccount = new FillSDFields().FillSDScope();

        if (SD_SDAccount.Rows.Count > 0)
        {
            rptAccount.DataSource = SD_SDAccount;
            rptAccount.DataBind();
        }
    }
    protected void rptAccount_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
    protected void rptAccount_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Master")
        {
            lblsofname.Text = "Master Details";
            FillSDAccount("Master");

        }
        else if (e.CommandName == "Technician")
        {
            lblsofname.Text = "Technician Details";
            FillSDAccount("Technician");
        }
        else if (e.CommandName == "SDUser")
        {
            lblsofname.Text = "SD User Details";
            FillSDAccount("SDUser");
        }
        else if (e.CommandName == "Admin")
        {
            lblsofname.Text = "Admin User Details";
            FillSDAccount("Admin");
        }
    }
    protected void FillSDAccount(string UserScope)
    {
        try
        {
            DataTable SD_SDAccount = new FillSDFields().FillSDAccount(UserScope);
            if (SD_SDAccount.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvSDAccount.DataSource = (object)SD_SDAccount;
                this.gvSDAccount.DataBind();
            }
            else
            {
                this.gvSDAccount.DataSource = (object)null;
                this.gvSDAccount.DataBind();
            }
            if (SD_SDAccount.Rows.Count > 0)
            {
                GridFormat(SD_SDAccount);
            }
        }

        catch (Exception ex)
        {
            // msg.ReportError(ex.Message);
        }
    }
    protected void GridFormat(DataTable dt)
    {
        gvSDAccount.UseAccessibleHeader = true;
        gvSDAccount.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvSDAccount.TopPagerRow != null)
        {
            gvSDAccount.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvSDAccount.BottomPagerRow != null)
        {
            gvSDAccount.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvSDAccount.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    protected void ImgBtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvSDAccount.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvSDAccount.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=SDAccounts.xlsx");
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
}