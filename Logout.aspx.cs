using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        database.ExecuteNonQuery("UPDATE SD_User_Master SET LoginStatus='0' ,LoginSessionID='' WHERE LoginName='" + Convert.ToString(Session["UserName"]) + "'");
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetNoServerCaching();
        HttpContext.Current.Response.Cache.SetNoStore();
        this.Session.Abandon();
        this.Session.RemoveAll();
        this.Session.Remove("UserType");
        this.Session.Remove("UserScope");
        this.Session.Remove("UserName");
        this.Session.Clear();
        this.Response.Redirect("~/Default.aspx");
    }
}