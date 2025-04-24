using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmFeed : System.Web.UI.Page
{
    errorMessage msg = new errorMessage();
    protected void Page_Load(object sender, EventArgs e)
    {

        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (!Page.IsPostBack)
        {
            try
            {
                if (Request.QueryString["TicketID"] != null)
                {
                    txtEmpID.Text = Request.QueryString["TicketID"].ToString();
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = con;
                            con.Open();

                            cmd.CommandText = "pcv_CusFeed";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TicketID", txtEmpID.Text);
                            cmd.Parameters.AddWithValue("@Option", "FeedbackValidation");
                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                            {
                                cmd.CommandTimeout = 900;
                                DataTable dt = new DataTable();
                                adp.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {

                                    lblerror.ForeColor = Color.Red;
                                    lblerror.Text = "You have already completed  survey for ticketid:" + txtEmpID.Text + "";
                                    txtEmpID.Style.Add("border", "solid 1px red");
                                    Panel1.Visible = false;

                                }
                                else
                                {
                                    loadSurvey();
                                }
                                con.Close();
                            }
                        }
                    }
                }
                else
                {
                    Panel1.Enabled = false;
                }
            }

            catch (Exception ex)
            {
                msg.ReportError(ex.Message);
                Panel1.Enabled = false;
            }

        }

    }
    public void loadSurvey()
    {


        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "pcv_CusFeed";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Option", "LoadSurvey");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 900;
                        DataTable dt = new DataTable();
                        adp.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            RepeaterforShowSurveyQuestion.DataSource = dt;
                            RepeaterforShowSurveyQuestion.DataBind();
                        }
                        else
                        {

                        }
                        con.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }
    }

    protected void RepeaterforShowSurveyQuestion_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //-------------------------------------------------------------------------------------------------------------------------------
                RepeaterItem drv = e.Item;

                RadioButtonList RadioButtonList1 = (RadioButtonList)e.Item.FindControl("dtlAnswers");
                HiddenField hnQuestionsid = (HiddenField)e.Item.FindControl("hnQuestionsid");
                //Get questionID here        
                int QuestionID = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "QuestionId"));
                //pass Question ID to your DB and get all available options for the question       
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {

                    string sql = string.Empty;

                    //Bind the RadiobUttonList here  

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.CommandText = "pcv_CusFeed";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@QuestionId", QuestionID);
                        cmd.Parameters.AddWithValue("@Option", "FillOptions");
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandTimeout = 900;
                            DataSet ds = new DataSet();
                            adp.Fill(ds);
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                RadioButtonList1.DataSource = ds;
                                RadioButtonList1.DataTextField = "Question_option";
                                RadioButtonList1.DataValueField = "id";
                                RadioButtonList1.DataBind();
                            }

                        }
                    }

                }
            }

        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }

    }
    private void InsertComment()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "pcv_CusFeed";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeedbackID", Label2.Text);
                    cmd.Parameters.AddWithValue("@Feedback", txtComments.Text);
                    cmd.Parameters.AddWithValue("@Date_Time", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    cmd.Parameters.AddWithValue("@Option", "FeedbackComment");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.ExecuteNonQuery();
                        lblerror.ForeColor = Color.Green;
                        //Response.Redirect("frmthankyou.aspx");
                        con.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }

    }

    private void InsertData()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "pcv_CusFeed";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeedbackID", Label2.Text);
                    cmd.Parameters.AddWithValue("@TicketID", txtEmpID.Text);
                    cmd.Parameters.AddWithValue("@QuestionId", QUES);
                    cmd.Parameters.AddWithValue("@Answer", selected);
                    cmd.Parameters.AddWithValue("@Date_Time", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    cmd.Parameters.AddWithValue("@Option", "FeedbackInsert");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.ExecuteNonQuery();
                        //txtEmpID.Text = "";
                        //txtComments.Text = "";
                        con.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }

    }
    string QUES, selected;
    public void Autogenrate()
    {
        int r;
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select max(FeedbackID) from pcv_FeedAnswers", con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            string d = dr[0].ToString();
                            if (d == "")
                            {
                                Label2.Text = "1001";//set the value in textbox which name is id
                            }
                            else
                            {
                                r = Convert.ToInt32(dr[0].ToString());
                                r = r + 1;
                                Label2.Text = r.ToString();
                            }
                        }
                        con.Close();
                        con.Open();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "pcv_CusFeed";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TicketID", txtEmpID.Text);
                    cmd.Parameters.AddWithValue("@Option", "TicketIDValidation");

                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 900;
                        DataSet ds = new DataSet();
                        adp.Fill(ds);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // Call the InsertFeedbackRecords method to insert data (assuming it exists)
                            InsertFeedbackRecords();

                            // Show the Thank You card using JavaScript (hide header-wrapper and show thankyouCard)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowThankYouCard", "showThankYouCard();", true);
                        }
                        else
                        {
                            // Invalid ticket ID handling
                            lblerror.ForeColor = Color.Red;
                            lblerror.Text = "Invalid ticketid";
                            txtEmpID.Style.Add("border", "solid 1px red");
                        }

                        con.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }
    }

    private void InsertFeedbackRecords()
    {
        try
        {
            Autogenrate();
            foreach (RepeaterItem item in RepeaterforShowSurveyQuestion.Items)
            {
                // Checking the item is a data item
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var rdbList = item.FindControl("dtlAnswers") as RadioButtonList;
                    var lblQ = item.FindControl("LblQuestion") as Label;
                    HiddenField hnQuestionsid = (HiddenField)item.FindControl("hnQuestionsid");
                    QUES = Convert.ToInt32(hnQuestionsid.Value).ToString();

                    // Get the selected value
                    selected = rdbList.SelectedItem.Text;
                }
                InsertData();
            }
            InsertComment();
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }

    }
}