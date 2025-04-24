using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_frmActivityLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Convert.ToString(Session["SDRole"])))
        {
            Response.Redirect("/Default.aspx");
        }
        getLog();
    }
    public void getLog()
    {
        string sql = "select * from CommonLog with(nolock)";
        DataTable dt = database.GetDataTable(sql);
        grdLog.DataSource = dt;
        grdLog.DataBind();
        if (dt.Rows.Count > 0)
        {
            GridFormat(dt);
        }

    }
    protected void GridFormat(DataTable dt)
    {
        grdLog.UseAccessibleHeader = true;
        grdLog.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (grdLog.TopPagerRow != null)
        {
            grdLog.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (grdLog.BottomPagerRow != null)
        {
            grdLog.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            grdLog.FooterRow.TableSection = TableRowSection.TableFooter;
    }
}