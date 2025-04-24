using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_frmOrgWiseTickets : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginName"] != null && Session["UserScope"] != null)
        {
            if (!IsPostBack)
            {
                FillDDLDesk();
            }
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    private void FillDDLDesk()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SD_AllServiceDesks", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Option", "ALL");
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        adp.Fill(ds);
                        DropDesks.DataSource = ds;
                        DropDesks.DataTextField = "Desk";
                        DropDesks.DataValueField = "Desk";
                        DropDesks.DataBind();
                        DropDesks.Items.Insert(0, new ListItem("----All----", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //  msg.ReportError(ex.Message);
        }

    }
    private void FillTickets(string Option)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                string todateString = txtTo.Text;
                DateTime todate = DateTime.ParseExact(todateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string fromdateString = txtFrom.Text;
                DateTime fromdate = DateTime.ParseExact(fromdateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                using (SqlCommand cmd = new SqlCommand("SDsp_TicketDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@From", fromdate);
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@To", todate);
                    cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", Option);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {

                        adp.SelectCommand.CommandTimeout = 6000;
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            lblTotalCount.Text = dt.Rows.Count.ToString();
                            gvAllTickets.DataSource = dt;
                            gvAllTickets.DataBind();
                            GridFormat(dt);
                        }
                        else
                        {
                            gvAllTickets.EmptyDataText = "No Records Found";
                            gvAllTickets.DataSource = null;
                            gvAllTickets.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // msg.ReportError(ex.Message);
        }

    }
    protected void ImageBtnExport_Click(object sender, EventArgs e)
    {

        try
        {
            gvAllTickets.AllowPaging = false;

            FillTickets("OrgTicketsDateWise");
            DataTable dt = new DataTable("GridView_Data");
            foreach (TableCell cell in gvAllTickets.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text.Replace("&nbsp;", ""));
            }
            foreach (GridViewRow row in gvAllTickets.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Controls.Count > 0)
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = (row.Cells[i].Controls[1] as Label).Text.Replace("&nbsp;", "");
                    }
                    else
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text.Replace("&nbsp;", "");
                    }
                }
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=All Tickets.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }


        catch (Exception ex)
        {
            //msg.ReportError(ex.Message);

        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string fromd = txtFrom.Text;
        string to = txtTo.Text;

        try
        {
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (todate < frmdate)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('To Date Cannot be Greater than From Date!');window.location ='frmOrgWiseTickets.aspx';", true);
                return;
            }
            if (todate > DateTime.Today)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('To Date Cannot be Greater than Today Date!');window.location ='frmOrgWiseTickets.aspx';", true);
                return;
            }
        }
        catch (FormatException)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('Please enter valid dates in dd-MM-yyyy format.');", true);
        }


        FillTickets("OrgTicketsDateWise");
    }
    protected void gvAllTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAllTickets.PageIndex = e.NewPageIndex;
        FillTickets("OrgTicketsDateWise");
    }
    protected void GridFormat(DataTable dt)
    {
        gvAllTickets.UseAccessibleHeader = true;
        gvAllTickets.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvAllTickets.TopPagerRow != null)
        {
            gvAllTickets.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvAllTickets.BottomPagerRow != null)
        {
            gvAllTickets.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvAllTickets.FooterRow.TableSection = TableRowSection.TableFooter;
    }
}