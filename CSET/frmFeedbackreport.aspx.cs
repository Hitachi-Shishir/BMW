using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CSET_frmFeedbackreport : System.Web.UI.Page
{
    errorMessage msg = new errorMessage();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        try
        {
            if (!Page.IsPostBack)
            {

                ComplianceDetails(Convert.ToString(Session["SD_OrgID"]));
            }
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);

        }
    }
    public override void VerifyRenderingInServerForm(Control control) { }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {

            ExportToExcel();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ExportToExcel()
    {
        if (gvPatchStatus == null || gvPatchStatus.Rows.Count == 0)
        {
            // Using ClientScript to show JavaScript alert
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('No data available to export.');", true);
            return;
        }

        string attachment = "attachment; filename=Feedback Report.xls";
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "";

        using (StringWriter oStringWriter = new StringWriter())
        {
            using (HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter))
            {
                gvPatchStatus.RenderControl(oHtmlTextWriter);
                Response.Write(oStringWriter.ToString());
            }
        }
        Response.End();
    }
    protected void gvPatchStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatchStatus.PageIndex = e.NewPageIndex;
        this.ComplianceDetails(Convert.ToString(Session["SD_OrgID"]));
    }
//    private void ComplianceDetails()
//    {
//        try
//        {

//            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
//            {

//                con.Open();
//                using (SqlCommand cmd = new SqlCommand(@"SELECT *
//				FROM
//				(SELECT DISTINCT 
//					a.TicketID,q.Question, a.Answer,f.Feedback				
//				FROM SD_FeedAnswers a INNER JOIN
//					SD_FeedQuestions q ON a.QuestionId = q.QuestionId 
					
//					INNER JOIN SD_Feedback f ON f.FeedbackID=a.FeedbackID
//				WHERE  (q.status = 'Active') ) AS SourceTable
//				PIVOT
//				(
//				 Max(Answer)
//				 FOR Question IN (
//					 [Are you satisfied with the service ?]
//					,[How do you rate timeliness of response ? ]
//					,[How do you rate frequency of communication and update ?]
//					,[How do you rate the knowledge of the Technician ?]
//					,[How do you rate the efficiency of our Service Delivery ?]
					
//				 )
//				) AS PivotTable 
//", con))
//                {
//                    cmd.CommandType = CommandType.Text;
//                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
//                    {
//                        using (DataTable dt = new DataTable())
//                        {
//                            adp.SelectCommand.CommandTimeout = 180;

//                            adp.Fill(dt);
//                            if (dt.Rows.Count > 0)
//                            {

//                                lblTotalRecord.Text = dt.Rows.Count.ToString();
//                                gvPatchStatus.DataSource = dt;
//                                gvPatchStatus.DataBind();
//                                GridFormat(dt);

//                            }
//                            else
//                            {
//                                gvPatchStatus.DataSource = null;
//                                gvPatchStatus.DataBind();

//                            }
//                        }
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            //msg.ReportError(ex.Message);
//        }
//    }
    private void ComplianceDetails(string orgid)
    {
        try
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"Select f.submitterName,f.Question,f.Answer,f.Date_Time,o.OrgName from vSD_FeedbackReport f inner join SD_OrgMaster o on f.organizationFK=o.Org_ID where f.organizationFK='" + orgid + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            adp.SelectCommand.CommandTimeout = 180;

                            adp.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {

                                lblTotalRecord.Text = dt.Rows.Count.ToString();
                                gvPatchStatus.DataSource = dt;
                                gvPatchStatus.DataBind();
                                GridFormat(dt);

                            }
                            else
                            {
                                gvPatchStatus.DataSource = null;
                                gvPatchStatus.DataBind();

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //msg.ReportError(ex.Message);
        }
    }
    protected void GridFormat(DataTable dt)
    {
        gvPatchStatus.UseAccessibleHeader = true;
        gvPatchStatus.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvPatchStatus.TopPagerRow != null)
        {
            gvPatchStatus.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvPatchStatus.BottomPagerRow != null)
        {
            gvPatchStatus.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvPatchStatus.FooterRow.TableSection = TableRowSection.TableFooter;
    }
}