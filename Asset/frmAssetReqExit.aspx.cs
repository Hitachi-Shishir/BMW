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

public partial class frmAssetReqExit : System.Web.UI.Page
{

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OrgID"] != null)
        {
            if (!IsPostBack)
            {
                
                    FillAllRequests();
                
                
                ActiveLocation();
                //FillProducts();
                FillActiveDepartments();
                ddlSearchItems.Items.Insert(0, new ListItem("--------------------Select All--------------------", "0"));
                FillAssetType();
                txtsAMAccountName.Text=Session["UserName"].ToString();
                txtEmpID.Text=Session["UserName"].ToString();
                txtEmpID.CssClass="form-control form-control-sm";
                txtEmpName.Text=Session["Name"].ToString();
                txtEmpName.CssClass="form-control form-control-sm";
                txtemail.Text=Session["EmailID"].ToString();
                txtemail.CssClass="form-control form-control-sm";
              
               
            }
        }
        else
        {
              Response.Redirect("https://10.110.0.22/Default.aspx");
        }
    }

    private void AllRequestByLocation()
    {
        try
        {
          

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ALMsp_AssetRequest ", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Location", Session["LocationCode"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "AllRequestByLocation");
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
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
        catch (Exception ex)
        {
           
        }
    }
    
    

   
    private void FillAssetType()
    {
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT  * FROM [dbo].[ALM_Asset_Type]"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                ddlAssetType.DataSource = cmd.ExecuteReader();
                ddlAssetType.DataTextField = "AssetType";
                ddlAssetType.DataValueField = "AssetTypeCode";
                ddlAssetType.DataBind();
                con.Close();
            }
        }
        ddlAssetType.Items.Insert(0, new ListItem("-----Select Asset Type-----", "0"));
    }

    private void FillActiveDepartments()
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("ALM_Asset_Dep", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "ActiveDepartment");
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            ddldepartment.DataSource = dt;
                            ddldepartment.DataValueField = "DepCode";
                            ddldepartment.DataTextField = "DepName";
                            ddldepartment.DataBind();
                            ddldepartment.Items.Insert(0, new ListItem("-----Select Department-----", "0"));
                        }
                        else
                        {
                            //gvEngineerWiseCalls.EmptyDataText = "No Records Found";
                            ddldepartment.DataSource = null;
                            ddldepartment.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            //objBLL = null;
        }
    }

    private void FillAllRequests()
    {
     
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("ALMsp_ExitRequest", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                cmd.Parameters.AddWithValue("@EmpID", Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@Option", "AllRequest");
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
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "PARDEEP2019KUMAR";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    private void ActiveLocation()
    {
        try
        {
           
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(" select * from  ALM_Asset_Location_Master where IsActive='True' and OrgID=@OrgID", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            ddlLocation.DataSource = dt;
                            ddlLocation.DataValueField = "LocCode";
                            ddlLocation.DataTextField = "Location";
                            ddlLocation.DataBind();
                            ddlLocation.Items.Insert(0, new ListItem("-----Select Location-----", "0"));
                        }
                        else
                        {
                            //gvEngineerWiseCalls.EmptyDataText = "No Records Found";
                            ddlLocation.DataSource = null;
                            ddlLocation.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            //objBLL = null;
        }
    }

    

  
    private string GetIP()
    {
        string strHostName = System.Net.Dns.GetHostName();
        IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
        IPAddress[] addr = ipEntry.AddressList;
        return addr[addr.Length - 1].ToString();
    }
    public static string Encry;

    private void InitiateRequest()
    {
       
            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("ALMsp_ExitRequest", con))
                    {
                        cmd.Parameters.AddWithValue("@EmpName", txtEmpName.Text);
                        cmd.Parameters.AddWithValue("@EmpID", txtEmpID.Text);
                        cmd.Parameters.AddWithValue("@EmpGrade", txtGrade.Text);
                        cmd.Parameters.AddWithValue("@EmpEmailID", txtemail.Text);
                        cmd.Parameters.AddWithValue("@EmpContactNo", txtContactNo.Text);
                        cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                        cmd.Parameters.AddWithValue("@Site", "NA");
                        cmd.Parameters.AddWithValue("@AssetTypeCode", "NA");
                        cmd.Parameters.AddWithValue("@Category", "NA");
                        cmd.Parameters.AddWithValue("@SubProductID", "NA");
                        cmd.Parameters.AddWithValue("@Organisation", "NA");
                        cmd.Parameters.AddWithValue("@ManufacturerID", "NA");
                        cmd.Parameters.AddWithValue("@ModelCode", "NA");
                        cmd.Parameters.AddWithValue("@Location", ddlLocation.SelectedValue);
                        cmd.Parameters.AddWithValue("@Department", ddldepartment.SelectedValue);
                        cmd.Parameters.AddWithValue("@FApproverEmail", txtApprover.Text);
                        cmd.Parameters.AddWithValue("@SApproverEmail", "NA");
                        cmd.Parameters.AddWithValue("@Reason", txtReason.Text);
                        cmd.Parameters.AddWithValue("@RequestType", "NA");
                        cmd.Parameters.AddWithValue("@SerialNo", "NA");
                        cmd.Parameters.AddWithValue("@ResignedReceivedDate", txtResignReceived.Text);
                        cmd.Parameters.AddWithValue("@LastWorkingDate", txtLastWorking.Text);
                        cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                        cmd.Parameters.AddWithValue("@InsertBy", Convert.ToInt32(Session["ALMUserID"]));
                        cmd.Parameters.Add("@RequestNumber", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@Option", "Request");
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        int res;
                        res = cmd.ExecuteNonQuery();
                        Encry = cmd.Parameters["@RequestNumber"].Value.ToString();
                        if (res > 0)
                        {

                            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                            string id = Encrypt(Encry);
                            string Rid = (Encry);


                          
                            Session["Result"] = "Request ID: " + Encry + " added successfully";
                           ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('Exit Request Raised..');window.location.href='" + Request.RawUrl + "';", true);
                            // Response.Write("<script>alert('not cxcxcxcxcxcc data')</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('not save data')</script>");
                        }
                        
                        con.Close();
                    }
                }

                

            }
            catch (Exception ex)
            {
                
            }
       
    }

    protected void btnRequest_Click(object sender, EventArgs e)
    {      
        CheckDataRequest();
    }

    protected void lnkbtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmUserlanding.aspx");
    }
    private string PopulateBody(string EmpID, string EmpName, string Grade, string EmpEmail, 
        string EmpContactNo, string EmpLocation, string EmpDepartment,string AssetType,string Category,
        string RequestID,string id, string url,string RequestNo,string Reason, string RequestType)
    {
        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Server.MapPath("~/Asset/AssetTemplates/NewAssetRequest.htm")))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{EmpID}", EmpID);
        body = body.Replace("{EmpName}", EmpName);
        body = body.Replace("{Grade}", Grade);
        body = body.Replace("{EmpEmail}", EmpEmail);
        body = body.Replace("{EmpContactNo}", EmpContactNo);
        body = body.Replace("{EmpLocation}", EmpLocation);
        body = body.Replace("{EmpDepartment}", EmpDepartment);
        body = body.Replace("{AssetType}", AssetType);
        body = body.Replace("{Category}", Category);
        body = body.Replace("{RequestID}", RequestID);
        body = body.Replace("{RID}", id);
        body = body.Replace("{url}", url);
        body = body.Replace("{RequestNo}", RequestNo);
        body = body.Replace("{RequestType}", RequestType);
        body = body.Replace("{Reason}", Reason);

        return body;
    }
    private void SendMail( string body)
    {
       
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("ALM_Sendmail", con))
            {
                cmd.Parameters.AddWithValue("@ApproverEmailID", txtApprover.Text);
                cmd.Parameters.AddWithValue("@MailSubject", "New Asset Request-"+ddlLocation.SelectedItem.ToString()+"-"+txtEmpName.Text.Trim()+"-"+ddlRequestType.SelectedItem.ToString()+"");
                cmd.Parameters.AddWithValue("@MailBody", body);
                cmd.Parameters.AddWithValue("@Option", "AllInOne");
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

   

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        
      try
        {

            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from  ALM_Asset_Location_Master  where LocCode=@LocCode and OrgID=@OrgID", con))
                {
                    cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@LocCode", ddlLocation.SelectedValue);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtApprover.Text = dt.Rows[0]["ITHeadEmail"].ToString();
                        }
                        else
                        {
                            txtApprover.Text = "";
                            //ddlCategory.DataBind();
                        };
                    }
                }
            }
            
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            //objBLL = null;
        }
    }

    

    protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{

        //    DataTable table = new BLL().DepartmentsApprovers(ddldepartment.SelectedValue);
        //    if (table.Rows.Count > 0)
        //    {
        //        lblApprover.Text = table.Rows[0]["Approver1"].ToString();
        //    }
        //    else
        //    {
        //        txtGrade.Text = ""; ;
        //        //ddlCategory.DataBind();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    msg.ReportError(ex.Message);
        //}
        //finally
        //{
        //    //objBLL = null;
        //}
    }

    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
       // CheckStock();
       // FillAllModel(ddlCategory.SelectedValue);
       // FillModel();
    }

   

   

    protected void gvHRRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell cell = e.Row.Cells[10];
          
            if (cell.Text == "Approved")
            {
                cell.BackColor = Color.Green;
                cell.ForeColor = Color.White;
                cell.Font.Bold = true;
            }
            if (cell.Text == "Pending")
            {
                cell.BackColor = Color.Yellow;
                cell.ForeColor = Color.Black;
                cell.Font.Bold = true;
            }
            if (cell.Text == "Rejected")
            {
                cell.BackColor = Color.Red;
                cell.ForeColor = Color.White;
                cell.Font.Bold = true;
            }
            TableCell cell10 = e.Row.Cells[11];

            if (cell10.Text == "Pending")
            {
                cell10.BackColor = Color.Yellow;
                cell10.ForeColor = Color.Black;
                cell10.Font.Bold = true;
            }
            if (cell10.Text == "Acquired")
            {
                cell10.BackColor = Color.Green;
                cell10.ForeColor = Color.White;
                cell10.Font.Bold = true;
            }
        }
    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {

                    string sql = "ALM_FilterData";
                    // string sql = "SELECT * FROM [dbo].[ALM_Asset_vInventory]";
                    if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                    {
                        // sql += " WHERE "+ ddlSearchItems.SelectedValue + " LIKE @SearchItem + '%'";
                        // sql += " WHERE " + ddlSearchItems.SelectedValue + "= @SearchItem";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ColumnName", ddlSearchItems.SelectedValue);
                        cmd.Parameters.AddWithValue("@SearchItem", txtSearch.Text.Trim());
                        cmd.Parameters.AddWithValue("@Option", "FilterExitRequestColumns");
                    }
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvHRRequest.DataSource = dt;
                        gvHRRequest.DataBind();
                    }
                }
            }
        }

        catch (Exception ex)
        {

            lblerrorMsg.Text = ex.Message;
        }
    }

    protected void ddlSearchItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearchItems.SelectedIndex == 0)
        {
            FillAllRequests();
            txtSearch.Text = string.Empty;
        }
        else
        {
            txtSearch.Text = string.Empty;
        }
    }


    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("ALMsp_Asset_Category"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AssetTypeCode", ddlAssetType.SelectedValue);
                cmd.Parameters.AddWithValue("@Option", "FillActiveCategory");
                cmd.Connection = con;
                con.Open();
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "ProductName";
                ddlCategory.DataValueField = "ProductID";
                ddlCategory.DataBind();
                con.Close();
            }
        }
        ddlCategory.Items.Insert(0, new ListItem("----------Select Category----------", "0"));
    }

    private void CheckData()
    {    
       using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("select * from ALM_ExitRequest where EmpID=@EmpID", con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@EmpID", txtsAMAccountName.Text);
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        
                        if (dt.Rows.Count > 0)
                        {        
                                  
                                         
                                btnRequest.Enabled=false;   
                                lblApprover.Text="Exit Request has already been initiated for mentioned user..";                                                                   
                        }
                        else
                        {
                               
                               
                               SearchDetailsAD();
                               btnRequest.Enabled=true;   
                               lblApprover.Text=""; 
                        }
                    }
                }
            }
        }
    }

   private void CheckDataRequest()
    {    
       using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("select * from ALM_ExitRequest where EmpID=@EmpID", con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@EmpID", txtsAMAccountName.Text);
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        
                        if (dt.Rows.Count > 0)
                        {        
                                                                                                              
                                lblApprover.Text="Exit Request has already been initiated for mentioned user..";                                                                   
                        }
                        else
                        {
                               
                               
                               InitiateRequest();
                               btnRequest.Enabled=true;   
                               lblApprover.Text=""; 
                        }
                    }
                }
            }
        }
    }

   private void SearchDetailsAD()
   {
       try
        {
            string connection = ConfigurationManager.ConnectionStrings["ADConnection"].ToString();
            DirectorySearcher dssearch = new DirectorySearcher(connection);
            dssearch.Filter = "(sAMAccountName=" + txtsAMAccountName.Text + ")";
            SearchResult sresult = dssearch.FindOne();
            if (sresult != null)
            {
                DirectoryEntry dsresult = sresult.GetDirectoryEntry();



                if (sresult.Properties["displayName"] != null && sresult.Properties["displayName"].Count > 0)
                {
                    txtEmpName.Text = dsresult.Properties["displayName"][0].ToString();
                }
                if (sresult.Properties["sAMAccountName"] != null && sresult.Properties["sAMAccountName"].Count > 0)
                {
                    txtEmpID.Text = dsresult.Properties["sAMAccountName"][0].ToString();
                }
                if (sresult.Properties["mail"] != null && sresult.Properties["mail"].Count > 0)
                {
                    txtemail.Text = dsresult.Properties["mail"][0].ToString();
                }
                if (sresult.Properties["telephoneNumber"] != null && sresult.Properties["telephoneNumber"].Count > 0)
                {
                     txtContactNo.Text = dsresult.Properties["telephoneNumber"][0].ToString();
                   // txtContactNo.Text = RemoveSpecialChar.RemoveSpecialChars(contact).ToString();
                }
                if (sresult.Properties["title"] != null && sresult.Properties["title"].Count > 0)
                {
                    txtGrade.Text = dsresult.Properties["title"][0].ToString();
                }
              
               }
        }
        catch (Exception ex)
        {
            Response.Write("Oops! error occured :" + ex.Message.ToString());
        }
   }

    protected void imgbtnSearch_Click(object sender, EventArgs e)
    {
        CheckData();
    }

    protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
}