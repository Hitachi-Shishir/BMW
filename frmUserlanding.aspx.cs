using DocumentFormat.OpenXml.Office2013.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmUserlanding : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            LoadNews();
        }
	lblUser.Text=Convert.ToString(Session["Name"]);
    }
    protected void imgbtnIncident_Click(object sender, EventArgs e)
    {
        Response.Write("<script type='text/javascript'>");
        Response.Write("window.location.href = '/frmMyTickets.aspx?redirected=true&Desk=Incident';");
        Response.Write("</script>");
        //Response.Write("<script type='text/javascript'>");
        //Response.Write("window.location.href = '/frmAddIncidentUsers.aspx?redirected=true&Desk=Incident';");
        //Response.Write("</script>");
    }
    protected void imgbtnServiceRequest_Click(object sender, EventArgs e)
    {
        //string url = "/frmAddIncidentUsers.aspx?redirected=true&Desk=Service Request";
        //Server.Transfer(url);
        Response.Write("<script type='text/javascript'>");
        Response.Write("window.location.href = '/frmMyTickets.aspx?redirected=true&Desk=Service Request';");
        Response.Write("</script>");
    }


    protected void imgBtnChangeReq_Click(object sender, EventArgs e)
    {
        //string Agent = Request.QueryString["un"].ToString();
        //Response.Write("<script type='text/javascript'>");
        //Response.Write("window.open('/frmAddIncidentUsers.aspx?&redirected=true&Desk=Change Request','_blank');");
        //Response.Write("</script>");
        Response.Write("<script type='text/javascript'>");
        Response.Write("window.location.href = '/frmMyTickets.aspx?redirected=true&Desk=Change Request';");
        Response.Write("</script>");
    }

    protected void imgbtnIamProc_Click(object sender, EventArgs e)
    {
        string url = "/frmAddIncidentUsers.aspx?&redirected=true&Desk=CloudProcess";
        Response.Redirect(url);
    }
    protected void imgBtnMyTickets_Click(object sender, EventArgs e)
    {
        Response.Write("<script type='text/javascript'>");
        Response.Write("window.location.href = '/frmMyTickets.aspx?redirected=true&Desk=Incident';");
        Response.Write("</script>");
        //  Response.Redirect("frmMyTickets.aspx");
    }
    protected void imgBtnSelfServ_Click(object sender, EventArgs e)
    {

    }

    protected void lnkChangeManagement_Click(object sender, EventArgs e)
    {
        Response.Write("<script type='text/javascript'>");
        Response.Write("window.location.href = '/frmAddIncidentUsers.aspx?redirected=true&Desk=Change Management';");
        Response.Write("</script>");
    }

    protected void lnkUserDashboard_Click(object sender, EventArgs e)
    {
        string url = "/Dashboard/frmUserDashboard.aspx";
        Response.Redirect(url);
    }


    protected void lnkmydeallocationrequest_Click(object sender, EventArgs e)
    {
        string url = "/Asset/frmAssetReqExit.aspx";
        Response.Redirect(url);
    }

    protected void lnkmyassets_Click(object sender, EventArgs e)
    {
        string url = "/Asset/frmmyassets.aspx";
        Response.Redirect(url);
    }


    protected void lnkassetrequest_Click(object sender, EventArgs e)
    {
        string url = "/Asset/frmAssetReq.aspx";
        Response.Redirect(url);
    }

    protected void lnkKBArticle_Click(object sender, EventArgs e)
    {
        string url = "/frmAddKnowledgeBasenew.aspx";
        Response.Redirect(url);
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        divCard.Visible = false;
        divTable.Visible = true;
        string sql = "select * from SD_KnowledgeBase where OrgDeskRef='" + Convert.ToString(Session["SD_OrgID"]) + "' and Issue like '%" + txtSearchKB.Text + "%'";
        DataTable dt = database.GetDataTable(sql);
        if (dt.Rows.Count > 0)
        {
            gvResolution.DataSource = dt;
            gvResolution.DataBind();
            GridFormat(dt);
        }
        else
        {
            this.gvResolution.DataSource = (object)null;
            this.gvResolution.DataBind();
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
    protected void lnkBtn_Click(object sender, EventArgs e)
    {
        divCard.Visible = true;
        divTable.Visible = false;
        txtSearchKB.Text = "";
    }
    protected void LoadNews()
    {
        string orgId = Convert.ToString(Session["SD_OrgID"]);
        DataTable dtNews = new DataTable();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_InsertSD_NewsUpdate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", "Select");
                cmd.Parameters.AddWithValue("@OrgId", orgId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtNews);
            }
        }

        //if (dtNews.Rows.Count > 0)
        //{
        //    rptNews.DataSource = dtNews;
        //    rptNews.DataBind();
        //}
    }

    [WebMethod]
    public static List<object> GetNews()
    {
        string orgId = HttpContext.Current.Session["OrgID"].ToString();
        List<object> newsList = new List<object>();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_InsertSD_NewsUpdate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", " ");
                cmd.Parameters.AddWithValue("@OrgId", orgId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    newsList.Add(new
                    {
                        News = reader["News"].ToString(),
                        //  InsertedTime = reader["InsertedTime"].ToString()
                    });
                }
                con.Close();
            }
        }

        return newsList;
    }
}