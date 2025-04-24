using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmTicketApprover : System.Web.UI.Page
{
    public object RequesterEmailID { get; private set; }

    errorMessage msg = new errorMessage();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("~/default.aspx");
        }
        if (Session["Result"] != null)
        {
            Session.Remove("Result");
        }

        if (Session["UserEmail"] != null)
        {
            if (!IsPostBack)
            {
                FillAllRequests();
            }
        }
    }
    private void FillAllRequests()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"select a.*,b.TicketNumber,b.Summary,b.SubmitterEmail,a.Approval1Status,
b.SubmitterName,b.Category,b.location,b.CreationDate,Priority,Status,Severity,ServiceDesk,Department,
c.Org_ID,c.OrgName from SD_SRApprovalStatus a WITH(NOLOCK)
inner join vSDTicket b WITH(NOLOCK) on a.TicketRef=b.TicketNumber and a.OrgId=b.OrgId
inner join SD_OrgMaster c WITH(NOLOCK)  on a.OrgId=c.Org_ID
where (Approval1Email=@Email or Approval2Email=@Email )  and (Approval1Status='Pending' and Approval2Status='Pending')  and (b.Status!='Resolved' and b.Status!='Closed' )  order by CreationDate DESC", con))
                {
                  cmd.Parameters.AddWithValue("@Email", Session["UserEmail"].ToString());
   //  cmd.Parameters.AddWithValue("@Email", "shishir.singh.jm@hitachi-systems.com");
                    //cmd.Parameters.AddWithValue("@OrgID", Session["SD_OrgID"].ToString());
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvHRRequest.DataSource = dt;
                        gvHRRequest.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            GridFormat(dt);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void GridFormat(DataTable dt)
    {
        gvHRRequest.UseAccessibleHeader = true;
        gvHRRequest.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvHRRequest.TopPagerRow != null)
        {
            gvHRRequest.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvHRRequest.BottomPagerRow != null)
        {
            gvHRRequest.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvHRRequest.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    protected void gvHRRequest_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Approve")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    string TicketRef = e.CommandArgument.ToString();
                    GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
                    Label Deskref = (row.FindControl("lblDesk") as Label);
                    Label OrgID = (row.FindControl("lblOrgFk") as Label);
 		Label ApprovalLevel= (row.FindControl("lbllevel") as Label);
                    LinkButton Prove_Button = (LinkButton)row.FindControl("btn_Prove");
                    string proc = "";
                    if (Convert.ToString(Deskref.Text).ToString() == "Service Request")
                    {
                        proc = "SD_SRTicketApprovalStatus_Manual";
                    }
                    else
                    {
                        proc = "SD_SRTicketApprovalStatus_CR";
                    }
                    using (SqlCommand cmd = new SqlCommand(proc, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ticketref", TicketRef);
                        cmd.Parameters.AddWithValue("@Remarks", "");
                        cmd.Parameters.AddWithValue("@OrgId", OrgID.Text);
                        cmd.Parameters.AddWithValue("@HODApproval", "");
                          cmd.Parameters.AddWithValue("@ApproverLevel", ApprovalLevel.Text);
                        cmd.Parameters.AddWithValue("@Option", "UpdateRequest");
                        int result = cmd.ExecuteNonQuery();
                        if (result != 0)
                        {
                            Label lbl = (row.FindControl("lblprov") as Label);
                            lbl.Visible = true;
                            lbl.Text = "Approved";
                            FillAllRequests();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' Request Approved..');window.location ='frmTicketApprover.aspx';", true);
                        }
                    }
                }
            }
	else if (e.CommandName == "Reject")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    string TicketRef = e.CommandArgument.ToString();
                    GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
                    Label Deskref = (row.FindControl("lblDesk") as Label);
                    Label OrgID = (row.FindControl("lblOrgFk") as Label);
