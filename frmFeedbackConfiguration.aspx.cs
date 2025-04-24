using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmFeedbackConfiguration : System.Web.UI.Page
{
    SqlConnection con;
    errorMessage msg = new errorMessage();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            btnQUpdate.Visible = false;
            btnOptUpdate.Visible = false;
            btnOptDelete.Visible = false;
            if (!Page.IsPostBack)
            {
                ddlQuestions.Attributes.Add("class", "chzn-select form-control1");

                if (Session["UserName"] != null)
                {
                    FillQuestions();
                    FillActiveQuestions();

                }
                else
                {
                    Response.Redirect("/Default.aspx");
                }

            }
        }
        catch (Exception ex)
        {

            msg.ReportError(ex.Message);

        }
    }

    private void FillActiveQuestions()
    {


        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("pcv_CusFeed", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 180;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Option", "SelectQues");
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                ddlQuestions.DataSource = ds;
                ddlQuestions.DataTextField = "Question";
                ddlQuestions.DataValueField = "QuestionId";
                ddlQuestions.DataBind();
                ddlQuestions.Items.Insert(0, "----------Select Question----------");
            }
            else
            {
                ddlQuestions.DataSource = null;
                ddlQuestions.DataBind();

            }

        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }
        finally { con.Close(); con.Dispose(); };

    }

    private void FillQuestions()
    {
        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();

            cmd.CommandText = "pcv_CusFeed";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Option", "SelectAllQues");
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 900;
            DataSet ds = new DataSet();
            adp.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridQuestions.DataSource = ds;
                GridQuestions.DataBind();

            }
            else
            {
                GridQuestions.EmptyDataText = "No Records Found";

            }
            con.Close();
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }
        finally { con.Close(); con.Dispose(); };
    }
    protected void ddlQuestions_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillOptions();
    }
    private void FillOptions()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("pcv_CusFeed", con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.SelectCommand.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@QuestionId", ddlQuestions.SelectedValue);
                        cmd.Parameters.AddWithValue("@Option", "FillQuesOptions");
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            if (dt.Rows.Count > 0 && dt != null)
                            {
                                gvOption.DataSource = dt;
                                gvOption.DataBind();

                            }
                            else
                            {
                                gvOption.DataSource = null;
                                gvOption.DataBind();
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
    public static int index;
    public static string itemID;
    public static int id;

    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            HiddenField hdnid = (HiddenField)row.FindControl("hdnid");
            string id = hdnid.Value;
            if (row != null)
            {
                //gets the row index selected
                index = row.RowIndex;
                //gets the datakey
                itemID = GridQuestions.DataKeys[index].Value.ToString();
                //ddlOrg.Text  = row.Cells[1].Text;
                txtQuestion.Text = row.Cells[1].Text;
                ddlStatus.Text = row.Cells[2].Text;
                btnQUpdate.Visible = true;
                btnQAdd.Visible = false;
            }
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }

    }


    protected void lblSelect_Click(object sender, EventArgs e)
    {
        //Determine the RowIndex of the Row whose Button was clicked.
        int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
        //Get the value of column from the DataKeys using the RowIndex.
        id = Convert.ToInt32(gvOption.DataKeys[rowIndex].Values[0]);
        GridViewRow row = gvOption.Rows[rowIndex];
        txtOption.Text = (row.FindControl("lblquestionoption") as Label).Text;
        btnOptAdd.Visible = false;
        btnOptUpdate.Visible = true;
        btnOptDelete.Visible = true;
    }


    protected void btnQAdd_Click(object sender, EventArgs e)
    {
        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();

            cmd.CommandText = "pcv_CusFeed";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Question", txtQuestion.Text);
            cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@Option", "QuesInsert");
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 900;
            cmd.ExecuteNonQuery();
            FillQuestions();
            string message = string.Format("Question added sucessfully");

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert(\"" + message + "\");", true);
            Response.Redirect(Request.Url.AbsoluteUri);
            con.Close();
        }
        catch (Exception ex)
        {

            msg.ReportError(ex.Message);


        }
        finally { con.Close(); con.Dispose(); };
    }

    protected void btnQUpdate_Click(object sender, EventArgs e)
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
                    cmd.Parameters.AddWithValue("@QuestionId", itemID);
                    cmd.Parameters.AddWithValue("@Question", txtQuestion.Text);
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Option", "QuesUpdate");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.ExecuteNonQuery();
                        FillQuestions();
                        string message = string.Format("Question updated sucessfully");
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert(\"" + message + "\");", true);
                        Response.Redirect(Request.Url.AbsoluteUri);
                        con.Close();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }
        finally { con.Close(); con.Dispose(); };
    }

    protected void btnOptAdd_Click(object sender, EventArgs e)
    {
        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("pcv_CusFeed", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestionId", ddlQuestions.SelectedValue);
            cmd.Parameters.AddWithValue("@Question_option", txtOption.Text);
            cmd.Parameters.AddWithValue("@Option", "OptionAdd");
            con.Open();
            int k = cmd.ExecuteNonQuery();
            if (k != 0)
            {
                lblmessge.ForeColor = System.Drawing.Color.Green;
                lblmessge.Text = txtOption.Text + "   Added successfully";
                btnOptAdd.Visible = true;
                btnOptUpdate.Visible = false;
                btnOptDelete.Visible = false;
            }
            con.Close();
            txtOption.Text = "";
            FillOptions();

        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }
        finally { con.Close(); con.Dispose(); };
    }

    protected void btnOptUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("pcv_CusFeed", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@QuestionId", ddlQuestions.SelectedValue);
            cmd.Parameters.AddWithValue("@Question_option", txtOption.Text);
            cmd.Parameters.AddWithValue("@Option", "OptionUpdate");
            con.Open();
            int k = cmd.ExecuteNonQuery();
            if (k != 0)
            {
                lblmessge.ForeColor = System.Drawing.Color.Green;
                lblmessge.Text = txtOption.Text + "   Update successfully";

            }
            con.Close();
            txtOption.Text = "";
            btnOptAdd.Visible = true;
            btnOptUpdate.Visible = false;
            btnOptDelete.Visible = false;
            FillOptions();

        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }
        finally { con.Close(); con.Dispose(); };
    }

    protected void btnOptDelete_Click(object sender, EventArgs e)
    {
        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("pcv_CusFeed", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@QuestionId", ddlQuestions.SelectedValue);
            cmd.Parameters.AddWithValue("@Question_option", txtOption.Text);
            cmd.Parameters.AddWithValue("@Option", "OptionDelete");
            con.Open();
            int k = cmd.ExecuteNonQuery();
            if (k != 0)
            {
                lblmessge.ForeColor = System.Drawing.Color.Green;
                lblmessge.Text = txtOption.Text + "   Deleted successfully";
            }
            con.Close();
            txtOption.Text = "";
            btnOptAdd.Visible = true;
            btnOptUpdate.Visible = false;
            btnOptDelete.Visible = false;
            FillOptions();

        }
        catch (Exception ex)
        {
            msg.ReportError(ex.Message);
        }
        finally { con.Close(); con.Dispose(); };
    }

    protected void btnQCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
}