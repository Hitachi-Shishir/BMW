using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Drawing;
using System.DirectoryServices;

public partial class frmmyassets : System.Web.UI.Page
{

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OrgID"] != null)
        {
            if (!IsPostBack)
            {
                
                    FillAllRequests();
                
                
                
              
               
            }
        }
        else
        {
              Response.Redirect("https://10.110.0.22/Default.aspx");
        }
    }

    protected void lnkbtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmUserlanding.aspx");
    }

    private void FillAllRequests()
    {
     
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("select * from alm_asset_vinventory where Assigned_to_person_Email=@EmpID", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EmpID", Session["UserEmail"].ToString());
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            gvHRRequest.DataSource = dt;
                            gvHRRequest.DataBind();
                        }
                        else
                        {
                            gvHRRequest.DataSource = null;
                            gvHRRequest.DataBind();
                        }
                    }
                }
            }
        }
    }
 



   
}