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
using ClosedXML.Excel;

public partial class frmMyTickets : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    public enum MessageType { success, error, info, warning };
    protected void ShowMessage(MessageType type, string Message)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OrgName"] != null)
        {
            Page.Title = Convert.ToString(Session["OrgName"]);
        }
        else
        {
            Page.Title = "Default Title";
        }
        if (Session["UserName"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        try
        {
            if (!IsPostBack)
            {
               
                if (Convert.ToString(Session["UserScope"]).ToLower() != "master")
                {
                    hdfnUserEmail.Value = Convert.ToString(Session["EmailID"]);
                }
                FillServDesk();
                //ddlRequestType.SelectedValue = "Incident";
                if (Request.QueryString["Desk"] != null)
                {
                    if(Request.QueryString["Desk"].ToString()=="Incident")
                    {
                        lblsofname.Text = "Issue Management Ticket Details";
                    }
                    else if(Request.QueryString["Desk"].ToString().Replace("%20","") == "Service Request")
                    {
                        lblsofname.Text = "Service Request Ticket Details";
btnCreatTicket.Text="New Request";
                    }
                    else if (Request.QueryString["Desk"].ToString().Replace("%20", "") == "Change Request")
                    {
                        lblsofname.Text = "Change Request Ticket Details";
                    }
                    FillUserWiseTickets(Request.QueryString["Desk"].ToString());
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
    private void FillServDesk()
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(Convert.ToInt64(Session["SD_OrgID"].ToString()));
            ddlRequestType.DataSource = RequestType;
            ddlRequestType.DataTextField = "ReqTypeRef";
            ddlRequestType.DataValueField = "ReqTypeRef";
            ddlRequestType.DataBind();
            ddlRequestType.Items.Insert(0, new ListItem("----------Select RequestType----------", "0"));
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
    protected void FillUserWiseTickets(string Desk)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_spGetTicket", con))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TotalRow", "0");
                cmd.Parameters.AddWithValue("@SubmitterEmail", Convert.ToString(Session["EmailID"]));
                cmd.Parameters.AddWithValue("@Desk", Desk.Replace("%20",""));
                cmd.Parameters.AddWithValue("@Option", "UserWiseViaEmail");
                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);

                        DataView dv = dt.DefaultView;

                        if (dt.Rows.Count > 0)
                        {
                            //	lblTotal.Text = dt.Rows.Count.ToString();

                            gvAllTickets.DataSource = dt;
                            gvAllTickets.DataBind();
                            GridFormat(dt);
                        }
                        else
                        {
                            //	lblTotal.Text = "0";
                            gvAllTickets.DataSource = null;
                            gvAllTickets.DataBind();
                        }
                        
                    }
                }
                con.Close();
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
                Response.AddHeader("content-disposition", "attachment;filename=MyTickets.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }


        }
        catch (Exception ex)
        {

        }
    }
    protected void lnkbtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmUserlanding.aspx");
    }

    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUserWiseTickets(ddlRequestType.SelectedValue);
    }

    protected void btnCreatTicket_Click(object sender, EventArgs e)
    {
        if(Request.QueryString["Desk"]!="")
        {
            string desk = "frmAddIncidentUsers.aspx?&redirected=true&Desk=" + Request.QueryString["Desk"].ToString();
            Response.Redirect(desk);
            //string desk = Request.QueryString["Desk"].ToString();
            //Response.Write("<script type='text/javascript'>");
            //Response.Write("window.open('/frmAddIncidentUsers.aspx?&redirected=true&Desk="+ desk + "','_blank');");
            //Response.Write("</script>");
        }
      
    }
 protected void gvAllTickets_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {



            if (e.CommandName == "TicketClick")
            {

                string ticketIdentifier = e.CommandArgument.ToString();

                // Loop through GridView rows to find the matching row
                foreach (GridViewRow row in gvAllTickets.Rows)
                {
                    // Find the Label that holds the ticket identifier in the row
                    Label lblOrgFK = row.FindControl("lblOrgFK") as Label;
                    Label lblDesk = row.FindControl("lblDesk") as Label;
                    if (lblOrgFK != null)
                    {
                        // Retrieve additional information from the row


                        // Get the values from the Labels
                        string orgFK = lblOrgFK != null ? lblOrgFK.Text : string.Empty;
                        string desk = lblDesk != null ? lblDesk.Text : string.Empty;

                        // Redirect to the edit page with the retrieved values
                        Response.Redirect("/frmUpdateUserNotes.aspx?TicketId=" + ticketIdentifier +
                   "&redirected=true&Desk=" + desk.ToString() +
                   "&NamelyId=" + orgFK.ToString()+ "&EmpID="+Convert.ToString(Session["EmailID"]));

                        break; // Exit the loop once the matching row is found
                    }
                }



            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
    }
}