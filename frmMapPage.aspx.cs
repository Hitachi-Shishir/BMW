using OtpNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmMapPage : System.Web.UI.Page
{
    string userid = "";
    string pwd = "";
    protected void Page_Load(object sender, EventArgs e)
    {
         if (!string.IsNullOrEmpty(Request.QueryString["usid"]) && !string.IsNullOrEmpty(Request.QueryString["pwd"]))
         {
             userid = Request.QueryString["usid"].ToString().Replace("%20", " ");
             pwd = Request.QueryString["pwd"].ToString().Replace("%20", " ");
         }
         else
         {
             Response.Redirect("/Default.aspx");
         }
           CheckNonDomain();
    }

    private void CheckNonDomain()
{
    try
    {
        string constr1 = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con1 = new SqlConnection(constr1))
        {
            con1.Open();
            using (SqlCommand cmd = new SqlCommand(@"SELECT * FROM SD_vUser 
     WHERE LoginName = @portal_username COLLATE SQL_Latin1_General_CP1_CS_AS 
       AND pass = @portal_password COLLATE SQL_Latin1_General_CP1_CS_AS;", con1))
            {
                cmd.Parameters.AddWithValue("@portal_username", userid);
                cmd.Parameters.AddWithValue("@portal_password", pwd);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            
                            Session["MFAStatus"] = dt.Rows[0]["MFAStatus"].ToString();
                            Session["ISMfa"] = dt.Rows[0]["ISMfa"].ToString();
                            //if (Session["MFAStatus"].ToString() == "False" || Session["MFAStatus"].ToString() == "0" || string.IsNullOrEmpty(Convert.ToString(Session["MFAStatus"])))
                            //{
                                InitilizeSession(dt.Rows[0]["LoginName"].ToString());
                                if (Convert.ToString(Session["UserRole"]).ToLower() != "sduser")
                                {
                                    Response.Redirect("/frmDashboard.aspx");
                                }
                                else
                                {
                                    Response.Redirect("/frmUserlanding.aspx");
                                }

                            //}
                           // else
                            //{
                               // string chkremb2fa = Convert.ToString(dt.Rows[0]["RememberISMfa"]);
                              //  string Serialno = Convert.ToString(dt.Rows[0]["Serialno"]);
                               // string Currentsystemserialno = GetBIOSserNo();

                              //  DateTime chkremb2faDate = Convert.ToDateTime(dt.Rows[0]["RememberISMfaTime"]);
                              //  string chkdate = chkremb2faDate.ToString("yyyy-MM-dd");
                             //   string sql = "SELECT DATEDIFF(DAY, '" + chkdate + "', cast(getdate() as date))";
                              //  Int64 daydiff = Convert.ToInt64(database.GetScalarValue(sql));

                              //  if (Convert.ToString(Session["UserRole"]).ToLower() != "sduser")
                              //  {
                               //     Response.Redirect("/frmDashboard.aspx");
                               // }
                                //else
                               // {
                                    //Response.Redirect("/frmUserlanding.aspx");
                               // }
                            //}
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Invalid Username or Password')</script>");
                        }
                    }
                }
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
        Response.Write(ex.Message.ToString());
    }
    string username = System.Environment.UserName;
}
private void InitilizeSession(string loginName)
{
    string sql = "SELECT * FROM SD_vUser WITH(NOLOCK) WHERE LoginName = '" + loginName + "'";
    DataTable dt = database.GetDataTable(sql);
    if (dt.Rows.Count > 0)
    {
        Session["UserName"] = userid;
        Session["LoginName"] = dt.Rows[0]["LoginName"].ToString();
        Session["SDRole"] = dt.Rows[0]["SDRole"].ToString();
        Session["Location"] = dt.Rows[0]["LocCode"].ToString();
        Session["UserID"] = dt.Rows[0]["UserID"].ToString();
        Session["UserScope"] = dt.Rows[0]["UserScope"].ToString();
        Session["UserType"] = dt.Rows[0]["UserType"].ToString();
        Session["UserRole"] = dt.Rows[0]["UserRole"].ToString();
        Session["EmpID"] = dt.Rows[0]["EmpID"].ToString();
        Session["SD_OrgID"] = dt.Rows[0]["Org_ID"].ToString();
        Session["OrgID"] = dt.Rows[0]["Org_ID"].ToString();
        Session["ISMfa"] = dt.Rows[0]["ISMfa"].ToString();
        Session["MFAStatus"] = dt.Rows[0]["MFAStatus"].ToString();
        Session["ThemeModify"] = dt.Rows[0]["ThemeModify"].ToString();
        Session["theme"] = dt.Rows[0]["theme"].ToString();
        Session["OrgName"] = dt.Rows[0]["OrgName"].ToString();
        Session["EmailID"] = dt.Rows[0]["EmailID"].ToString();
        Session["UserEmail"] = dt.Rows[0]["EmailID"].ToString();
    }

}
private string Decrypt(string cipherText)
{
    string EncryptionKey = "#PCVISOR@DEVELOPER#";
    byte[] cipherBytes = Convert.FromBase64String(cipherText);
    using (Aes encryptor = Aes.Create())
    {
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.Close();
            }
            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        }
    }
    return cipherText;
}
public string GetBIOSserNo()
{
    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
    foreach (ManagementObject wmi in searcher.Get())
    {
        try
        {
            return wmi.GetPropertyValue("SerialNumber").ToString();
        }
        catch { }
    }
    return "BIOS Serial Number: Unknown";
}
}