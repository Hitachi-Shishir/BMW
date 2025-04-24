using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmChgPass : System.Web.UI.Page
{
    errorMessage msg = new errorMessage();
    string holdOldPwd = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Session["UserType"] != null && Session["UserName"] != null)
                {
                }
                else
                {
                    Response.Redirect("/Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        }
    }
    protected void btnChgpass_Click(object sender, EventArgs e)
    {
        if (txtpassword.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Old Password can not be Empty !")}');", true);
            return;
        }
        if (txtNewPwd.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("New Password can not be Empty !")}');", true);
            return;
        }
        if (txtCnfPwd.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Cnf Password can not be Empty !")}');", true);
            return;
        }
        string chk = OldPwdCheck();
        if (chk == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Old Password did not Matched !")}');", true);
            return;
        }
        if (txtNewPwd.Text != txtCnfPwd.Text)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("New Password and Cnf Password Did't Matched !")}');", true);
            return;
        }
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddRequester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Pass", txtCnfPwd.Text);
                    cmd.Parameters.AddWithValue("@LoginName", Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdatePassFromPortal");
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    con.Open();
                    int k = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (ErrorChk == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmChgPass.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Password changed successfully")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}');", true);
                    }
                    con.Close();
                    txtpassword.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "#PCVISOR@DEVELOPER#";
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

    protected string OldPwdCheck()
    {
        string res = "";
        if (txtpassword.Text != "")
        {
            string sql = "if exists(select Pass from SD_User_Master where UserID='" + Convert.ToString(Session["UserID"]) + "' and Pass='" + txtpassword.Text + "') " +
                "begin select 'true' end";
            res = Convert.ToString(database.GetScalarValue(sql));
            //     if (res == "")
            //     {
            //         ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
            //$"if (window.location.pathname.endsWith('/frmChgPass.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Old Password Did't Matched !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
            //     }
        }
        return res;
    }
}