using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmFlowChart : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (Convert.ToString(Session["UserRole"]).ToLower() != "master")
        {
            ddlOrg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
            ddlOrg.Enabled=false;
        }
        if (!IsPostBack)
        {
            FillOrganization();
            FillServDesk();
            getFlow();
            btnViewFlow.CssClass = "btn btn-sm btn-secondary";
            btnAddFlow.CssClass = "btn btn-sm btn-outline-secondary";
            btnViewFlow.Enabled = false;
            btnAddFlow.Enabled = true;
            divGO.Visible = true;
        }
    }
    private void FillOrganization()
    {

        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization(); 
            ddlOrg.DataSource = SD_Org;
            ddlOrg.DataTextField = "OrgName";
            ddlOrg.DataValueField = "Org_ID";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillServDesk()
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(Convert.ToInt64(Session["SD_OrgID"].ToString()));
            ddlRequestType.DataSource = RequestType;
            ddlRequestType.DataTextField = "ReqTypeRef";
            ddlRequestType.DataValueField = "ReqTypeRef";
            ddlRequestType.DataBind();
            ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ThreadAbortException"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string imageData = hdnImageData.Value;
        string filePath = "";
        if (!string.IsNullOrEmpty(imageData))
        {
            try
            {
                string base64Data = imageData.Substring(imageData.IndexOf(',') + 1);
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Bitmap bitmap = new Bitmap(ms))
                    {
                        string filename = Convert.ToString(Session["SD_OrgID"]) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".jpg";
                        filePath = Server.MapPath("~/Flowcharts/" + filename + "");
                        bitmap.Save(filePath, ImageFormat.Jpeg);
                    }
                }
                string shapesData = hdnShapesData.Value;
                var shapes = JsonConvert.DeserializeObject<List<Shape>>(shapesData);
                SaveShapes(shapes, filePath);
                // Provide feedback to the user

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Flowchart saved successfully!');window.location ='frmFlowChart.aspx';", true);
            }
            catch (Exception ex)
            {
                // Handle any errors during the save process
                //Response.Write($"<script>alert('Error saving image: {ex.Message}');</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('No image data found.');</script>");
        }

    }
    private void SaveShapes(List<Shape> shapes, string filePath)
    {
        Util obj = new Util();
        foreach (var shape in shapes)
        {
            string FlowDesc = shape.Text;
            if (FlowDesc == null || FlowDesc == "")
            {
                FlowDesc = "";
            }
            obj.InsertDynamicFlow(ddlOrg.SelectedValue, FlowDesc, filePath, ddlRequestType.SelectedValue);
        }
    }
    public void getFlow()
    {
        try
        {
            DataTable dt = new DataTable();
            string orgid = "";
            if (ddlOrg.SelectedValue == "0")
            {
                orgid = Convert.ToString(Session["SD_OrgID"]);
            }
            else
            {
                orgid = ddlOrg.SelectedValue;
            }
            string sql = "Select top 1 * from DynamicFLow with(nolock) where Org_Id='" + Convert.ToString(Session["SD_OrgID"]) + "' and ReqType='Incident' order by inserttime desc  ";
            dt = database.GetDataTable(sql);
            try
            {
                ddlRequestType.SelectedValue = "Incident";
                ddlOrg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
            }
            catch { }
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

    protected void btnAddFlow_Click(object sender, EventArgs e)
    {
        pnlCreateFlow.Visible = true;
        pnlView.Visible = false;
        btnAddFlow.CssClass = "btn btn-sm btn-secondary";
        btnViewFlow.CssClass = "btn btn-sm btn-outline-secondary";
        btnAddFlow.Enabled = false;
        btnViewFlow.Enabled = true;
        divGO.Visible = false;
    }

    protected void btnViewFlow_Click(object sender, EventArgs e)
    {
        pnlCreateFlow.Visible = false;
        pnlView.Visible = true;
        getFlow();
        btnViewFlow.CssClass = "btn btn-sm btn-secondary";
        btnAddFlow.CssClass = "btn btn-sm btn-outline-secondary";
        btnViewFlow.Enabled = false;
        btnAddFlow.Enabled = true;
        divGO.Visible = true;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string sql = "Select top 1 * from DynamicFLow with(nolock) where Org_Id='" + ddlOrg.SelectedValue + "' and ReqType='"+ ddlRequestType.SelectedValue + "' order by inserttime desc  ";
        dt = database.GetDataTable(sql);
        if (dt.Rows.Count > 0 && dt != null)
        {
            string path = Convert.ToString(dt.Rows[0]["imagepath"]);
            string rootPath = Server.MapPath("~/");
            string FileName = path.Substring(path.LastIndexOf('\\'));
            string FinalPath = "\\" + "Flowcharts" + FileName;
            imgFlow.ImageUrl = ResolveUrl(FinalPath);
        }
        else
        {
            imgFlow.ImageUrl = null;
        }
    }
}
public class Shape
{
    public string X { get; set; }
    public string Y { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
    public string Text { get; set; }
    public string FontSize { get; set; }
    public string FontStyle { get; set; }
    public string Color { get; set; }
}