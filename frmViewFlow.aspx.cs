using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmViewFlow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        if (!IsPostBack)
        {
            getFlow();
        }
    }
    public void getFlow()
    {
        try
        {
            DataTable dt = new DataTable();
            string sql = "Select top 1 * from DynamicFLow with(nolock) where Org_Id='" + Convert.ToString(Session["SD_OrgID"]) + "' order by inserttime desc  ";
            dt = database.GetDataTable(sql);
            if (dt.Rows.Count > 0 && dt != null)
            {
                string path = Convert.ToString(dt.Rows[0]["imagepath"]);
                string rootPath = Server.MapPath("~/");
                string FileName = path.Substring(path.LastIndexOf('\\'));
                string FinalPath = "\\" + "Flowcharts" + FileName;
                imgFlow.ImageUrl = ResolveUrl(FinalPath);
            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }
}