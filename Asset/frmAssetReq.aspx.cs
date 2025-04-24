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
using DocumentFormat.OpenXml.Office.Word;

public partial class frmAssetReq : System.Web.UI.Page
{

  
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OrgID"] != null)
        {
            if (!IsPostBack)
            {
                
                FillAllRequests();
                
                ActiveLocation();
                FillActiveDepartments();
                ddlSearchItems.Items.Insert(0, new ListItem("--------------------Select All--------------------", "0"));
                FillAssetType();
               
                rfvservicerqstno.Enabled=false;
                txtservicerqstno.Enabled=false;
                txtservicerqstno.CssClass="form-control form-control-sm";
                txtApprover.Enabled=false;
                txtApprover.CssClass="form-control form-control-sm";
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

     private void FillProject()
    {
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT distinct ProjectName FROM [dbo].[ALM_Project] where IsActive='1' and OrgID=@OrgID and SubDepartment=@SubDepartment"))
            {
                cmd.Parameters.AddWithValue("@SubDepartment", ddldepartment.SelectedValue);
                cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                ddlProject.DataSource = cmd.ExecuteReader();
                ddlProject.DataTextField = "ProjectName";
                ddlProject.DataValueField = "ProjectName";
                ddlProject.DataBind();
                con.Close();
            }
        }
        ddlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-----Select Project----", ""));
    }

    private void FillProducts()
    {
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(@"SELECT  ProductID, ProductName FROM [dbo].[ALM_Asset_Category] where OrgID=@OrgID and IsActive='1' and AssetTypeCode=@AssetTypeCode"))
            {
                cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                cmd.Parameters.AddWithValue("@AssetTypeCode", ddlAssetType.SelectedValue);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "ProductName";
                ddlCategory.DataValueField = "ProductID";
                ddlCategory.DataBind();
                con.Close();
            }
        }
        ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-----Select Category-----", "0"));
    }

    protected void lnkbtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmUserlanding.aspx");
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
            using (SqlCommand cmd = new SqlCommand("SELECT  * FROM [dbo].[ALM_Asset_Type] where IsActive=1 and OrgID=@OrgID and (AssetType='IT Assets' or AssetType='IT Consumables')"))
            {
                cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
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
                            ddldepartment.DataValueField = "DepName";
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
            using (SqlCommand cmd = new SqlCommand("ALMsp_AssetRequest", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                cmd.Parameters.AddWithValue("@EmpId", Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@Option", "AllRequestEndUser");
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

    private void RequestAttachment(string RequestID)
    {
        try
        {
            if (FileUploadPO.HasFile == true)
            {
                string fileName = string.Empty;
                string filePath = string.Empty;

                filePath = Server.MapPath("Asset Request Attachment/" + FileUploadPO.FileName);
                FileUploadPO.SaveAs(filePath);

                string cmdText = "update ALM_AssetRequest set Attachment=@Attachment where RequestNumber=@RequestNumber";

                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Attachment", "~/Asset Request Attachment/" + FileUploadPO.FileName);
                        cmd.Parameters.AddWithValue("@RequestNumber", RequestID);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        cmd.ExecuteNonQuery();
                        sqlConnection.Close();


                    }

                }
            }
            else 
            {
            
            }
            
                   
            
           
        }

        catch (Exception ex)
        {
           
        }
    }

    public static string Encry;
    protected void btnRequest_Click(object sender, EventArgs e)
    {      
       
        
            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("ALMSP_AssetRequest", con))
                    {
                        cmd.Parameters.AddWithValue("@EmpName", txtEmpName.Text);
                        cmd.Parameters.AddWithValue("@ADUserName", txtsAMAccountName.Text);
                        cmd.Parameters.AddWithValue("@EmpID", txtEmpID.Text);
                        cmd.Parameters.AddWithValue("@EmpGrade", txtGrade.Text);
                        cmd.Parameters.AddWithValue("@EmpEmailID", txtemail.Text);
                        cmd.Parameters.AddWithValue("@EmpContactNo", txtContactNo.Text);
                        cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                        cmd.Parameters.AddWithValue("@Site", "NA");
                        cmd.Parameters.AddWithValue("@AssetTypeCode", ddlAssetType.SelectedValue);
                        cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@SubProductID", "0");
                        cmd.Parameters.AddWithValue("@ManufacturerID", "NA");
                        cmd.Parameters.AddWithValue("@ModelCode", "NA");
                        cmd.Parameters.AddWithValue("@Location", ddlLocation.SelectedValue);
                        cmd.Parameters.AddWithValue("@Project", ddlProject.SelectedValue);
                        cmd.Parameters.AddWithValue("@Department", ddldepartment.SelectedValue);
                        cmd.Parameters.AddWithValue("@FApproverEmail", txtApprover.Text);
                        cmd.Parameters.AddWithValue("@SApproverEmail", txtApprover2.Text);
                        cmd.Parameters.AddWithValue("@Reason", txtReason.Text);
                        cmd.Parameters.AddWithValue("@ServiceRequestNumber", txtservicerqstno.Text);
                        cmd.Parameters.AddWithValue("@RequestType", ddlRequestType.SelectedValue);
                        cmd.Parameters.AddWithValue("@SerialNo", txtSerialNo.Text);
                        cmd.Parameters.AddWithValue("@OrgID", Session["Orgid"].ToString());
                        cmd.Parameters.AddWithValue("@InsertBy", Convert.ToInt32(Session["ALMUserID"]));
                        cmd.Parameters.Add("@RequestNumber", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@Option", "Request");
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        Encry = cmd.Parameters["@RequestNumber"].Value.ToString();
                        RequestAttachment(Encry);
                       // PopulateBody(Encry);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('Asset Request Generated..');window.location.href='" + Request.RawUrl + "';", true);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
    }
    private void PopulateBody(string RequestNo)
    {
        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Server.MapPath("~/Asset/AssetTemplates/NewAssetRequest.htm")))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{EmpID}", txtEmpID.Text);
        body = body.Replace("{EmpName}", txtEmpName.Text);
        body = body.Replace("{Grade}", txtGrade.Text);
        body = body.Replace("{EmpEmail}", txtemail.Text);
        body = body.Replace("{EmpContactNo}", txtContactNo.Text);
        body = body.Replace("{EmpLocation}", ddlLocation.SelectedValue);
        body = body.Replace("{EmpDepartment}", ddldepartment.SelectedValue);
        body = body.Replace("{AssetType}", ddlAssetType.SelectedValue);
        body = body.Replace("{Category}", ddlCategory.SelectedValue);
        body = body.Replace("{RID}", RequestNo);               
        body = body.Replace("{RequestType}", ddlRequestType.SelectedValue);
        body = body.Replace("{Reason}", txtReason.Text);

        SendMail(body);
    }

   private void SendMail(string body)
    {
      try
       {
        using (MailMessage message = new MailMessage())
        {
            message.From = new MailAddress("helpdesk.bmwtechworks@hisysmc.com", "helpdesk.bmwtechworks@hisysmc.com");
            message.Subject = "Asset Allocation Approval Required";
            message.Body = body;
            message.IsBodyHtml = true;
            string str = txtApprover.Text;
            char[] chArray = new char[1] { ',' };
            foreach (string address in str.Split(chArray))
            message.To.Add(new MailAddress(address));          
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.office365.com";
            smtpClient.EnableSsl = Convert.ToBoolean("True");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            NetworkCredential networkCredential = new NetworkCredential();
            networkCredential.UserName = "helpdesk.bmwtechworks@hisysmc.com";
            networkCredential.Password = "F$715300231817aw";
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = (ICredentialsByHost)networkCredential;
            smtpClient.Port = int.Parse("587");
            smtpClient.Send(message);
            CreateLogs(body);
        }
     }

     catch (Exception ex)
       {
           using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(@"insert into ALM_EmailLogs([from],[to],[cc],[subject],bodycontent,[sent],sendstatus,FailureReason) values 
(@from,@to,@cc,@subject,@bodycontent,GETDATE(),@sendstatus,@FailureReason)", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@from", "helpdesk.bmwtechworks@hisysmc.com");
                cmd.Parameters.AddWithValue("@to", txtApprover.Text.Trim());
                cmd.Parameters.AddWithValue("@cc", "");
                cmd.Parameters.AddWithValue("@subject", "Asset Allocation Approval Required");
                cmd.Parameters.AddWithValue("@bodycontent", body);
                cmd.Parameters.AddWithValue("@sendstatus", "Failed");
                cmd.Parameters.AddWithValue("@FailureReason", ex.ToString());
                cmd.ExecuteNonQuery();
           }

       }
       }

    }

    private void CreateLogs(string body)
    {
       using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(@"insert into ALM_EmailLogs([from],[to],[cc],[subject],bodycontent,[sent],sendstatus) values 
(@from,@to,@cc,@subject,@bodycontent,GETDATE(),@sendstatus)", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@from", "helpdesk.bmwtechworks@hisysmc.com");
                cmd.Parameters.AddWithValue("@to", txtApprover.Text.Trim());
                cmd.Parameters.AddWithValue("@cc", "");
                cmd.Parameters.AddWithValue("@subject", "Asset Allocation Approval Required");
                cmd.Parameters.AddWithValue("@bodycontent", body);
                cmd.Parameters.AddWithValue("@sendstatus", "Success");
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

    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
       if(ddlRequestType.SelectedValue=="Repair"||ddlRequestType.SelectedValue=="Replacement"||ddlRequestType.SelectedValue=="Replacement of EOL")
       {
          txtservicerqstno.Enabled=true;
          rfvservicerqstno.Enabled=true;
          txtservicerqstno.CssClass="form-control form-control-sm";
       }

       else
       {
          txtservicerqstno.Enabled=false;
          rfvservicerqstno.Enabled=false;
          txtservicerqstno.CssClass="form-control form-control-sm";
       }
    }   

    protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
       FillProject();
    }

    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }

   
    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
       FillProducts();
    }
   

    protected void gvHRRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell cell = e.Row.Cells[9];
          
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
            TableCell cell10 = e.Row.Cells[10];

            if (cell10.Text == "Pending")
            {
                cell10.BackColor = Color.Yellow;
                cell10.ForeColor = Color.Black;
                cell10.Font.Bold = true;
            }
            if (cell10.Text == "Assigned")
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
                        cmd.Parameters.AddWithValue("@OrgId", Session["OrgID"].ToString());
                        cmd.Parameters.AddWithValue("@ColumnName", ddlSearchItems.SelectedValue);
                        cmd.Parameters.AddWithValue("@SearchItem", txtSearch.Text.Trim());
                        cmd.Parameters.AddWithValue("@Option", "FilterRequestColumns");
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
  

    protected void imgbtnSearch_Click(object sender, EventArgs e)
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
                    
                }
                if (sresult.Properties["title"] != null && sresult.Properties["title"].Count > 0)
                {
                    txtGrade.Text = dsresult.Properties["title"][0].ToString();
                }
                if (ddldepartment.Items.FindByValue(dsresult.Properties["department"][0].ToString().Trim()) != null && sresult.Properties["department"].Count > 0)
                {
                    ddldepartment.SelectedValue = dsresult.Properties["department"][0].ToString().Trim();
                }               
            }

            else
            {
                lblerrorMsg.Text = "Error: Not found";
            }
        }

        catch (Exception f)
        {
            lblerrorMsg.Text = "Error:" + f.Message;

        }
    }

    protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
}