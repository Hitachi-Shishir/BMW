using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmAddIncident : System.Web.UI.Page
{
    Random r = new Random();
    InsertErrorLogs inEr = new InsertErrorLogs();
    DataTable oddNumberCstmFlds;
    DataTable EvenNumberCstmFlds;
    DataTable oddNumberDdlCstmFlds;
    DataTable EvenNumberDdlCstmFlds;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
Page.Items["SkipPace"] = true;
            if (Session["UserID"] == null || Convert.ToString(Session["UserID"]) == "")
            {
                Response.Redirect("/Default.aspx");
            }
            themeupdate();

            lblErrorMsg.Text = "";
            if (Session["SD_OrgID"] != null)
            {
                if (!IsPostBack)
                {
                    FillServDesk();
                    if (ddlRequestType.Items.FindByText("Incident") != null)
                    {
                        ddlRequestType.SelectedValue = "Incident";
                    }

                    ddlRequestType_SelectedIndexChanged(sender, e);
                  
                    FillDepartment();
                    FillLocation();
FillUser();
                   FillUserDetails();
                    divCategory2.Attributes.Add("style", "display: none;");
                    divCategory3.Attributes.Add("style", "display: none;");
                    divCategory4.Attributes.Add("style", "display: none;");
                    divCategory5.Attributes.Add("style", "display: none;");
                    pnlDownTime.Visible = false;
                    pnlShowRollOutDetails.Visible = false;
                    pnlShowImpactDetails.Visible = false;
                    pnlTaksAssociation.Visible = false;
                    btnSubmit.Visible = true;
                    btnCancel.Visible = true;
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
hfRequestType.Value = ddlRequestType.SelectedItem.Text;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }

    }

    private void FillHODEmail()
    {
        try
        {
            DataTable FillAssigne = new SDTemplateFileds().FillHOD();

            ddlHodApproval.DataSource = FillAssigne;
            ddlHodApproval.DataTextField = "Name";
            ddlHodApproval.DataValueField = "UserEmail";
            ddlHodApproval.DataBind();
            ddlHodApproval.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-----Select-----", "0"));


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
                Response.Redirect("~/Error/Error.html");

            }

        }
    }

 private void FillUser()
    {
        try
        {
            DataTable FillAssigne = new SDTemplateFileds().FillUser();

            ddloginName.DataSource = FillAssigne;
            ddloginName.DataTextField = "Name";
            ddloginName.DataValueField = "Empcode";
            ddloginName.DataBind();
           ddloginName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-----Select-----", "0"));


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
                Response.Redirect("~/Error/Error.html");

            }

        }
    }
    public void themeupdate()
    {
        try
        {
            string sql = "";
            if (Convert.ToString(Session["UserRole"]).ToUpper() == "MASTER")
            {
                sql = "select theme,ThemeModify from SD_User_Master a  where UserID='" + Convert.ToString(Session["UserID"]) + "'";
            }
            else
            {
                sql = "select theme,ThemeModify,OrgName from SD_User_Master a " +
                   "INNER JOIN SD_OrgMaster o ON a.Org_ID=o.Org_ID where UserID='" + Convert.ToString(Session["UserID"]) + "'  and o.Org_ID='" + Convert.ToString(Session["SD_OrgID"]) + "'";
            }
            DataTable dt = database.GetDataTable(sql);
            string theme = Convert.ToString(dt.Rows[0]["theme"]);
            if (theme != null)
            {
                string script = $"document.documentElement.setAttribute('data-bs-theme', '{theme}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTheme", script, true);
            }
        }
        catch { }
    }
    private void FillLocation()
    {
        try
        {
            DataTable FillLocation = new SDTemplateFileds().FillLocation(Convert.ToInt64(Session["SD_OrgID"].ToString()));

            ddlLocation.DataSource = FillLocation;
            ddlLocation.DataTextField = "LocName";
            ddlLocation.DataValueField = "LocCode";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));


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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void FillUserDetails()
    {
        string constr1 = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con1 = new SqlConnection(constr1))
        {
            con1.Open();
            using (SqlCommand cmd = new SqlCommand("select * from  SD_vUser where LoginName=@LoginName ", con1))
            {
                cmd.Parameters.AddWithValue("@LoginName", Session["LoginName"].ToString());

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
 		if (ddloginName.Items.FindByValue(dt.Rows[0]["EmpID"].ToString().Trim()) != null )
                {

                 ddloginName.SelectedValue= dt.Rows[0]["EmpID"].ToString().Trim();
                }
                           // ddloginName.SelectedValue= dt.Rows[0]["EmpID"].ToString();
                            LoadAssets(ddloginName.SelectedValue);
                            txtSubmitterName.Text = dt.Rows[0]["UserName"].ToString(); ;
                            txtSubmitterEmail.Text = dt.Rows[0]["EmailID"].ToString();
                            txtPhoneNumber.Text = dt.Rows[0]["ContactNo"].ToString();
 if (ddlDepartment.Items.FindByValue(dt.Rows[0]["DepCode"].ToString().Trim()) != null )
                {

                   ddlDepartment.SelectedValue = dt.Rows[0]["DepCode"].ToString().Trim();
                }
                    if (ddlLocation.Items.FindByValue(dt.Rows[0]["LocCode"].ToString().Trim()) != null )
                {

                   ddlLocation.SelectedValue = dt.Rows[0]["LocCode"].ToString().Trim();
                }
                        }
                    }
                }
            }
        }
    }
    protected void ShowCustomFields(string RequestType)
    {
        try
        {
            DataTable SD_SDCustomFields = new FillSDFields().FillSDODDNumberCustomFields(RequestType, Session["SD_OrgID"].ToString());
            DataTable SD_SDDDLCustomFields = new FillSDFields().FillSDODDNumberDropDownCustomFields(RequestType, Session["SD_OrgID"].ToString());

            if (SD_SDCustomFields.Rows.Count > 0)
            {
                oddNumberCstmFlds = SD_SDCustomFields;
                rptOddControl.DataSource = SD_SDCustomFields;
                rptOddControl.DataBind();
            }
            if (SD_SDDDLCustomFields.Rows.Count > 0)
            {


                oddNumberDdlCstmFlds = SD_SDCustomFields;
                rptddlOddControl.DataSource = SD_SDDDLCustomFields;
                rptddlOddControl.DataBind();
            }
            DataTable SD_SDEvenCustomFields = new FillSDFields().FillSDEvenNumberCustomFields(RequestType, Session["SD_OrgID"].ToString());
            EvenNumberCstmFlds = SD_SDEvenCustomFields;
            rptEvenControl.DataSource = SD_SDEvenCustomFields;
            rptEvenControl.DataBind();

            DataTable SD_SDddlEvenCustomFields = new FillSDFields().FillSDEvenNumberDropDownCustomFields(RequestType, Session["SD_OrgID"].ToString());
            EvenNumberDdlCstmFlds = SD_SDddlEvenCustomFields;
            rptddlEvenControl.DataSource = SD_SDddlEvenCustomFields;
            rptddlEvenControl.DataBind();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }


    }
    protected void rptEvenControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {

            //	if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //	{
            if (EvenNumberCstmFlds != null)
            {
                //foreach (RepeaterItem item in rptOddControl.Items)
                //{
                DataRowView drv = e.Item.DataItem as DataRowView;
                string ID = drv["FieldMode"].ToString();
                TextBox txtodd = e.Item.FindControl("txteven") as TextBox;
                //	txtodd.TextMode = TextBoxMode.ID;
                switch (ID)
                {

                    case "DateTime":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        //txtodd.Attributes.Add("type", "DateTime");
                        //	txtodd.TextMode = TextBoxMode.Date;
                        txtodd.Attributes.Add("type", "datetime-local");
                        break;
                    case "SingleLine":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        txtodd.TextMode = TextBoxMode.SingleLine;

                        break;
                    case "varchar(500)":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        txtodd.TextMode = TextBoxMode.SingleLine;
                        txtodd.Text = "";

                        break;

                }


            }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void rptOddControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {

            //	if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //	{
            if (oddNumberCstmFlds != null)
            {
                //foreach (RepeaterItem item in rptOddControl.Items)
                //{
                DataRowView drv = e.Item.DataItem as DataRowView;
                string ID = drv["FieldMode"].ToString();
                TextBox txtodd = e.Item.FindControl("txt") as TextBox;
                //	txtodd.TextMode = TextBoxMode.ID;
                switch (ID)
                {

                    case "DateTime":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        //txtodd.Attributes.Add("type", "DateTime");

                        txtodd.Attributes.Add("type", "datetime-local");
                        break;
                    case "SingleLine":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        txtodd.TextMode = TextBoxMode.SingleLine;

                        break;
                    case "varchar(500)":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        txtodd.TextMode = TextBoxMode.SingleLine;
                        txtodd.Text = "";

                        break;
                }


            }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
  private void SearchDetailsAD()
    {
        try
        {
            System.Threading.Thread.Sleep(2000);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("select * from SD_Employee where Empcode=@samAccountName", con))
                {
                    cmd.Parameters.AddWithValue("@samAccountName",  ddloginName.SelectedValue);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
 
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtSubmitterName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                          //  txtGrade.Text = ds.Tables[0].Rows[0]["title"].ToString();
                            txtSubmitterEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                             txtPhoneNumber.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                         //  ddloginName.SelectedValue= ds.Tables[0].Rows[0]["EmpCode"].ToString();
                          //  ddldepartment.SelectedValue = ds.Tables[0].Rows[0]["department"].ToString().ToUpper();
  if (ddloginName.Items.FindByValue(ds.Tables[0].Rows[0]["EmpCode"].ToString().Trim()) != null )
                {

                   ddloginName.SelectedValue = ds.Tables[0].Rows[0]["EmpCode"].ToString().Trim();
                }
                 
  if (ddlDepartment.Items.FindByValue(ds.Tables[0].Rows[0]["Department"].ToString().Trim()) != null )
                {

                   ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["Department"].ToString().Trim();
                }
                        }
                        else
                        {
 
                        }
 
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Oops! error occured :" + ex.Message.ToString());
        }
    }



  protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
SearchDetailsAD();
       /*     string connection = ConfigurationManager.ConnectionStrings["ADConnection"].ToString();
            DirectorySearcher dssearch = new DirectorySearcher(connection);
            dssearch.Filter = "(sAMAccountName=" + ddloginName.SelectedValue+ ")";
            SearchResult sresult = dssearch.FindOne();
            if (sresult != null)
            {
                DirectoryEntry dsresult = sresult.GetDirectoryEntry();
                if (sresult.Properties["displayName"] != null && sresult.Properties["displayName"].Count > 0)
                {
                    txtSubmitterName.Text = dsresult.Properties["displayName"][0].ToString();
                }

                if (sresult.Properties["mail"] != null && sresult.Properties["mail"].Count > 0)
                {
                    txtSubmitterEmail.Text = dsresult.Properties["mail"][0].ToString();
                }
                if (sresult.Properties["telephoneNumber"] != null && sresult.Properties["telephoneNumber"].Count > 0)
                {
                    txtPhoneNumber.Text = dsresult.Properties["telephoneNumber"][0].ToString();
                }
                //if (sresult.Properties["samAccountName"] != null && sresult.Properties["samAccountName"].Count > 0)
                //{
                //	txtEmployeeID.Text = dsresult.Properties["samAccountName"][0].ToString();
                //}
                //if (sresult.Properties["title"] != null && sresult.Properties["title"].Count > 0)
                //{
                //    txtGrade.Text = dsresult.Properties["title"][0].ToString();
                //}

                //if (ddldepartment.Items.FindByValue(dsresult.Properties["department"][0].ToString().Trim()) != null && sresult.Properties["department"].Count > 0)
                //{
                //    lblerrorMsg.Text = dsresult.Properties["department"][0].ToString().Trim();

                //}
                //if (ddlLocation.Items.FindByValue(dsresult.Properties["physicalDeliveryOfficeName"][0].ToString().Trim()) != null && sresult.Properties["physicalDeliveryOfficeName"].Count > 0)
                //{
                // lblmsg.Text = dsresult.Properties["physicalDeliveryOfficeName"][0].ToString().Trim();
                //}
                //if (ddldepartment.Items.FindByValue(dsresult.Properties["department"][0].ToString().Trim()) != null && sresult.Properties["department"].Count > 0)
                //{

                //    ddldepartment.SelectedValue = dsresult.Properties["department"][0].ToString().Trim();
                //}

                //if (ddlLocation.Items.FindByValue(dsresult.Properties["physicalDeliveryOfficeName"][0].ToString().Trim()) != null && sresult.Properties["physicalDeliveryOfficeName"].Count > 0)
                //{

                //    ddlLocation.SelectedValue = dsresult.Properties["physicalDeliveryOfficeName"][0].ToString().Trim();

                //}
            }

            else
            {
                lblMsg.Text = "Error: Not found";
            }*/
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

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
            ddlRequestType.Items.Insert(0, new ListItem("--Select--", "0"));


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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillLocationss()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand("select ref,listValue from ksd.CustomFieldListValues where customFieldID=814467375778815 order by listValue asc ", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {

                        // cmd.Parameters.AddWithValue("@Option", "ProcessDetails");
                        adp.SelectCommand.CommandTimeout = 180;
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                ddlLocation.DataSource = dt;
                                ddlLocation.DataTextField = "listValue";
                                ddlLocation.DataValueField = "listValue";
                                ddlLocation.DataBind();
                                ddlLocation.Items.Insert(0, new ListItem("--Select--", "0"));
                            }

                        }
                    }
                }
            }

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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillCategory1()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT CategoryCodeRef,
           Categoryref FROM [dbo].fnGetCategoryFullPathForDesk('" + ddlRequestType.SelectedValue + "','" + Session["SD_OrgID"] + "', 1) where Level=1 order by Categoryref asc", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {

                        // cmd.Parameters.AddWithValue("@Option", "ProcessDetails");
                        adp.SelectCommand.CommandTimeout = 180;
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                ddlCategory1.DataSource = dt;
                                ddlCategory1.DataTextField = "CategoryCodeRef";
                                ddlCategory1.DataValueField = "Categoryref";
                                ddlCategory1.DataBind();
                                ddlCategory1.Items.Insert(0, new ListItem("--Select--", "0"));
                            }

                        }
                    }
                }
            }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private DataTable FillCategoryLevel(string category, int categoryLevel)
    {
        try
        {
            //	hdnVarCategoryIII.Value = hdnVarCategoryI.Value;
            DataTable dtFillCategory = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"	select Categoryref,categorycoderef from 
					(select a.Categoryref as sdCategoryFK,b.Categoryref,b.categorycoderef from dbo.fnGetCategoryFullPathForPartition(1,'" + Session["SD_OrgID"] + "') a  left join  dbo.fnGetCategoryFullPathForPartition(1,'" + Session["SD_OrgID"] + "') b on a.id=b.sdCategoryFK) c where c.sdCategoryFK='" + category + "' and c.Categoryref!='' order by categorycoderef asc", con))

                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {

                        // cmd.Parameters.AddWithValue("@Option", "ProcessDetails");
                        adp.SelectCommand.CommandTimeout = 180;
                        adp.Fill(dtFillCategory);
                        //using (DataTable dt = new DataTable())
                        //{


                        //}
                    }
                }
            }
            return dtFillCategory;

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
                Response.Redirect("~/frmAddIncident.aspx");

            }
            return null;
        }
    }
    protected void ddlCategory1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowCustomFields(ddlRequestType.SelectedItem.ToString());
            hdnCategoryID.Value = ddlCategory1.SelectedValue.ToString();

            DataTable FillCategoryLevel2 = new DataTable();
            FillCategoryLevel2 = FillCategoryLevel(ddlCategory1.SelectedValue, 2);
            if (FillCategoryLevel2.Rows.Count > 0)
            {
                ddlCategory2.DataSource = FillCategoryLevel2;
                ddlCategory2.DataTextField = "CategoryCodeRef";
                ddlCategory2.DataValueField = "Categoryref";
                ddlCategory2.DataBind();
                ddlCategory2.Items.Insert(0, new ListItem("--Select--", "0"));
                divCategory2.Attributes.Add("style", "display: block;");
                lblCategory2.Visible = true;
                ddlCategory2.Visible = true;
                ddlCategory2.Enabled = true;
                RfvddlCategory2.Enabled = true;
                RfvddlCategory3.Enabled = false;
                rfvddlCategory4.Enabled = false;
            }
            else
            {
                RfvddlCategory2.Enabled = false;
                RfvddlCategory2.Visible = false;
                RfvddlCategory3.Enabled = false;
                RfvddlCategory3.Visible = false;
                rfvddlCategory4.Enabled = false;

                divCategory2.Attributes.Add("style", "display: none;");
                divCategory3.Attributes.Add("style", "display: none;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");
                ddlCategory2.ClearSelection();
                lblCategory2.Visible = false;

                ddlCategory2.Visible = false;
                ddlCategory2.Enabled = false;
                ddlCategory3.Enabled = false;

                ddlCategory3.ClearSelection();
                ddlCategory4.Enabled = false;

                ddlCategory5.Enabled = false;
                ddlCategory5.ClearSelection();
                FillCategoryLevel2 = null;
                //ddlCatII.DataSource = null;
                //  ddlCatII.DataBind();
                // ddlCatII.Items.Insert(0, new ListItem("----------Select Category Level 2----------", "0"));
            }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void ddlCategory2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowCustomFields(ddlRequestType.SelectedItem.ToString());
            hdnCategoryID.Value = ddlCategory2.SelectedValue.ToString();
            DataTable FillCategoryLevel3 = FillCategoryLevel(ddlCategory2.SelectedValue, 3);
            if (FillCategoryLevel3.Rows.Count > 0)
            {
                lblCategory3.Visible = true;
                ddlCategory3.Visible = true;
                ddlCategory3.Enabled = true;
                ddlCategory3.DataSource = FillCategoryLevel3;
                ddlCategory3.DataTextField = "CategoryCodeRef";
                ddlCategory3.DataValueField = "Categoryref";
                ddlCategory3.DataBind();
                ddlCategory3.Items.Insert(0, new ListItem("--Select--", "0"));
                divCategory3.Attributes.Add("style", "display: block;");
                RfvddlCategory2.Enabled = true;
                RfvddlCategory3.Enabled = true;
                rfvddlCategory4.Enabled = false;

            }
            else
            {

                RfvddlCategory2.Enabled = true;
                RfvddlCategory3.Enabled = false;
                rfvddlCategory4.Enabled = false;
                divCategory3.Attributes.Add("style", "display: none;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");
                ddlCategory3.ClearSelection();
                //ddlCategory3.Enabled = false;
                lblCategory3.Visible = false;
                ddlCategory3.Visible = false;
                ddlCategory3.DataSource = null;
                ddlCategory3.DataBind();
                ddlCategory4.ClearSelection();
                ddlCategory4.Enabled = false;
                ddlCategory4.Visible = false;
                lblCategory4.Visible = false;
                ddlCategory5.ClearSelection();
                ddlCategory5.Enabled = false;
                ddlCategory5.Visible = false;
                lblCategory5.Visible = false;
            }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void ddlCategory3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowCustomFields(ddlRequestType.SelectedItem.ToString());
            hdnCategoryID.Value = ddlCategory3.SelectedValue.ToString();
            DataTable FillCategoryLevel4 = FillCategoryLevel(ddlCategory3.SelectedValue, 4);
            if (FillCategoryLevel4.Rows.Count > 0)
            {
                ddlCategory4.DataSource = FillCategoryLevel4;
                ddlCategory4.DataTextField = "CategoryCodeRef";
                ddlCategory4.DataValueField = "Categoryref";
                ddlCategory4.DataBind();
                ddlCategory4.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlCategory4.Enabled = true;
                ddlCategory4.Visible = true;
                lblCategory4.Visible = true;
                divCategory4.Attributes.Add("style", "display: block;");
                RfvddlCategory2.Enabled = true;
                RfvddlCategory3.Enabled = true;
                rfvddlCategory4.Enabled = true;
            }
            else
            {
                RfvddlCategory2.Enabled = true;
                RfvddlCategory3.Enabled = true;
                rfvddlCategory4.Enabled = false;
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");
                ddlCategory4.DataSource = null;
                ddlCategory4.DataBind();
                ddlCategory4.ClearSelection();
                ddlCategory4.Enabled = false;
                ddlCategory4.Visible = false;
                lblCategory4.Visible = false;
                ddlCategory5.ClearSelection();
                ddlCategory5.Enabled = false;
                ddlCategory5.Visible = false;
                lblCategory5.Visible = false;
            }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void ddlCategory4_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            ShowCustomFields(ddlRequestType.SelectedItem.ToString());
            hdnCategoryID.Value = ddlCategory4.SelectedValue.ToString();
            DataTable FillCategoryLevel4 = FillCategoryLevel(ddlCategory4.SelectedValue, 5);
            if (FillCategoryLevel4.Rows.Count > 0)
            {
                ddlCategory5.DataSource = FillCategoryLevel4;
                ddlCategory5.DataTextField = "CategoryCodeRef";
                ddlCategory5.DataValueField = "Categoryref";
                ddlCategory5.DataBind();
                ddlCategory5.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlCategory5.Enabled = true;
                divCategory5.Attributes.Add("style", "display: block;");
            }
            else
            {

                divCategory5.Attributes.Add("style", "display: none;");
                ddlCategory5.DataSource = null;
                ddlCategory5.DataBind();

                ddlCategory5.ClearSelection();
                ddlCategory5.Enabled = false;
                ddlCategory6.ClearSelection();
                ddlCategory6.Enabled = false;
            }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void ddlCategory5_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowCustomFields(ddlRequestType.SelectedItem.ToString());
            hdnCategoryID.Value = ddlCategory5.SelectedValue.ToString();
            DataTable FillCategoryLevel4 = FillCategoryLevel(ddlCategory5.SelectedValue, 6);
            if (FillCategoryLevel4.Rows.Count > 0)
            {
                ddlCategory6.DataSource = FillCategoryLevel4;
                ddlCategory6.DataTextField = "CategoryCodeRef";
                ddlCategory6.DataValueField = "Categoryref";
                ddlCategory6.DataBind();
                ddlCategory6.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            else
            {
                divCategory2.Attributes.Add("style", "display: none;");
                divCategory3.Attributes.Add("style", "display: none;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");
                ddlCategory6.DataSource = null;
                ddlCategory6.DataBind();

                ddlCategory6.ClearSelection();
                ddlCategory6.Enabled = false;

            }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlRequestType.SelectedItem.ToString().ToLower().Contains("change"))
            {
                showChangeControl.Visible = true;
                pnlChange.Visible = true;
                pnlSRFields.Visible = false;
                rfvddlHodApproval.Visible = false;
                AddDefaultFirstRecordForImpact();
                AddDefaultFirstRecordForRollOut();
                AddDefaultFirstRecordForTask();
                //pnlAddImpact.Visible = false;
                //pnlAddRollout.Visible = false;
                pnlDownTime.Visible = false;
                //pnlImpactGrid.Visible = false;
                //pnlRollOutGrid.Visible = false;
                pnlShowRollOutDetails.Visible = false;
                pnlShowImpactDetails.Visible = false;
                pnlTaksAssociation.Visible = false;
                btnSubmit.Visible = false;
                btnCancel.Visible = false;
                btnPrev.Enabled = false;
                FillChangeType();
                FillReasonForChange();
                FillAssignee();

            }
            
            else if (ddlRequestType.SelectedItem.ToString().ToLower().Contains("Problem"))
            {
                pnlSRFields.Visible = false;
                rfvddlHodApproval.Visible = false;
            }
            if (ddlRequestType.SelectedItem.ToString().ToLower() == "service request")
            {
                FillHODEmail();
                showChangeControl.Visible = false;
                pnlIncident.Visible = true;
                pnlChange.Visible = false;
                pnlSRFields.Visible = true;
                rfvddlHodApproval.Visible = true;
                rfvddlHodApproval.Enabled = true;
            }
            
            else
            {
                showChangeControl.Visible = false;
                pnlIncident.Visible = true;
                pnlChange.Visible = false;
                rfvddlHodApproval.Visible = false;
                pnlSRFields.Visible = false;
            }

            FillCategory1();
            FillSeverity();
            FillPriority();
            ShowCustomFields(ddlRequestType.SelectedItem.ToString());

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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }

    }
    private void FillSeverity()
    {
        try
        {

            DataTable SD_Severity = new SDTemplateFileds().FillSeverity(ddlRequestType.SelectedValue, Session["SD_OrgID"].ToString()); ;

            ddlSeverity.DataSource = SD_Severity;
            ddlSeverity.DataTextField = "Serveritycoderef";
            ddlSeverity.DataValueField = "id";
            ddlSeverity.DataBind();
            ddlSeverity.Items.Insert(0, new ListItem("--Select--", "0"));


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
    private void FillPriority()
    {

        try
        {
            DataTable SD_Priority = new SDTemplateFileds().FillPriority(ddlRequestType.SelectedValue, Session["SD_OrgID"].ToString());
            ddlPriority.DataSource = SD_Priority;
            ddlPriority.DataTextField = "PriorityCodeRef";
            ddlPriority.DataValueField = "id";
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("--Select--", "0"));
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
    private void FillDepartment()
    {
        try
        {
            DataTable FillDepartment = new SDTemplateFileds().FillDepartment(Convert.ToInt64(Session["SD_OrgID"].ToString()));

            ddlDepartment.DataSource = FillDepartment;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentCode";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));


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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        MakeTicket();

    }
    protected void UpdateCreatedCustomField(string ticketNo, string OrgId)
    {
        try
        {
            foreach (RepeaterItem item in rptOddControl.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox C = (TextBox)item.FindControl("txt");
                    Label lbl = (Label)item.FindControl("lbl");
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {

                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@TicketNo", ticketNo);
                            cmd.Parameters.AddWithValue("@OrgId", OrgId);
                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                            cmd.Parameters.AddWithValue("@FieldValue", C.Text);
                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                            con.Open();
                            int res = cmd.ExecuteNonQuery();

                        }
                    }
                }
            }
            foreach (RepeaterItem item in rptEvenControl.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox C = (TextBox)item.FindControl("txteven");
                    Label lbl = (Label)item.FindControl("lbleven");
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {

                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@TicketNo", ticketNo);
                            cmd.Parameters.AddWithValue("@OrgId", OrgId);
                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                            cmd.Parameters.AddWithValue("@FieldValue", C.Text);



                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                            con.Open();
                            int res = cmd.ExecuteNonQuery();

                        }
                    }
                }
            }
            foreach (RepeaterItem item in rptddlOddControl.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList C = (DropDownList)item.FindControl("ddlOdd");
                    Label lbl = (Label)item.FindControl("lblOddlist");
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {

                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@TicketNo", ticketNo);
                            cmd.Parameters.AddWithValue("@OrgId", OrgId);
                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                            cmd.Parameters.AddWithValue("@FieldValue", C.SelectedValue);



                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                            con.Open();
                            int res = cmd.ExecuteNonQuery();

                        }
                    }
                }
            }
            foreach (RepeaterItem item in rptddlEvenControl.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList C = (DropDownList)item.FindControl("ddlEven");
                    Label lbl = (Label)item.FindControl("lblEvenlist");
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TicketNo", ticketNo);
                            cmd.Parameters.AddWithValue("@OrgId", OrgId);
                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                            cmd.Parameters.AddWithValue("@FieldValue", C.SelectedValue);
                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                            con.Open();
                            int res = cmd.ExecuteNonQuery();

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
    public static string TicketRef;

    protected void MakeTicketSR()
    {
        string sql2 = "select top 1 OrgName from SD_OrgMaster where Org_ID='" + Session["SD_OrgID"].ToString() + "'";
        string OrgName = Convert.ToString(database.GetScalarValue(sql2));
        string Error = ErrorsCase.ShowErrorTypeSR(TicketRef, Convert.ToString(Session["SD_OrgID"]), Convert.ToString(Session["UserID"]),
           OrgName);
        if (Error != "")
        {
            lblErrorMsg.Text = Error;
            return;
        }
        ErrorLog obj = new ErrorLog();
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spSDIncident", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ticketout", TicketRef);
                    cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@HODApproval", ddlHodApproval.SelectedValue);
                    cmd.Parameters.AddWithValue("@UserIDForAppro", Convert.ToInt64(Session["UserID"].ToString()));
                    cmd.Parameters.AddWithValue("@EmpID", ddloginName.SelectedValue.ToString().Trim());
                    cmd.Parameters.AddWithValue("@Option", "AddSR_ApproverStatus");
                    cmd.Parameters.AddWithValue("@OrgName", OrgName);
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        string msg = "Your Ticket has been Created, Your Ticket No is " + TicketRef + "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddIncident.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("'" + msg + "'")}'); setTimeout(function() {{ window.location.reload(); }}, 1000); }}", true);
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
            if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    public string genTicket(int increment)
    {
        string sql = @"Select concat(ReqPrefix,right('0000000'+cast((LastUpdatedID+" + increment + ") as varchar(7)),7))  " +
            "from SD_TicketIncrementRef where ReqRef='" + ddlRequestType.SelectedValue + "' and OrgRef='" + Session["SD_OrgID"].ToString() + "'";
        string Ticketref = Convert.ToString(database.GetScalarValue(sql));
        return Ticketref;
    }
    public void AssignTech(string TechID, string Ticketref, string organizationFK)
    {
        string sql = "update SDIncident set assigneeParticipantFK='" + TechID + "' where Ticketref= '" + Ticketref + "'and organizationFK='" + organizationFK + "'";
        database.ExecuteNonQuery(sql);
        string sql1 = "select TicketCount from SD_Technician where TechID='" + TechID + "'";
        Int64 count = Convert.ToInt64(database.GetScalarValue(sql1));
        Int64 Newcount = count + 1;
        string sql2 = "update SD_Technician set TicketCount =" + Newcount + " where TechID='" + TechID + "'";
        database.ExecuteNonQuery(sql2);
    }
    public void AutoAssign(string ticketnumber)
    {
        string sql = "select top 1 TechID from SD_Technician where TicketCount=0 or TicketCount=null";
        string TechIDglevel = Convert.ToString(database.GetScalarValue(sql));
        if (TechIDglevel != "")
        {
            AssignTech(TechIDglevel, ticketnumber, Convert.ToString(Session["SD_OrgID"]));
        }
        else
        {
            string sql1 = "select top 1 TechID from SD_Technician where TicketCount=0 or TicketCount=null order by TicketCount asc";
            string TechID = Convert.ToString(database.GetScalarValue(sql));
            AssignTech(TechID, ticketnumber, Convert.ToString(Session["SD_OrgID"]));
        }
    }

    public void ScriptCall()
    {
        string jqueryScript = "<script src='assets/js/jquery-3.6.0.min.js'></script>";
        string dataTableScript = "<script src='assets/plugins/notifications/js/notification-custom-script.js'></script>";
        ClientScript.RegisterStartupScript(this.GetType(), "jqueryScript", jqueryScript, false);
        ClientScript.RegisterStartupScript(this.GetType(), "dataTableScript", dataTableScript, false);
    }

    //Getting SLA then on the basis of that  get response time and resolution time
    public string getSLA(string Deskref, string OrgId)
    {
        string sql = "select top 1 SlaName from SD_ServDeskDefn a  inner join  SD_SLAConfig b on a.SLA=b.ID where Deskref='" + Deskref + "' and OrgFk='" + OrgId + "'";
        string SlaName = Convert.ToString(database.GetScalarValue(sql));
        return SlaName;
    }
    public DateTime respTime(DateTime created, string deskRef, string orgStaffFK)
    {
        ExeRes obj = new ExeRes();
        string SlaName = getSLA(deskRef, orgStaffFK);
        DateTime respTime = DateTime.MinValue;

        if (SlaName == "Category")
        {
            respTime = obj.GetDueDateForCategory(created, deskRef, hdnCategoryID.Value, orgStaffFK);
        }
        else if (SlaName == "Priority")
        {
            respTime = obj.GetDueDateForPriority(created, deskRef, ddlPriority.SelectedValue, orgStaffFK);
        }
        else
        {
            respTime = obj.GetDueDate(created, deskRef, ddlSeverity.SelectedValue, orgStaffFK);
        }
        return respTime;
    }
    public DateTime reslnTime(DateTime created, string deskRef, string orgStaffFK)
    {
        ExeRes obj = new ExeRes();
        string SlaName = getSLA(deskRef, orgStaffFK);
        DateTime reslnTime = DateTime.MinValue;

        if (SlaName == "Category")
        {
            reslnTime = obj.GetDueDateForCategoryResolution(created, deskRef, hdnCategoryID.Value, orgStaffFK);
        }
        else if (SlaName == "Priority")
        {
            reslnTime = obj.GetDueDateForPriorityResolution(created, deskRef, ddlPriority.SelectedValue, orgStaffFK);
        }
        else
        {
            reslnTime = obj.GetDueDateForResolution(created, deskRef, ddlSeverity.SelectedValue, orgStaffFK);
        }
        return reslnTime;
    }
    protected void MakeTicket()
    {
        try
        {
            string msg = "";
            string Error = ErrorsCase.ShowErrorType(ddloginName.SelectedValue.ToString().ToUpper(), hdnCategoryID.Value,
            ddlRequestType.SelectedValue, ddlPriority.SelectedValue,
            System.Web.HttpUtility.HtmlEncode(txtDescription.Text).ToString(), txtSummary.Text);
            if (Error != "")
            {

                lblErrorMsg.Text = Error;
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
           $"warning_noti('{HttpUtility.JavaScriptStringEncode("" + Error + "")}'); setTimeout(function() {{ window.location.reload(true); }}, 100);", true);

                return;
            }
            string sql2 = "select top 1 OrgName from SD_OrgMaster where Org_ID='" + Session["SD_OrgID"].ToString() + "'";
            string OrgName = Convert.ToString(database.GetScalarValue(sql2));

            ErrorLog obj = new ErrorLog();
            string Ticketref = genTicket(1);
            obj.Log("TicketId : " + Ticketref + "MakeTicket" + r.Next() + " RequestType: " + ddlRequestType.SelectedValue + " Priority " + ddlPriority.SelectedValue +
                       " Severity: " + ddlSeverity.SelectedValue + " LoginName: " + ddloginName.SelectedValue+ " CategoryID :" + hdnCategoryID.Value
                       + " Summary: " + txtSummary.Text + " Description: " + System.Web.HttpUtility.HtmlEncode(txtDescription.Text).ToString() +
                       " SubmitterName : " + txtSubmitterName.Text + " PhoneNumber: " + txtPhoneNumber.Text + " SD_OrgID: " + Session["SD_OrgID"].ToString()
                       + " SDRole: " + Session["SDRole"].ToString() + " Location: " + ddlLocation.SelectedValue + " Department: " + ddlDepartment.SelectedValue
                       + " CategoryID: " + hdnCategoryID.Value.Replace("||", " - "), "'" + OrgName + "'_ParametersDebugNoIssue_Log");

            if (Ticketref != "")
            {
                string sql1 = "if exists(select * from SDIncident where Ticketref='" + Ticketref + "' and organizationFK='" + Session["SD_OrgID"].ToString() + "') begin select 'true' end";
                string chkduplicateTicket = Convert.ToString(database.GetScalarValue(sql1));
                if (chkduplicateTicket != "")
                {
                    obj.Log("Duplicate Error", "'" + OrgName + "'_Duplicate_Ticket_Log");
                    Ticketref = genTicket(2);
                }

            }
            //DateTime responeTime= respTime(DateTime.Now, ddlRequestType.SelectedValue,Convert.ToString(Session["SD_OrgID"]));
            //DateTime resolutionTime= reslnTime(DateTime.Now, ddlRequestType.SelectedValue,Convert.ToString(Session["SD_OrgID"]));
            //string resp = "";
            //string resl = "";
            //try
            //{
            //    resp = responeTime.ToString("yyyy-MM-dd HH:mm:ss");
            //    resl = responeTime.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //catch {
            //    resp = Convert.ToString(DateTime.Now.AddDays(1));
            //    resl = Convert.ToString(DateTime.Now.AddDays(1));
            //}
            if (ddlRequestType.SelectedItem.ToString() == "Service Request")
            {
                if (ddlHodApproval.SelectedValue == "0" || ddlHodApproval.SelectedValue == "")
                {
                    Session["Popup"] = "HOD Can Not be Empty !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('HOD Can Not be Empty !')", true);
                    return;
                }

            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand("SD_spSDIncident", con))
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 3600;
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", r.Next());
                        cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
                        cmd.Parameters.AddWithValue("@TicketNumber", Ticketref);
                        cmd.Parameters.AddWithValue("@sdPriorityFK", ddlPriority.SelectedValue);
                        cmd.Parameters.AddWithValue("@sdSeverityFK", ddlSeverity.SelectedValue);
                        cmd.Parameters.AddWithValue("@SubmitterID", ddloginName.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@sdCategoryRef", hdnCategoryID.Value);
                        cmd.Parameters.AddWithValue("@TicketSummary", txtSummary.Text);
                        cmd.Parameters.AddWithValue("@TicketDesc", System.Web.HttpUtility.HtmlEncode(txtDescription.Text).ToString());
                        cmd.Parameters.AddWithValue("@submitterType", "");
                        cmd.Parameters.AddWithValue("@submitterName", txtSubmitterName.Text);
                        cmd.Parameters.AddWithValue("@submitterEmailAddr", txtSubmitterEmail.Text);
                        cmd.Parameters.AddWithValue("@submitterPhone", txtPhoneNumber.Text);
                        cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
                        cmd.Parameters.AddWithValue("@assigneeType", "");
                        //  cmd.Parameters.AddWithValue("@expectedDueDate", Convert.ToDateTime(resp));
                        //cmd.Parameters.AddWithValue("@actualResolutionDate", Convert.ToDateTime(resl));
                        cmd.Parameters.AddWithValue("@SDRole", Session["SDRole"].ToString());
                        cmd.Parameters.AddWithValue("@sourceType", "Desk");
                        cmd.Parameters.AddWithValue("@location", ddlLocation.SelectedValue);
                        cmd.Parameters.AddWithValue("@Department", ddlDepartment.SelectedValue);
                        cmd.Parameters.AddWithValue("@categoryFullText", hdnCategoryID.Value.Replace("||", " - "));
                        cmd.Parameters.Add("@Ticketref", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@Option", "AddTicket");
                        cmd.Parameters.AddWithValue("@OrgName", OrgName);
                        int res = cmd.ExecuteNonQuery();
                        string ticketnumber = cmd.Parameters["@Ticketref"].Value.ToString();
                        string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                        TicketRef = ticketnumber;
                        ScriptCall();
                        if (res >= 1 && string.IsNullOrEmpty(ErrorChk))
                        {
                            transaction.Commit();
                            string sql = "update SD_TicketIncrementRef set LastUpdatedID=RIGHT('" + TicketRef + "', 7) where ReqRef='" + ddlRequestType.SelectedValue + "' and OrgRef='" + Convert.ToString(Session["SD_OrgID"]) + "'";
                            database.ExecuteNonQuery(sql);
                            UpdateCreatedCustomField(ticketnumber, Session["SD_OrgID"].ToString());
                            if (ddlRequestType.SelectedValue.ToString() == "Service Request")
                            {
                                MakeTicketSR();
                            }
                            if (ddlRequestType.SelectedValue.ToString() == "Change Request")
                            {

                                MakeChangeTicket(ticketnumber);
                                ADDChangeImpactDetails(ticketnumber);
                                ADDChangeRollOutDetails(ticketnumber);
                                ADDTaskForEngineer(ticketnumber);
                                //	CloudTicketDetails(ticketnumber);
                                MakeTicketSR();
                            }
                            ADDMailinDB(ticketnumber);
                            if (Session["UploadedFiles"] != null)
                            {
                                FileUpload(ticketnumber);
                            }
                            AssetTicketDetails(ticketnumber);
                            msg = "Ticket has been Created, Your Ticket Number is " + TicketRef + ".";
                            ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "showNotification",
                    $@"
if (window.location.pathname.endsWith('/frmAddIncident.aspx')) {{
    success_noti('{HttpUtility.JavaScriptStringEncode(msg)}');
    setTimeout(function() {{
        var url = window.location.href.split('?')[0];  // Remove any existing query parameters
        url += '?t=' + new Date().getTime();  // Add a timestamp to the URL to avoid cache
        window.location.href = url;  // Redirect to the new URL with a cache-busted query string
    }}, 3000);
}}",
                    true
                );
                            //msg = "Ticket has been Created, Your Ticket Number is " + TicketRef + ".";
                            //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                            //   $"if (window.location.pathname.endsWith('/frmAddIncident.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("" + msg + "")}'); setTimeout(function() {{ window.location.reload(true); }}, 200); }}", true);

                        }
                        else
                        {
                            transaction.Rollback();
                            con.Close();
                            msg = "Something Went Wrong Please try Again!";
                            if (ErrorChk == "Exists")
                            {
                                msg = "Ticket With same Summary already Exists!";
                            }
                            obj.Log("Error", "'" + OrgName + "'_Error_Log '" + msg + "'");
                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + msg + "');window.location ='frmAddIncident.aspx';", true);
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                $"warning_noti('{HttpUtility.JavaScriptStringEncode("" + msg + "")}'); setTimeout(function() {{ window.location.reload(true); }}, 200);", true);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void AssetTicketDetails(string Ticketno)
    {
        try
        {
            foreach (GridViewRow row in gvAssets.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.CheckBox chkRow = (row.FindControl("chkSelect") as System.Web.UI.WebControls.CheckBox);
                    if (chkRow.Checked == true)
                    {
                        System.Web.UI.WebControls.Label lblSerialNo = (row.Cells[1].FindControl("lblSerialNumber") as System.Web.UI.WebControls.Label);
                        DropDownList ddlOptions = (DropDownList)row.FindControl("ddlPeripherals");

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SD_spAssetTickDetails", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                                cmd.Parameters.AddWithValue("@AssetSerialNo", lblSerialNo.Text);
                                cmd.Parameters.AddWithValue("@PeriPheralSerialNo", ddlOptions.SelectedValue);
                                cmd.Parameters.AddWithValue("@OrgRef", Session["SD_OrgID"].ToString());
                                cmd.Parameters.AddWithValue("@Option", "AddTickDetails");
                                con.Open();
                                int res = cmd.ExecuteNonQuery();
                            }
                        }
                    }

                }
            }

        }
        catch (Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            inEr.InsertErrorLogsF(Session["UserName"].ToString()
, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        }
    }

    private void ADDMailinDB(string ticketNumber)
    {
        try
        {
            //Sendmail(body);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_Sendmail", con))
                {
                    cmd.Parameters.AddWithValue("@TicketNumber", ticketNumber);
                    cmd.Parameters.AddWithValue("@HODApproval", ddlHodApproval.SelectedValue);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "ReminderonTicketCreaton");
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void rptddlOddControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (oddNumberDdlCstmFlds != null)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Label lbl = e.Item.FindControl("lblOddlist") as Label;
                DropDownList selectList = e.Item.FindControl("ddlOdd") as DropDownList;
                if (selectList != null)
                {
                    DataTable dt = new SDTemplateFileds().FillCustomFieldDropdown(lbl.Text);
                    selectList.DataSource = dt;
                    selectList.DataTextField = lbl.Text;
                    selectList.DataValueField = lbl.Text;
                    selectList.DataBind();
                    selectList.Items.Insert(0, new ListItem("--Select--", "0"));
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
            if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void rptddlEvenControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (EvenNumberDdlCstmFlds != null)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Label lbl = e.Item.FindControl("lblEvenlist") as Label;
                DropDownList selectList = e.Item.FindControl("ddlEven") as DropDownList;
                if (selectList != null)
                {
                    DataTable dt = new SDTemplateFileds().FillCustomFieldDropdown(lbl.Text);
                    selectList.DataSource = dt;
                    selectList.DataTextField = lbl.Text;
                    selectList.DataValueField = lbl.Text;
                    selectList.DataBind();
                    selectList.Items.Insert(0, new ListItem("--Select--", "0"));
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
            if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillAssignee()
    {
        try
        {
            DataTable FillAssigne = new SDTemplateFileds().FillAssigne(Convert.ToInt64(Session["SD_OrgID"].ToString()));

            lstTechAssoc.DataSource = FillAssigne;
            lstTechAssoc.DataTextField = "TechLoginName";
            lstTechAssoc.DataValueField = "TechID";
            lstTechAssoc.DataBind();
            lstTechAssoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));

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
    private void FillChangeType()
    {
        try
        {
            DataTable FillDepartment = new SDTemplateFileds().FillChangeType();

            ddlChangeType.DataSource = FillDepartment;
            ddlChangeType.DataTextField = "ChangeTypeRef";
            ddlChangeType.DataValueField = "ChangeTypeRef";
            ddlChangeType.DataBind();
            ddlChangeType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));


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
    private void FillReasonForChange()
    {
        try
        {
            DataTable FillDepartment = new SDTemplateFileds().FillReasonForChange();

            ddlRFC.DataSource = FillDepartment;
            ddlRFC.DataTextField = "ReasonTypeRef";
            ddlRFC.DataValueField = "ReasonTypeRef";
            ddlRFC.DataBind();
            ddlRFC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
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
    private void AddDefaultFirstRecordForImpact()
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.TableName = "Impact";
        dt.Columns.Add(new DataColumn("ImpactDetails", typeof(string)));

        dr = dt.NewRow();
        dt.Rows.Add(dr);
        ViewState["Impact"] = dt;
        //bind Gridview
        gridAddImpact.DataSource = dt;
        gridAddImpact.DataBind();
    }
    private void AddNewRecordRowToGridForImpact()
    {
        // check view state is not null
        if (ViewState["Impact"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Impact"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["ImpactDetails"] = txtImpactDesc.Text;
                }
                //Remove initial blank row
                if (dtCurrentTable.Rows[0][0].ToString() == "")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();

                }

                //add created Rows into dataTable
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Save Data table into view state after creating each row
                ViewState["Impact"] = dtCurrentTable;
                //Bind Gridview with latest Row
                gridAddImpact.DataSource = dtCurrentTable;
                gridAddImpact.DataBind();
            }
        }
        txtImpactDesc.Text = string.Empty;


        txtImpactDesc.Focus();

    }
    private void AddDefaultFirstRecordForTask()
    {
        //creating dataTable 
        DataTable dt = new DataTable();
        DataRow dr;
        dt.TableName = "Task";
        dt.Columns.Add(new DataColumn("TaskDescription", typeof(string)));
        dt.Columns.Add(new DataColumn("Status", typeof(string)));
        dt.Columns.Add(new DataColumn("EngineerAssociation", typeof(string)));
        dr = dt.NewRow();
        dt.Rows.Add(dr);
        //saving databale into viewstate 
        ViewState["Task"] = dt;
        //bind Gridview
        gvAddTask.DataSource = dt;
        gvAddTask.DataBind();
    }
    protected void btnShowBasicDetails_Click(object sender, EventArgs e)
    {
        pnlIncident.Visible = true;
        pnlShowImpactDetails.Visible = false;
        pnlDownTime.Visible = false;
        pnlShowRollOutDetails.Visible = false;
        pnlTaksAssociation.Visible = false;

        btnShowBasicDetails.CssClass = "btn btn-sm btn-secondary btnEnabled";
        btnImpactDetails.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnRolloutPlan.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnDowntime.CssClass = "btn btn-sm btn-outline-secondary btn Disabled";
        btnTaskAssociation.CssClass = "btn btn-sm  btn-outline-secondary btnDisabled";
        next = 0;
        prev = 0;
        btnNext.Enabled = true;
        btnPrev.Enabled = false;
        btnSubmit.Visible = false; btnCancel.Visible = false;
    }
    protected void btnImpactDetails_Click(object sender, EventArgs e)
    {
        next = 1;
        prev = 1;
        btnPrev.Enabled = true;
        btnNext.Enabled = true;
        pnlShowImpactDetails.Visible = true;
        //pnlAddImpact.Visible = false;
        //pnlImpactGrid.Visible = true;
        pnlIncident.Visible = false;

        pnlDownTime.Visible = false;
        pnlShowRollOutDetails.Visible = false;
        pnlTaksAssociation.Visible = false;

        btnShowBasicDetails.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnImpactDetails.CssClass = "btn btn-sm btnEnabled btn-secondary";
        btnRolloutPlan.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnDowntime.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnTaskAssociation.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";

        btnSubmit.Visible = false; btnCancel.Visible = false;
    }
    protected void btnRolloutPlan_Click(object sender, EventArgs e)
    {
        next = 2;
        prev = 2;
        btnPrev.Enabled = true;
        btnNext.Enabled = true;
        pnlShowRollOutDetails.Visible = true;
        //	pnlAddRollout.Visible = false;
        //	pnlRollOutGrid.Visible = true;
        pnlIncident.Visible = false;
        pnlShowImpactDetails.Visible = false;
        pnlDownTime.Visible = false;
        pnlTaksAssociation.Visible = false;

        btnShowBasicDetails.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnImpactDetails.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnRolloutPlan.CssClass = "btn btn-sm btnEnabled btn-secondary";
        btnDowntime.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnTaskAssociation.CssClass = "btn btn-sm btn-outline-secondary btnDisabled";
        btnSubmit.Visible = false; btnCancel.Visible = false;

    }
    protected void btnAddImpactDetails_Click(object sender, EventArgs e)
    {
        AddNewRecordRowToGridForImpact();
    }
    protected void btnAddRollOutGrid_Click(object sender, EventArgs e)
    {
        AddNewRecordRowToGridForRollOut();
    }
    private void AddDefaultFirstRecordForRollOut()
    {
        //creating dataTable 
        DataTable dt = new DataTable();
        DataRow dr;
        dt.TableName = "RollOut";
        dt.Columns.Add(new DataColumn("RollOutDetails", typeof(string)));

        dr = dt.NewRow();
        dt.Rows.Add(dr);
        //saving databale into viewstate 
        ViewState["RollOut"] = dt;
        //bind Gridview
        gridAddRollOut.DataSource = dt;
        gridAddRollOut.DataBind();
    }
    private void AddNewRecordRowToGridForRollOut()
    {
        // check view state is not null
        if (ViewState["RollOut"] != null)
        {
            //get datatable from view state 
            DataTable dtCurrentTable = (DataTable)ViewState["RollOut"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {

                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {

                    //add each row into data table
                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RollOutDetails"] = txtRollOut.Text;



                }
                //Remove initial blank row
                if (dtCurrentTable.Rows[0][0].ToString() == "")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();

                }

                //add created Rows into dataTable
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Save Data table into view state after creating each row
                ViewState["RollOut"] = dtCurrentTable;
                //Bind Gridview with latest Row
                gridAddRollOut.DataSource = dtCurrentTable;
                gridAddRollOut.DataBind();
            }
        }
        txtRollOut.Text = string.Empty;


        txtRollOut.Focus();

    }
    private void AddNewRecordRowToGridForTask()
    {
        // check view state is not null
        if (ViewState["Task"] != null)
        {
            //get datatable from view state 
            DataTable dtCurrentTable = (DataTable)ViewState["Task"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {

                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    string message = "";
                    foreach (ListItem item in lstTechAssoc.Items)
                    {
                        if (item.Selected)
                        {
                            message += item.Text + ",";
                        }
                    }
                    message = message.TrimEnd(',');
                    //add each row into data table
                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["TaskDescription"] = txtTaskSummary.Text;
                    drCurrentRow["Status"] = ddlTaskStatus.SelectedValue;
                    drCurrentRow["EngineerAssociation"] = message;

                }
                //Remove initial blank row
                if (dtCurrentTable.Rows[0][0].ToString() == "")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();

                }

                //add created Rows into dataTable
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Save Data table into view state after creating each row
                ViewState["Task"] = dtCurrentTable;
                //Bind Gridview with latest Row
                gvAddTask.DataSource = dtCurrentTable;
                gvAddTask.DataBind();
            }
        }
        txtTaskSummary.Text = string.Empty;
        txtTaskSummary.Focus();
        ddlTaskStatus.ClearSelection();
        ddlTaskStatus.Focus();
        lstTechAssoc.ClearSelection();
        lstTechAssoc.Focus();

    }
    protected void btnDowntime_Click(object sender, EventArgs e)
    {
        next = 3;
        prev = 3;
        btnPrev.Enabled = true;
        btnNext.Enabled = true;
        pnlIncident.Visible = false;
        pnlShowImpactDetails.Visible = false;

        pnlShowRollOutDetails.Visible = false;
        pnlTaksAssociation.Visible = false;
        pnlDownTime.Visible = true;

        btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnDowntime.CssClass = "btn btn-sm btnEnabled btn-secondary";
        btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnSubmit.Visible = false; btnCancel.Visible = false;


    }
    protected void btnTaskAssociationShowPanel_Click(object sender, EventArgs e)
    {
        next = 4;
        prev = 4;
        btnNext.Enabled = false;
        btnPrev.Enabled = true;
        pnlShowRollOutDetails.Visible = false;
        //pnlAddRollout.Visible = false;
        //	pnlRollOutGrid.Visible = false;
        pnlIncident.Visible = false;
        pnlShowImpactDetails.Visible = false;
        //pnlImpactGrid.Visible = false;
        //pnlAddImpact.Visible = false;
        pnlDownTime.Visible = false;
        pnlTaksAssociation.Visible = true;


        btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnTaskAssociation.CssClass = "btn btn-sm btnEnabled btn-secondary";

    }
    protected void btnAddTaskAssociationData_Click(object sender, EventArgs e)
    {
        AddNewRecordRowToGridForTask();
    }

    public static int next = 0;
    public static int prev = 0;
    public string getPriority_Id(string priority)
    {
        string sql = "select ID from SD_Priority where DeskRef='" + ddlRequestType.SelectedValue + "' and" +
                " OrgDeskRef='" + Session["SD_OrgID"].ToString() + "' and PriorityCodeRef Like '%" + priority + "%'";
        string Priority = Convert.ToString(database.GetScalarValue(sql));
        return Priority;
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (next < 4)
        {
            next++;
        }

        if (next == 0)
        {
            pnlShowRollOutDetails.Visible = false;
            pnlIncident.Visible = true;
            pnlShowImpactDetails.Visible = false;
            pnlDownTime.Visible = false;
            pnlTaksAssociation.Visible = false;

            btnShowBasicDetails.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled  btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnNext.Enabled = true;
            btnPrev.Enabled = false;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
        }
        if (next == 1)
        {
            pnlShowImpactDetails.Visible = true;
            pnlIncident.Visible = false;

            pnlDownTime.Visible = false;
            pnlShowRollOutDetails.Visible = false;
            pnlTaksAssociation.Visible = false;

            btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnEnabled btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
        }
        if (next == 2)
        {
            ///// RollOut Details
            ///
            pnlShowRollOutDetails.Visible = true;
            //	pnlAddRollout.Visible = false;
            //	pnlRollOutGrid.Visible = true;
            pnlIncident.Visible = false;
            pnlShowImpactDetails.Visible = false;
            pnlDownTime.Visible = false;
            pnlTaksAssociation.Visible = false;

            btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;

        }
        if (next == 3)
        {
            ////////////////show downtime panel on it
            ///
            pnlIncident.Visible = false;
            pnlShowImpactDetails.Visible = false;

            pnlShowRollOutDetails.Visible = false;
            pnlTaksAssociation.Visible = false;
            pnlDownTime.Visible = true;

            btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnDowntime.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
        }
        if (next == 4)
        {
            ///////////////// task association details
            ///

            pnlShowRollOutDetails.Visible = false;
            //pnlAddRollout.Visible = false;
            //	pnlRollOutGrid.Visible = false;
            pnlIncident.Visible = false;
            pnlShowImpactDetails.Visible = false;
            //pnlImpactGrid.Visible = false;
            //pnlAddImpact.Visible = false;
            pnlDownTime.Visible = false;
            pnlTaksAssociation.Visible = true;


            btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnNext.Enabled = false;
            btnPrev.Enabled = true;

            btnSubmit.Visible = true;
            btnCancel.Visible = true;

        }
    }
    protected void btnPrev_Click(object sender, EventArgs e)
    {

        if (next >= 0)
        {
            --prev;
            --next;
        }
        if (next == 0)
        {
            pnlShowRollOutDetails.Visible = false; ;
            //	pnlAddRollout.Visible = false;
            //	pnlRollOutGrid.Visible = true;
            pnlIncident.Visible = true;
            pnlShowImpactDetails.Visible = false;
            pnlDownTime.Visible = false;
            pnlTaksAssociation.Visible = false;

            btnShowBasicDetails.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnNext.Enabled = true;
            btnPrev.Enabled = false;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
        }
        if (next == 1)
        {
            pnlShowImpactDetails.Visible = true;
            //pnlAddImpact.Visible = false;
            //pnlImpactGrid.Visible = true;
            pnlIncident.Visible = false;

            pnlDownTime.Visible = false;
            pnlShowRollOutDetails.Visible = false;
            pnlTaksAssociation.Visible = false;

            btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
        }
        if (next == 2)
        {
            ///// RollOut Details
            ///
            pnlShowRollOutDetails.Visible = true;
            //	pnlAddRollout.Visible = false;
            //	pnlRollOutGrid.Visible = true;
            pnlIncident.Visible = false;
            pnlShowImpactDetails.Visible = false;
            pnlDownTime.Visible = false;
            pnlTaksAssociation.Visible = false;

            btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;

        }
        if (next == 3)
        {
            ////////////////show downtime panel on it
            ///
            pnlIncident.Visible = false;
            pnlShowImpactDetails.Visible = false;

            pnlShowRollOutDetails.Visible = false;
            pnlTaksAssociation.Visible = false;
            pnlDownTime.Visible = true;

            btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnDowntime.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
        }
        if (next == 4)
        {
            ///////////////// task association details
            ///

            pnlShowRollOutDetails.Visible = false;
            //pnlAddRollout.Visible = false;
            //	pnlRollOutGrid.Visible = false;
            pnlIncident.Visible = false;
            pnlShowImpactDetails.Visible = false;
            //pnlImpactGrid.Visible = false;
            //pnlAddImpact.Visible = false;
            pnlDownTime.Visible = false;
            pnlTaksAssociation.Visible = true;


            btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
            btnTaskAssociation.CssClass = "btn btn-sm btnEnabled btn-secondary";
            btnNext.Enabled = false;
            btnPrev.Enabled = true;

            btnSubmit.Visible = true;
            btnCancel.Visible = true;

        }
    }
    protected void ddlSeverity_SelectedIndexChanged(object sender, EventArgs e)
    {
        string priority = "0";
        string Priority = "";
        try
        {
            if (ddlSeverity.SelectedItem.Text.Contains("S2"))
            {
                priority = "High";
                Priority = getPriority_Id(priority);
            }
            else if (ddlSeverity.SelectedItem.Text.Contains("S3"))
            {
                priority = "Medium";
                Priority = getPriority_Id(priority);
            }
            else if (ddlSeverity.SelectedItem.Text.Contains("S4"))
            {
                priority = "Low";
                Priority = getPriority_Id(priority);
            }
            else
            {
		priority = "Very High";
                Priority = getPriority_Id(priority);
                
            }
		string sql = "select ID from SD_Priority where DeskRef='" + ddlRequestType.SelectedValue + "' and" +
                                " OrgDeskRef='" + Session["SD_OrgID"].ToString() + "' and Severity = '" + ddlSeverity.SelectedValue + "'";
                Priority = Convert.ToString(database.GetScalarValue(sql));
            if (Priority != "")
            {
                ddlPriority.SelectedValue = Priority;
                ddlPriority.Enabled = false;
                rfvddlPriority.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlPriority_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Severity = "0";
        try
        {
            string Severityid = "";
            if (ddlPriority.SelectedItem.Text.Contains("High"))
            {
                Severity = "S2";
                Severityid = getSeverityId(Severity);
            }
            else if (ddlPriority.SelectedItem.Text.Contains("Medium"))
            {
                Severity = "S3";
                Severityid = getSeverityId(Severity);
            }
            else if (ddlPriority.SelectedItem.Text.Contains("Low"))
            {
                Severity = "S4";
                Severityid = getSeverityId(Severity);
            }
            else
            {
                Severity = "S1";
                Severityid = getSeverityId(Severity);

            }
		string sql = "select Severity from SD_Priority where DeskRef='" + ddlRequestType.SelectedValue + "' and" +
                                " OrgDeskRef='" + Session["SD_OrgID"].ToString() + "'";
                Severityid = Convert.ToString(database.GetScalarValue(sql));
            if (Severityid != "")
            {
                ddlSeverity.SelectedValue = Severityid;
                ddlSeverity.Enabled = false;
                RfvddlSeverity.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public string getSeverityId(string Severity)
    {
        string sql = "select ID from SD_Severity where DeskRef='" + ddlRequestType.SelectedValue + "' and" +
               " OrgDeskRef='" + Session["SD_OrgID"].ToString() + "' and Serveritycoderef Like '%" + Severity + "%'";
        string Severityid = Convert.ToString(database.GetScalarValue(sql));
        return Severityid;
    }
    //protected void btnUpload_Click(object sender, EventArgs e)
    //{
    //    if (FileUploadTickDoc.HasFiles) 
    //    {
    //        string tempPath = Server.MapPath("~/Temp/");
    //        if (!Directory.Exists(tempPath))
    //        {
    //            Directory.CreateDirectory(tempPath);
    //        }
    //        List<string> uploadedFiles = new List<string>();

    //        foreach (HttpPostedFile postedFile in FileUploadTickDoc.PostedFiles)
    //        {
    //            string fileName = Path.GetFileName(postedFile.FileName);
    //            string fullPath = Path.Combine(tempPath, fileName);

    //            try
    //            {
    //                postedFile.SaveAs(fullPath);
    //                uploadedFiles.Add(fileName); 
    //            }
    //            catch (Exception ex)
    //            {
    //                Response.Write("Error uploading file: " + fileName + " - " + ex.Message);
    //            }
    //        }
    //        lblinvoiceupload.Text = string.Join(", ", uploadedFiles);
    //        Session["UploadedFileNames"] = uploadedFiles;
    //        Session["UploadedTempFilePath"] = tempPath;
    //    }
    //}
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        Int64 Id = 0;
        if (FileUploadTickDoc.HasFiles)
        {
            DataTable dt = new DataTable();
            if (ViewState["files"] != null)
            {
                dt = ViewState["files"] as DataTable;
                Id = Convert.ToInt64(ViewState["ID"]);
            }
            string tempPath = Server.MapPath("~/Temp/");
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            List<string> uploadedFiles = new List<string>();

            foreach (HttpPostedFile postedFile in FileUploadTickDoc.PostedFiles)
            {
                Id++;
                ViewState["ID"] = Id;
                string fileName = Path.GetFileName(postedFile.FileName);
                string fullPath = Path.Combine(tempPath, fileName);

                try
                {
                    postedFile.SaveAs(fullPath);
                    uploadedFiles.Add(fileName);
                }
                catch (Exception ex)
                {
                    Response.Write("Error uploading file: " + fileName + " - " + ex.Message);
                }
                if (ViewState["files"] == null)
                {
                    dt.Columns.Add("ID", typeof(Int64));
                    dt.Columns.Add("FileName", typeof(string));
                    dt.Columns.Add("FullPath", typeof(string));
                    dt.Rows.Add(Id, fileName, fullPath);
                    dt.AcceptChanges();
                    ViewState["files"] = dt;
                }
                else
                {
                    dt.Rows.Add(Id, fileName, fullPath);
                }
            }
            StringBuilder sw = new StringBuilder();
            if (ViewState["FileName"] != null)
            {
                sw = ViewState["FileName"] as StringBuilder;
            }
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sw.Append(Convert.ToString(dt.Rows[i]["FileName"]) + ", ");
                    //lblinvoiceupload.Text = string.Join(", ", uploadedFiles);
                }
                ViewState["FileName"] = sw;
            }
            //lblinvoiceupload.Text = Convert.ToString(ViewState["FileName"]);
            // Store the uploaded file names and temp path in session
            //Session["UploadedFileNames"] = uploadedFiles;
            Session["UploadedTempFilePath"] = tempPath;
            Session["UploadedFiles"] = dt;
            grd.DataSource = dt;
            grd.DataBind();
        }
    }
    public void FileUpload(string Ticketref)
    {
        try
        {
            string ticketno = Ticketref;
            string OrgId = Convert.ToString(Session["SD_OrgID"].ToString());
            string tempFilePath = Convert.ToString(Session["UploadedTempFilePath"]);
            DataTable dt = Session["UploadedFiles"] as DataTable;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SD_spTicketFileupload", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 3600;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // string FolderPath = System.Configuration.ConfigurationManager.AppSettings["FILEPATH"].ToString();
                        string FolderPath = Server.MapPath("/TicketAttachment/") + OrgId;
                        //	string FolderPath = Server.MapPath("\\TicketAttachment\\");


                        if (!Directory.Exists(FolderPath))
                        {
                            Directory.CreateDirectory(FolderPath);
                        }
                        string TicketIdFolder = FolderPath + "\\" + ticketno;
                        if (!Directory.Exists(TicketIdFolder))
                        {
                            Directory.CreateDirectory(TicketIdFolder);
                        }
                        //   string finalPath = Server.MapPath("/TicketAttachment/") + Ticketref;
                        //if (!Directory.Exists(finalPath))
                        //{
                        //    Directory.CreateDirectory(finalPath);
                        //}

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string originalFileName = Convert.ToString(dt.Rows[i]["fileName"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["fileName"])))
                            {

                                string fileExtension = Path.GetExtension(originalFileName);
                                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);

                                // Append timestamp to avoid filename conflicts
                                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                string newFileName = $"{fileNameWithoutExt}_{timestamp}{fileExtension}";

                                string sourceFile = Path.Combine(tempFilePath, originalFileName);
                                string destinationFile = Path.Combine(TicketIdFolder, newFileName);
                                //  string sourceFile = Path.Combine(tempFilePath, Convert.ToString(dt.Rows[i]["fileName"]));
                                // string destinationFile = Path.Combine(TicketIdFolder, Convert.ToString(dt.Rows[i]["fileName"]));

                                try
                                {
                                    if (File.Exists(sourceFile))
                                    {
                                        if (File.Exists(destinationFile))
                                        {
                                            File.Delete(destinationFile);
                                        }
                                        File.Move(sourceFile, destinationFile);
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@TicketID", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@Ticketref", Ticketref);
                                        cmd.Parameters.AddWithValue("@Filepath", TicketIdFolder + "/" + Convert.ToString(dt.Rows[i]["fileName"]));
                                        cmd.Parameters.AddWithValue("@EntryDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@InsertDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@UpdateBy", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@UpdateDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@Filename", Convert.ToString(dt.Rows[i]["fileName"]));
                                        cmd.Parameters.AddWithValue("@OrgRef", Session["SD_OrgID"].ToString());
                                        cmd.Parameters.AddWithValue("@Option", "AddTicketAttach");
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Response.Write("Error processing file: " + Convert.ToString(dt.Rows[i]["fileName"]) + " - " + ex.Message);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log or handle the general error
            Response.Write("Error: " + ex.Message);
        }
    }
    protected void lnkView_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnfullPath = (HiddenField)gvr.FindControl("hdnfullPath");
        string fullPath = hdnfullPath.Value;
        string fileName = System.IO.Path.GetFileName(fullPath);
        string relativePath = "~/Temp/" + fileName;
        string url = ResolveUrl(relativePath);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "openFile", "window.open('" + url + "', '_blank');", true);
    }
    protected void lnkbtnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnid = (HiddenField)gvr.FindControl("hdnid");
        string uid = hdnid.Value;
        DataTable dt = Session["UploadedFiles"] as DataTable;
        if (dt != null)
        {
            Int64 id = Convert.ToInt64(uid);
            DataRow[] rows = dt.Select("ID = " + id);
            if (rows.Length > 0)
            {
                dt.Rows.Remove(rows[0]);
            }
            Session["UploadedFiles"] = dt;
            ViewState["files"] = dt;
            grd.DataSource = dt;
            grd.DataBind();
        }
    }
    protected void ADDChangeImpactDetails(string TicketRef)
    {
        //try

        //{
        DataTable dt = (DataTable)ViewState["Impact"];
        if (dt.Rows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                foreach (DataRow r in dt.Rows)
                {
                    using (SqlCommand cmd = new SqlCommand("SD_spAddChangeMngmtTickDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                        cmd.Parameters.AddWithValue("@ImpactDescription", r["ImpactDetails"]);
                        cmd.Parameters.AddWithValue("@Option", "AddImpactDetails");

                        int res = cmd.ExecuteNonQuery();



                    }
                }
            }
        }
        //	}
        //	catch (ThreadAbortException e2)
        //	{
        //		Console.WriteLine("Exception message: {0}", e2.Message);
        //		Thread.ResetAbort();
        //	}
        //	catch (Exception ex)
        //	{
        //		if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
        //		{

        //		}
        //		else
        //		{
        //			var st = new StackTrace(ex, true);
        //			// Get the top stack frame
        //			var frame = st.GetFrame(0);
        //			// Get the line number from the stack frame
        //			var line = frame.GetFileLineNumber();
        //			inEr.InsertErrorLogsF(Session["UserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}
        //	}
    }
    protected void ADDChangeRollOutDetails(string TicketRef)
    {
        //try

        //{
        DataTable dt = (DataTable)ViewState["RollOut"];
        if (dt.Rows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                foreach (DataRow r in dt.Rows)
                {
                    using (SqlCommand cmd = new SqlCommand("SD_spAddChangeMngmtTickDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                        cmd.Parameters.AddWithValue("@RolloutDescription", r["RollOutDetails"]);
                        cmd.Parameters.AddWithValue("@Option", "AddRollOutDetails");

                        int res = cmd.ExecuteNonQuery();



                    }
                }
            }
        }
        //	}
        //	catch (ThreadAbortException e2)
        //	{
        //		Console.WriteLine("Exception message: {0}", e2.Message);
        //		Thread.ResetAbort();
        //	}
        //	catch (Exception ex)
        //	{
        //		if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
        //		{

        //		}
        //		else
        //		{
        //			var st = new StackTrace(ex, true);
        //			// Get the top stack frame
        //			var frame = st.GetFrame(0);
        //			// Get the line number from the stack frame
        //			var line = frame.GetFileLineNumber();
        //			inEr.InsertErrorLogsF(Session["UserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}
        //	}
    }
    protected void ADDTaskForEngineer(string TicketRef)
    {
        try

        {
            DataTable dt = (DataTable)ViewState["Task"];

            if (dt.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    foreach (DataRow r in dt.Rows)
                    {
                        string[] arr = r["EngineerAssociation"].ToString().Split(',');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            using (SqlCommand cmd = new SqlCommand("SD_spAddChangeMngmtTickDetails", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                                cmd.Parameters.AddWithValue("@TaskDesc", r["TaskDescription"]);
                                cmd.Parameters.AddWithValue("@Status", r["Status"]);
                                cmd.Parameters.AddWithValue("@Assignee", arr[i]);
                                cmd.Parameters.AddWithValue("@Option", "AddTaskForTech");

                                int res = cmd.ExecuteNonQuery();



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

            }
        }
    }
    protected void MakeChangeTicket(string TicketRef)
    {
        //try

        //{
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("SD_spAddChangeMngmtTickDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                cmd.Parameters.AddWithValue("@ChangeType", ddlChangeType.SelectedValue);
                cmd.Parameters.AddWithValue("@RequestForChange", ddlRFC.SelectedValue);
                cmd.Parameters.AddWithValue("@DurationFrom", txtChangeDurationfrom.Text);
                cmd.Parameters.AddWithValue("@DurationTo", txtChangeDurationTo.Text);
                cmd.Parameters.AddWithValue("@DownTimeDesc", txtDownTimeName.Text);
                cmd.Parameters.AddWithValue("@DownTimeFrom", txtDownTimeStart.Text);
                cmd.Parameters.AddWithValue("@DownTimeTo", txtDownTimeTo.Text);
                cmd.Parameters.AddWithValue("@Option", "AddChangeTick");
                con.Open();
                int res = cmd.ExecuteNonQuery();



            }
        }
        //	}
        //	catch (ThreadAbortException e2)
        //	{
        //		Console.WriteLine("Exception message: {0}", e2.Message);
        //		Thread.ResetAbort();
        //	}
        //	catch (Exception ex)
        //	{
        //		if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
        //		{

        //		}
        //		else
        //		{
        //			var st = new StackTrace(ex, true);
        //			// Get the top stack frame
        //			var frame = st.GetFrame(0);
        //			// Get the line number from the stack frame
        //			var line = frame.GetFileLineNumber();
        //			inEr.InsertErrorLogsF(Session["UserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}
        //	}
    }

    ////////////////////////////// CMDB //////////////////
    private void LoadAssets(string EmpCode)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            string query = @"select SerialNo,Asset_Tag_ID,CategoryID,Warranty_Expiration_Date,ManufacturerID,ModelCode from ALM_AssetInventory
                            where Assigned_to_person_EMPCode='"+EmpCode+"' ";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ViewState["AssetData"] = dt;
            gvAssets.DataSource = dt;
            gvAssets.DataBind();
        }
    }

    protected void gvAssets_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string serialNo = DataBinder.Eval(e.Row.DataItem, "SerialNo").ToString();
            LoadPeripherals(serialNo);
            DropDownList ddlPeripherals = (DropDownList)e.Row.FindControl("ddlPeripherals");

            if (ddlPeripherals != null)
            {
                DataTable dt = ViewState["PeripheralData"] as DataTable;
                //  DataView dv = new DataView(dt);
                //  dv.RowFilter = "SerialNo = '" + serialNo + "'";

                ddlPeripherals.DataSource = dt;
                ddlPeripherals.DataTextField = "categoryID";
                ddlPeripherals.DataValueField = "SerialNo";
                ddlPeripherals.DataBind();

                // Add default selection
                ddlPeripherals.Items.Insert(0, new ListItem("-- Select Peripheral --", ""));
            }
        }
    }


    // Load Peripherals based on Serial No
    private void LoadPeripherals(string serialNo)
    {

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            string query = @"select categoryID,SerialNo from ALM_AssetInventory where AssetTypeCode='IT-Consumables' and PrimarySerialNo ='" + serialNo + "'";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ViewState["PeripheralData"] = dt;

        }
    }
   protected void ddloginName_SelectedIndexChanged(object sender, EventArgs e)
   {
SearchDetailsAD();
   }
}