Label ApprovalLevel= (row.FindControl("lbllevel") as Label);
                    LinkButton Prove_Button = (LinkButton)row.FindControl("btn_Prove");
                    string proc = "";
                    if (Convert.ToString(Deskref.Text).ToString() == "Service Request")
                    {
                        proc = "SD_SRTicketApprovalStatus_Manual";
                    }
                    else
                    {
                        proc = "SD_SRTicketApprovalStatus_CR";
                    }
                    using (SqlCommand cmd = new SqlCommand(proc, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ticketref", TicketRef);
                        cmd.Parameters.AddWithValue("@Remarks", "");
                        cmd.Parameters.AddWithValue("@OrgId", OrgID.Text);
                        cmd.Parameters.AddWithValue("@HODApproval", "");
                        cmd.Parameters.AddWithValue("@ApproverLevel", ApprovalLevel.Text);
                        cmd.Parameters.AddWithValue("@Option", "RejectRequest");
                        int result = cmd.ExecuteNonQuery();
                        if (result != 0)
                        {
                            Label lbl = (row.FindControl("lblprov") as Label);
                            lbl.Visible = true;
                            lbl.Text = "Approved";
                            FillAllRequests();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Asset Request Rejected..');window.location ='frmTicketApprover.aspx';", true);
                        }
                    }
                }
            }

            else if (e.CommandName == "Reject1")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    string TicketRef = e.CommandArgument.ToString();
                    GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
                    Label Deskref = (row.FindControl("lblDesk") as Label);
                    Label OrgID = (row.FindControl("lblOrgFk") as Label);
Label ApprovalLevel= (row.FindControl("lbllevel") as Label);
                    LinkButton Prove_Button = (LinkButton)row.FindControl("btn_Prove");
                    string proc = "";
                    if (Convert.ToString(Deskref.Text).ToString() == "Service Request")
                    {
                        proc = "SD_SRTicketApprovalStatus_SRFix";
                    }
                    else
                    {
                        proc = "SD_SRTicketApprovalStatus_CR";
                    }
                    using (SqlCommand cmd = new SqlCommand(proc, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ticketref", TicketRef);
                        cmd.Parameters.AddWithValue("@Remarks", "");
                        cmd.Parameters.AddWithValue("@OrgId", OrgID.Text);
                        cmd.Parameters.AddWithValue("@HODApproval", "");
                      cmd.Parameters.AddWithValue("@ApproverLevel", ApprovalLevel.Text);
                        cmd.Parameters.AddWithValue("@Options", "RejectRequest");
                        int result = cmd.ExecuteNonQuery();
                        if (result != 0)
                        {
                            Label lbl = (row.FindControl("lblprov") as Label);
                            lbl.Visible = true;
                            lbl.Text = "Approved";
                            FillAllRequests();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Asset Request Rejected..');window.location ='frmTicketApprover.aspx';", true);
                        }
                    }
                }

            }
            else if (e.CommandName == "Details")
            {
                string TicketRef = e.CommandArgument.ToString();
                GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
                Label Deskref = (Label)row.FindControl("lblDesk");
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
 Label Approv = (row.FindControl("lbllevel") as Label);

                Response.Write("<script type='text/javascript'>");
                Response.Write("window.open('/frmApprover.aspx?TicketId=" + TicketRef + "&redirected=true&ApproverLevel="+Approv.Text+"&OrgID=" + OrgID.Text + "','_blank');");
                Response.Write("</script>");
            }
            //FillAllRequests();
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }



    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void gvHRRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            System.Web.UI.WebControls.TableCell cell = e.Row.Cells[3];

            if (cell.Text == "Approved")
            {
                cell.BackColor = System.Drawing.Color.Green;
                cell.ForeColor = System.Drawing.Color.White;
                cell.Font.Bold = true;
            }
            if (cell.Text == "Pending")
            {
                cell.BackColor = System.Drawing.Color.Green;
                cell.ForeColor = System.Drawing.Color.White;
                cell.Font.Bold = true;
            }
            if (cell.Text == "Rejected")
            {
                cell.BackColor = System.Drawing.Color.Green;
                cell.ForeColor = System.Drawing.Color.White;
                cell.Font.Bold = true;
            }
            //FillAllRequests();
        }
    }

    private void Modal()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$(function () {");
        sb.Append(" $('#myModal').modal('show');});");
        sb.Append("</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", sb.ToString(), false);
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }

    protected void ddlSearchItems_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}