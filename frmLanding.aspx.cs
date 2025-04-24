using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmLanding : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (Session["UserName"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        if (Convert.ToString(Session["UserType"]).ToLower() == "master")
        {

        }
    }

    protected void lnkSetting_Click(object sender, EventArgs e)
    {
        mp1.Show();
    }

    protected void lnkSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void btnALMDsbrd_Click(object sender, EventArgs e)
    {
        string username = Session["UserName"] != null ? Session["UserName"].ToString() : string.Empty;
        string password = Session["PWD"] != null ? Session["PWD"].ToString() : string.Empty;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            string url = $"https://10.110.0.22:5722/frmMapPage.aspx/?redirected=true&usid={HttpUtility.UrlEncode(username)}&pwd={HttpUtility.UrlEncode(password)}&module={HttpUtility.UrlEncode("ALM")}";
            Response.Redirect(url, false);
        }
        else
        {
            // Handle the case where username or password is missing
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Session expired or invalid credentials. Please log in again.');", true);
        }
    }

    protected void lnkRPTDSB_Click(object sender, EventArgs e)
    {
        string username = Session["UserName"] != null ? Session["UserName"].ToString() : string.Empty;
        string password = Session["PWD"] != null ? Session["PWD"].ToString() : string.Empty;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            string url = $"https://pcv-demo.hitachi-systems-mc.com:5722/frmMapPage.aspx/?redirected=true&usid={HttpUtility.UrlEncode(username)}&pwd={HttpUtility.UrlEncode(password)}&module={HttpUtility.UrlEncode("CP")}";
            Response.Redirect(url, false);
        }
        else
        {
            // Handle the case where username or password is missing
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Session expired or invalid credentials. Please log in again.');", true);
        }
    }

    protected void lnkBtnSD_Click(object sender, EventArgs e)
    {
        Response.Redirect("/frmAllTickets.aspx");
    }

    protected void lnklogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Logout.aspx");
    }
}