using iTextSharp.text.pdf.codec;
using Nest;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmMergeNGroupUpdt : System.Web.UI.Page
{
	InsertErrorLogs inEr = new InsertErrorLogs();
	public enum MessageType { success, error, info, warning };
	protected void ShowMessage(MessageType type, string Message)
	{
		ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
	}
	protected override void OnInit(EventArgs e)
	{
		try
		{
			//Change your condition here
			if (Session["Popup"] != null)
			{
				if (Session["Popup"].ToString() == "Insert")
				{
					ShowMessage(MessageType.success, "Changes updated  Successfully!!");

				}

				Session.Remove("Popup");
				//Session.Remove("hdnCategoryID.Value");
			}
			//	ShowCustomFields();


		}
		catch (Exception ex)
		{
			ExceptionLogging.SendErrorToText(ex);

		}
	}
	DataTable oddNumberCstmFlds;
	DataTable EvenNumberCstmFlds;
	DataTable oddNumberDdlCstmFlds;
	DataTable EvenNumberDdlCstmFlds;
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			Session["UserName"] = "Hitachi Systems India Pvt Ltd";
			

			if (Request.QueryString["Desk"] != null)
			{
				if (!IsPostBack)
				{


					FillDepartment();
					FillLocation();
					//	FillUserDetails();
					FillBasic();
					divCategory2.Attributes.Add("style", "display: none;");
					divCategory3.Attributes.Add("style", "display: none;");
					divCategory4.Attributes.Add("style", "display: none;");
					divCategory5.Attributes.Add("style", "display: none;");
					pnlDownTime.Visible = false;
					//pnlImpactGrid.Visible = false;
					//pnlRollOutGrid.Visible = false;
					pnlShowRollOutDetails.Visible = false;
					pnlShowImpactDetails.Visible = false;
					pnlTaksAssociation.Visible = false;
					btnSubmit.Visible = true;
					btnCancel.Visible = true;
					btnPrev.Visible = false;
					btnNext.Visible = false;
					string[] arr = Request.QueryString["Tickets"].ToString().Replace("%2c","").Split(',');
					FillAssignee();
					rptCategories.DataSource = arr;
						rptCategories.DataBind();
					

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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

			}
		}

	}

	protected void ShowCustomFields(string RequestType)
	{
		//try
		//{
		DataTable SD_SDCustomFields = new FillSDFields().FillSDODDNumberCustomFields(RequestType, Convert.ToString(Session["SD_OrgID"]));
		DataTable SD_SDDDLCustomFields = new FillSDFields().FillSDODDNumberDropDownCustomFields(RequestType, Convert.ToString(Session["SD_OrgID"]));

		if (SD_SDCustomFields.Rows.Count > 0)
		{


			oddNumberCstmFlds = SD_SDCustomFields;
			rptOddControl.DataSource = SD_SDCustomFields;
			rptOddControl.DataBind();
		}


		else
		{

		}

		if (SD_SDDDLCustomFields.Rows.Count > 0)
		{


			oddNumberDdlCstmFlds = SD_SDCustomFields;
			rptddlOddControl.DataSource = SD_SDDDLCustomFields;
			rptddlOddControl.DataBind();
		}


		else
		{

		}






		DataTable SD_SDEvenCustomFields = new FillSDFields().FillSDEvenNumberCustomFields(RequestType, Convert.ToString(Session["SD_OrgID"]));


		EvenNumberCstmFlds = SD_SDEvenCustomFields;
		rptEvenControl.DataSource = SD_SDEvenCustomFields;
		rptEvenControl.DataBind();

		DataTable SD_SDddlEvenCustomFields = new FillSDFields().FillSDEvenNumberDropDownCustomFields(RequestType, Convert.ToString(Session["SD_OrgID"]));


		EvenNumberDdlCstmFlds = SD_SDddlEvenCustomFields;
		rptddlEvenControl.DataSource = SD_SDddlEvenCustomFields;
		rptddlEvenControl.DataBind();



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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

			}
		}
	}

	private void FillStatus()
	{

		try
		{

			DataTable SD_Priority = new SDTemplateFileds().FillStatus(Request.QueryString["Desk"].ToString().Replace("%20", " "),ddlStatus.SelectedValue, Convert.ToString(Session["SD_OrgID"])); ;

			ddlStatus.DataSource = SD_Priority;
			ddlStatus.DataTextField = "StatusCodeRef";
			ddlStatus.DataValueField = "id";
			ddlStatus.DataBind();
			ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Status----------", "0"));


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

	private void FillCategory1()
	{
		try
		{
			using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
			{

				con.Open();
				using (SqlCommand cmd = new SqlCommand(@"SELECT CategoryCodeRef,
           Categoryref FROM [dbo].fnGetCategoryFullPathForDesk('" + (Request.QueryString["Desk"].ToString().Replace("%20", " ")) + "', 1) where Level=1 order by Categoryref asc", con))
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
								ddlCategory1.Items.Insert(0, new ListItem("----------Select Category----------", "0"));
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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

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
					(select a.Categoryref as sdCategoryFK,b.Categoryref,b.categorycoderef from dbo.fnGetCategoryFullPathForPartition(1) a  left join  dbo.fnGetCategoryFullPathForPartition(1) b on a.id=b.sdCategoryFK
					) c where c.sdCategoryFK='" + category + "' and c.Categoryref!='' order by categorycoderef asc", con))

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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

			}
			return null;
		}
	}
	protected void ddlCategory1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ShowCustomFields((Request.QueryString["Desk"].ToString().Replace("%20", " ")));
			hdnCategoryID.Value = ddlCategory1.SelectedValue.ToString();

			DataTable FillCategoryLevel2 = new DataTable();
			FillCategoryLevel2 = FillCategoryLevel(ddlCategory1.SelectedValue, 2);
			if (FillCategoryLevel2.Rows.Count > 0)
			{
				ddlCategory2.DataSource = FillCategoryLevel2;
				ddlCategory2.DataTextField = "CategoryCodeRef";
				ddlCategory2.DataValueField = "Categoryref";
				ddlCategory2.DataBind();
				ddlCategory2.Items.Insert(0, new ListItem("----------Select Category Level 2----------", "0"));
				divCategory2.Attributes.Add("style", "display: flex;");
				lblCategory2.Visible = true;
				ddlCategory2.Visible = true;
				ddlCategory2.Enabled = true;
			}
			else
			{
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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

			}
		}
	}
	protected void ddlCategory2_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ShowCustomFields(Request.QueryString["Desk"].ToString().Replace("%20", " "));
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
				ddlCategory3.Items.Insert(0, new ListItem("----------Select Category Level 3----------", "0"));
				divCategory3.Attributes.Add("style", "display: flex;");

			}
			else
			{

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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

			}
		}
	}
	protected void ddlCategory3_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ShowCustomFields(Request.QueryString["Desk"].ToString().Replace("%20", " "));
			hdnCategoryID.Value = ddlCategory3.SelectedValue.ToString();
			DataTable FillCategoryLevel4 = FillCategoryLevel(ddlCategory3.SelectedValue, 4);
			if (FillCategoryLevel4.Rows.Count > 0)
			{
				ddlCategory4.DataSource = FillCategoryLevel4;
				ddlCategory4.DataTextField = "CategoryCodeRef";
				ddlCategory4.DataValueField = "Categoryref";
				ddlCategory4.DataBind();
				ddlCategory4.Items.Insert(0, new ListItem("----------Select Category Level 4----------", "0"));
				ddlCategory4.Enabled = true;
				ddlCategory4.Visible = true;
				lblCategory4.Visible = true;
				divCategory4.Attributes.Add("style", "display: flex;");
			}
			else
			{

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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

			}
		}
	}
	protected void ddlCategory4_SelectedIndexChanged(object sender, EventArgs e)
	{

		try
		{
			ShowCustomFields(Request.QueryString["Desk"].ToString().Replace("%20", " "));
			hdnCategoryID.Value = ddlCategory4.SelectedValue.ToString();
			DataTable FillCategoryLevel4 = FillCategoryLevel(ddlCategory4.SelectedValue, 5);
			if (FillCategoryLevel4.Rows.Count > 0)
			{
				ddlCategory5.DataSource = FillCategoryLevel4;
				ddlCategory5.DataTextField = "CategoryCodeRef";
				ddlCategory5.DataValueField = "Categoryref";
				ddlCategory5.DataBind();
				ddlCategory5.Items.Insert(0, new ListItem("----------Select Category Level 4----------", "0"));
				ddlCategory5.Enabled = true;
				divCategory5.Attributes.Add("style", "display: flex;");
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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

			}
		}
	}
	protected void ddlCategory5_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ShowCustomFields(Request.QueryString["Desk"].ToString().Replace("%20", " "));
			hdnCategoryID.Value = ddlCategory5.SelectedValue.ToString();
			DataTable FillCategoryLevel4 = FillCategoryLevel(ddlCategory5.SelectedValue, 6);
			if (FillCategoryLevel4.Rows.Count > 0)
			{
				ddlCategory6.DataSource = FillCategoryLevel4;
				ddlCategory6.DataTextField = "CategoryCodeRef";
				ddlCategory6.DataValueField = "Categoryref";
				ddlCategory6.DataBind();
				ddlCategory6.Items.Insert(0, new ListItem("----------Select Category Level 4----------", "0"));
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
				ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

			}
		}
	}
	protected void FillBasic()
	{
		//try
		
		//{
		if (Request.QueryString["Desk"].ToString().Replace("%20", " ").Contains("cloud"))
		{
			pnlCloud.Visible = true;
			pnlChange.Visible = false;
			showChangeControl.Visible = false;
		}

		else if (Request.QueryString["Desk"].ToString().Replace("%20", " ").ToLower().Contains("change"))
		{
			showChangeControl.Visible = true;
			pnlChange.Visible = true;
			pnlCloud.Visible = false;
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
			btnNext.Visible = true;
			btnPrev.Visible = true;
			FillChangeType();
			FillReasonForChange();
			FillAssignee();
			pnlChange.Visible = false;
		}
		else
		{
			showChangeControl.Visible = false;
			pnlIncident.Visible = true;
			pnlCloud.Visible = false;
			pnlChange.Visible = false;

			// FillLocations();



		}
		FillStatus();
		FillCategory1();
		FillSeverity();
		FillPriority();
		FillResolutionDetails();
		ShowCustomFields(Request.QueryString["Desk"].ToString().Replace("%20", " "));

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
	private void FillResolutionDetails()
	{

		try
		{

			DataTable SD_Resolution = new FillSDFields().FillResolution(); ;

			if (SD_Resolution.Rows.Count > 0)
			{
				//  this.lb.Text = dataTable.Rows.Count.ToString();
				this.ddlResoultion.DataSource = (object)SD_Resolution;

				ddlResoultion.DataTextField = "ResolutionCodeRef";
				ddlResoultion.DataValueField = "id";
				ddlResoultion.DataBind();
				ddlResoultion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Resolution----------", "0"));
			}
			else
			{
				this.ddlResoultion.DataSource = (object)null;
				this.ddlResoultion.DataBind();
			}


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
	private void FillSeverity()
	{
		try
		{

			DataTable SD_Severity = new SDTemplateFileds().FillSeverity(Request.QueryString["Desk"].ToString().Replace("%20", " "), Convert.ToString(Session["SD_OrgID"])); ;

			ddlSeverity.DataSource = SD_Severity;
			ddlSeverity.DataTextField = "Serveritycoderef";
			ddlSeverity.DataValueField = "id";
			ddlSeverity.DataBind();
			ddlSeverity.Items.Insert(0, new ListItem("----------Select Severity----------", "0"));


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

			DataTable SD_Priority = new SDTemplateFileds().FillPriority(Request.QueryString["Desk"].ToString().Replace("%20", " "), Convert.ToString(Session["SD_OrgID"])); ;

			ddlPriority.DataSource = SD_Priority;
			ddlPriority.DataTextField = "PriorityCodeRef";
			ddlPriority.DataValueField = "id";
			ddlPriority.DataBind();
			ddlPriority.Items.Insert(0, new ListItem("----------Select Priority----------", "0"));


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

	Random r = new Random();
	private void FillDepartment()
	{
		try
		{
			DataTable FillDepartment = new SDTemplateFileds().FillDepartment(Convert.ToInt64(Session["SD_OrgID"]));

			ddlDepartment.DataSource = FillDepartment;
			ddlDepartment.DataTextField = "DepartmentName";
			ddlDepartment.DataValueField = "DepartmentCode";
			ddlDepartment.DataBind();
			ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Department----------", "0"));


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
	private void FillLocation()
	{
		try
		{
			DataTable FillLocation = new SDTemplateFileds().FillLocation(Convert.ToInt64(Session["SD_OrgID"]));

			ddlLocation.DataSource = FillLocation;
			ddlLocation.DataTextField = "LocName";
			ddlLocation.DataValueField = "LocCode";
			ddlLocation.DataBind();
			ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Location----------", "0"));


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

	private void CheckForPrevStage(string ticketnumber)
	{

		//try
		//{
			string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
			using (SqlConnection con = new SqlConnection(constr))
			{
				using (SqlCommand cmd = new SqlCommand("SD_spCheckTicketStatus", con))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Ticketref", ticketnumber);
					//	cmd.Parameters.AddWithValue("@sdStageFK", ddlStage.SelectedValue);
					cmd.Parameters.AddWithValue("@sdStatusFK", ddlStatus.SelectedValue);
					cmd.Parameters.AddWithValue("@Option", "Checkstatus");
					cmd.Parameters.Add("@ERROR", SqlDbType.Char, 100);
					cmd.Parameters["@ERROR"].Direction = ParameterDirection.Output;

					cmd.Connection = con;
					con.Open();
					cmd.ExecuteNonQuery();
					if (cmd.Parameters["@ERROR"].Value.ToString().Contains("change"))
					{
						if (ddlStatus.SelectedItem.ToString() == "Resolved" || ddlStatus.SelectedItem.ToString() == "Closed")
						{
							if ((txtResolution.Value.ToString() == " " || txtResolution.Value.ToString() == null) || ddlResoultion.SelectedValue == "0")
							{
								rfvddlResoultion.Enabled = true;
								rfvddlResoultion.Visible = true;

								rfvtxtResolution.Enabled = true;
								rfvtxtResolution.Visible = true;
								rfvddlResoultion.InitialValue = "0";
								lblMsg.Text = "Please select resolution and enter resolution description";
							}
							else
							{
								UpdateTicketDetails(ticketnumber);
							}


						}
						else
						{
							rfvddlResoultion.Enabled = false;
							rfvddlResoultion.Visible = false;
							rfvtxtResolution.Enabled = false;
							rfvtxtResolution.Visible = false;
							UpdateTicketDetails(ticketnumber);
						}
					}
					else
					{
						lblMsg.Text = (string)cmd.Parameters["@ERROR"].Value;
					}
					con.Close();
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
	//		if (ex.ToString().Contains("ThreadAbortException"))
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
	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		//try
		//{
			string Tickets = null;
			string[] arr = Request.QueryString["Tickets"].ToString().Replace("%2c", "").Split(',');
		foreach (string TicketNo in arr)
		{


			//UpdateTicketDetails();
			//if (Request.QueryString["Desk"] == "Incident")
			//{
			//	CheckForPrevStage(TicketNo);
			//}

			//else
			//{
				UpdateTicketDetails(TicketNo);
			
		//	}

		}
		Session["Popup"] = "Insert";
		ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('Changes has been made successfully on Tickets :"+ Request.QueryString["Tickets"].ToString().Replace("%2c", ",") + "');window.location.href='" + Request.RawUrl + "';", true);

		//}
		//catch (ThreadAbortException e2)
		//{
		//	Console.WriteLine("Exception message: {0}", e2.Message);
		//	Thread.ResetAbort();
		//}
		//catch (Exception ex)
		//{
		//	if (ex.ToString().Contains("ThreadAbortException"))
		//	{

		//	}
		//	else
		//	{
		//		var st = new StackTrace(ex, true);
		//		// Get the top stack frame
		//		var frame = st.GetFrame(0);
		//		// Get the line number from the stack frame
		//		var line = frame.GetFileLineNumber();
		//		inEr.InsertErrorLogsF(Session["UserName"].ToString()
		//, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
		//		ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

		//	}

		//}
	}
	protected void UpdateCreatedCustomField(string ticketNo)
	{
		//try
		//{
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
							cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
							cmd.Parameters.AddWithValue("@FieldValue", C.SelectedValue);



							cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
							con.Open();
							int res = cmd.ExecuteNonQuery();

						}
					}
				}
			}
//		}
//		catch (ThreadAbortException e2)
//		{
//			Console.WriteLine("Exception message: {0}", e2.Message);
//			Thread.ResetAbort();
//		}
//		catch (Exception ex)
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
	}
	public static string TicketRef;
	protected void MakeTicketSR()
	{
		//try

		//{
		using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
		{

			using (SqlCommand cmd = new SqlCommand("SD_spSDIncident", con))
			{

				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@ticketout", TicketRef);
				cmd.Parameters.AddWithValue("@UserIDForAppro", Convert.ToInt64(Session["UserID"].ToString()));
				cmd.Parameters.AddWithValue("@Option", "AddSR_ApproverStatus");
				con.Open();
				int res = cmd.ExecuteNonQuery();

				if (res > 0)
				{
					Session["Popup"] = "Insert";
					Response.Redirect(Request.Url.AbsoluteUri);
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
	private void FillAssignee()
	{
		try
		{
			DataTable FillAssigne = new SDTemplateFileds().FillAssigne(Convert.ToInt64(Session["SD_OrgID"]));

			ddlAssigne.DataSource = FillAssigne;
			ddlAssigne.DataTextField = "TechLoginName";
			ddlAssigne.DataValueField = "TechID";
			ddlAssigne.DataBind();
			ddlAssigne.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Assignee----------", "0"));


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
	static string AssigneEmail;
	protected string GetAssigneEmail(long id)
	{
		try
		{
			DataTable UpdateWIP = new SDCustomFields().GetAssigneEmail(Convert.ToInt64(ddlAssigne.SelectedValue));
			if (UpdateWIP.Rows.Count > 0)
			{
				foreach (DataRow row in UpdateWIP.Rows)
				{
					AssigneEmail = row["EmailID"].ToString();
				}
			}
			return AssigneEmail;
		}
		catch (ThreadAbortException e2)
		{
			Console.WriteLine("Exception message: {0}", e2.Message);
			Thread.ResetAbort();
			return null;
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

			return null;
		}
	}
	public static string ToEmail;
	protected void SendMailToAssignee (string ticketnum)
	{
		try
		{
			Session["AssigneUpdate"] = "True";
			ToEmail = GetAssigneEmail(Convert.ToInt64(ddlAssigne.SelectedValue));
			ShowCustomFields(ticketnum);
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

	protected void UpdateTicketDetails(string ticketnumbers)
	{
		//try

		//{
		using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
		{

			using (SqlCommand cmd = new SqlCommand("SD_spSDMergeNupate", con))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.Parameters.AddWithValue("@Ticketref", ticketnumbers);
				if (ddlPriority.SelectedValue == "0")
				{
					cmd.Parameters.AddWithValue("@sdPriorityFK", null);
				}
				else
				{
					cmd.Parameters.AddWithValue("@sdPriorityFK", ddlPriority.SelectedValue);
				}
				if (ddlSeverity.SelectedValue == "0")
				{
					cmd.Parameters.AddWithValue("@sdSeverityFK", null);
				}
				else
				{
					cmd.Parameters.AddWithValue("@sdSeverityFK", ddlSeverity.SelectedValue);
				}
	
		
	
				//cmd.Parameters.AddWithValue("@closedDateTime","");
			
				//cmd.Parameters.AddWithValue("@TicketDesc", System.Web.HttpUtility.HtmlEncode(txtDescription.Text).ToString());
	
				cmd.Parameters.AddWithValue("@submitterType", "");
				if (ddlStatus.SelectedValue == "0")
				{
					cmd.Parameters.AddWithValue("@sdStatusFK", null);
				}
				else
				{
					cmd.Parameters.AddWithValue("@sdStatusFK", ddlStatus.SelectedValue);
				}
			
				cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
				cmd.Parameters.AddWithValue("@assigneeType", "");
				if (ddlAssigne.SelectedValue == "0")
				{
					cmd.Parameters.AddWithValue("@assigneeParticipantFK", null);
				}
				else
				{
					cmd.Parameters.AddWithValue("@assigneeParticipantFK", ddlAssigne.SelectedValue);
				}

				//cmd.Parameters.AddWithValue("@assigneePoolFK","");
				//cmd.Parameters.AddWithValue("@assigneeParticipantFK","");
				//cmd.Parameters.AddWithValue("@actualCompletedDate",);
				//cmd.Parameters.AddWithValue("@expectedDueDate",);
				//cmd.Parameters.AddWithValue("@actualResolutionDate",);
				//cmd.Parameters.AddWithValue("@organizationFK", Session[""].ToString());
				//cmd.Parameters.AddWithValue("@orgStaffFK",);
				//cmd.Parameters.AddWithValue("@sdErrorMessage",);
				if (ddlLocation.SelectedValue == "0")
				{
					cmd.Parameters.AddWithValue("@location", null);
				}
				else
				{
					cmd.Parameters.AddWithValue("@location", ddlLocation.SelectedValue);
				}
				if (ddlDepartment.SelectedValue == "0")
				{
					cmd.Parameters.AddWithValue("@Department", null);
				}
				else
				{
					cmd.Parameters.AddWithValue("@Department", ddlDepartment.SelectedValue);
				}


				//cmd.Parameters.AddWithValue("@previousStageFK",);
				if (string.IsNullOrEmpty(hdnCategoryID.Value))
				{
					cmd.Parameters.AddWithValue("@sdCategoryRef", null);
					cmd.Parameters.AddWithValue("@categoryFullText", null);
				}
				else
				{
					cmd.Parameters.AddWithValue("@sdCategoryRef", hdnCategoryID.Value);
					cmd.Parameters.AddWithValue("@categoryFullText", hdnCategoryID.Value.Replace("||", " - "));
				}
				
				//cmd.Parameters.AddWithValue("@InsertBy" Session["UserName"]);
				//cmd.Parameters.AddWithValue("@IsActive", '1');
	
				cmd.Parameters.AddWithValue("@Option", "UpdateTicketMerge");
				con.Open();
				int res = cmd.ExecuteNonQuery();
				
				///custom fields are created by default by stored procedure	


				UpdateCreatedCustomField(ticketnumbers);
				Session["Popup"] = "Insert";
				//string notes = this.PopulateBody(
				//ticketnumber, txtSubmitterName.Text, txtLoginName.Text, txtSummary.Text, ddlCategory1.SelectedItem.ToString());

				//CloudTicketDetails();

				if (res > 0)
				{
				
			
				
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
	protected void CloudTicketDetails()
	{
		//try

		//{
		using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
		{

			using (SqlCommand cmd = new SqlCommand("SD_spCloudTickDetails", con))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
			

				cmd.Parameters.AddWithValue("@UserEmail", txtUserEmail.Text);
				cmd.Parameters.AddWithValue("@AccountID", txtAccountID.Text);
				cmd.Parameters.AddWithValue("@Permisssions", "");
				cmd.Parameters.AddWithValue("@DurationFrom", txtDurationFrom.Text);
				cmd.Parameters.AddWithValue("@DurationTo", txtDurationto.Text);
				cmd.Parameters.AddWithValue("@EmailChangeReason", txtEmailReason.Text);
				cmd.Parameters.AddWithValue("@Option", "AddTickDetails");
				con.Open();
				int res = cmd.ExecuteNonQuery();

				//if (res > 0)
				//{
				//	Session["Popup"] = "Insert";
				//	Response.Redirect(Request.Url.AbsoluteUri);
				//}

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



	private void FillAssigneeForChange()
	{
		try
		{
			DataTable FillAssigne = new SDTemplateFileds().FillAssigne(Convert.ToInt64(Session["SD_OrgID"]));

			lstTechAssoc.DataSource = FillAssigne;
			lstTechAssoc.DataTextField = "TechLoginName";
			lstTechAssoc.DataValueField = "TechID";
			lstTechAssoc.DataBind();
			lstTechAssoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Assignee----------", "0"));

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
				//foreach (RepeaterItem item in rptOddControl.Items)
				//{
				DataRowView drv = e.Item.DataItem as DataRowView;

				Label lbl = e.Item.FindControl("lblOddlist") as Label;
				DropDownList selectList = e.Item.FindControl("ddlOdd") as DropDownList;
				if (selectList != null)
				{
					DataTable dt = new SDTemplateFileds().FillCustomFieldDropdown(lbl.Text);
					selectList.DataSource = dt; //your datasource
					selectList.DataTextField = lbl.Text;
					selectList.DataValueField = lbl.Text;
					selectList.DataBind();
					selectList.Items.Insert(0, new ListItem("----------Select Value----------", "0"));

					//selectList.DataTextField = "SomeColumn";
					//selectList.DataValueField = "SomeID";
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
	protected void rptddlEvenControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		try
		{

			if (EvenNumberDdlCstmFlds != null)
			{
				//foreach (RepeaterItem item in rptOddControl.Items)
				//{
				DataRowView drv = e.Item.DataItem as DataRowView;

				Label lbl = e.Item.FindControl("lblEvenlist") as Label;
				DropDownList selectList = e.Item.FindControl("ddlEven") as DropDownList;
				if (selectList != null)
				{
					DataTable dt = new SDTemplateFileds().FillCustomFieldDropdown(lbl.Text);
					selectList.DataSource = dt; //your datasource
					selectList.DataTextField = lbl.Text;
					selectList.DataValueField = lbl.Text;
					selectList.DataBind();
					selectList.Items.Insert(0, new ListItem("----------Select Value----------", "0"));
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



	/// <summary>
	/// Change  coding start from below           All functions and panel and button coding will start from below
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	/// 
	//////////////////////////////////////////////     This will add default impact details  ////////////////////////////////////////////////////////
	
	private void FillChangeType()
	{
		try
		{
			DataTable FillDepartment = new SDTemplateFileds().FillChangeType();

			ddlChangeType.DataSource = FillDepartment;
			ddlChangeType.DataTextField = "ChangeTypeRef";
			ddlChangeType.DataValueField = "ChangeTypeRef";
			ddlChangeType.DataBind();
			ddlChangeType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select ChangeType----------", "0"));


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
			ddlRFC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Reason----------", "0"));


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
		//creating dataTable 
		DataTable dt = new DataTable();
		DataRow dr;
		dt.TableName = "Impact";
		dt.Columns.Add(new DataColumn("ImpactDetails", typeof(string)));

		dr = dt.NewRow();
		dt.Rows.Add(dr);
		//saving databale into viewstate 
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
			//get datatable from view state 
			DataTable dtCurrentTable = (DataTable)ViewState["Impact"];
			DataRow drCurrentRow = null;

			if (dtCurrentTable.Rows.Count > 0)
			{

				for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
				{

					//add each row into data table
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
	protected void gvAddTask_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			string item = e.Row.Cells[1].Text;
			foreach (Button button in e.Row.Cells[4].Controls.OfType<Button>())
			{
				if (button.CommandName == "Delete")
				{
					button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
				}
			}
		}
	}
	protected void gridAddImpact_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			string item = e.Row.Cells[1].Text;
			foreach (System.Web.UI.WebControls.Button button in e.Row.Cells[2].Controls.OfType<System.Web.UI.WebControls.Button>())
			{
				if (button.CommandName == "Delete")
				{
					button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
				}
			}
		}
	}
	protected void gridAddImpact_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		int index = Convert.ToInt32(e.RowIndex);
		DataTable dt = ViewState["Impact"] as DataTable;
		dt.Rows[index].Delete();
		ViewState["Impact"] = dt;
		if (dt.Rows.Count > 0)
		{
			gridAddImpact.DataSource = ViewState["Impact"];
			gridAddImpact.DataBind();
		}
		else
		{
			AddDefaultFirstRecordForImpact();
		}
	}
	protected void gvAddTask_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		int index = Convert.ToInt32(e.RowIndex);
		DataTable dt = ViewState["Task"] as DataTable;
		dt.Rows[index].Delete();
		ViewState["Task"] = dt;
		if (dt.Rows.Count > 0)
		{
			gvAddTask.DataSource = ViewState["Task"];
			gvAddTask.DataBind();
		}
		else
		{
			AddDefaultFirstRecordForTask();
		}
	}
	protected void btnShowBasicDetails_Click(object sender, EventArgs e)
	{
		pnlIncident.Visible = true;
		pnlShowImpactDetails.Visible = false;
		pnlDownTime.Visible = false;
		pnlShowRollOutDetails.Visible = false;
		pnlTaksAssociation.Visible = false;

		btnShowBasicDetails.CssClass = "btn btn-sm btnEnabled";
		btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
		btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
		btnDowntime.CssClass = "btn btn-sm btnDisabled";
		btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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

		btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
		btnImpactDetails.CssClass = "btn btn-sm btnEnabled";
		btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
		btnDowntime.CssClass = "btn btn-sm btnDisabled";
		btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";

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

		btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
		btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
		btnRolloutPlan.CssClass = "btn btn-sm btnEnabled";
		btnDowntime.CssClass = "btn btn-sm btnDisabled";
		btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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
	protected void gridAddRollOut_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		int index = Convert.ToInt32(e.RowIndex);
		DataTable dt = ViewState["RollOut"] as DataTable;
		dt.Rows[index].Delete();
		ViewState["RollOut"] = dt;
		if (dt.Rows.Count > 0)
		{
			gridAddRollOut.DataSource = ViewState["RollOut"];
			gridAddRollOut.DataBind();
		}
		else
		{
			AddDefaultFirstRecordForRollOut();
		}
	}

	protected void gridAddRollOut_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			string item = e.Row.Cells[1].Text;
			foreach (Button button in e.Row.Cells[2].Controls.OfType<Button>())
			{
				if (button.CommandName == "Delete")
				{
					button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
				}
			}
		}
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

		btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
		btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
		btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
		btnDowntime.CssClass = "btn btn-sm btnEnabled";
		btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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


		btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
		btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
		btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
		btnDowntime.CssClass = "btn btn-sm btnDisabled";
		btnTaskAssociation.CssClass = "btn btn-sm btnEnabled";

	}
	protected void btnAddTaskAssociationData_Click(object sender, EventArgs e)
	{
		AddNewRecordRowToGridForTask();
	}

	public static int next = 0;
	public static int prev = 0;
	protected void btnNext_Click(object sender, EventArgs e)
	{





		if (next < 4)
		{
			next++;
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

			btnShowBasicDetails.CssClass = "btn btn-sm btnEnabled";
			btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
			btnDowntime.CssClass = "btn btn-sm btnDisabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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

			btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
			btnImpactDetails.CssClass = "btn btn-sm btnEnabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
			btnDowntime.CssClass = "btn btn-sm btnDisabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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

			btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
			btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnEnabled";
			btnDowntime.CssClass = "btn btn-sm btnDisabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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

			btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
			btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
			btnDowntime.CssClass = "btn btn-sm btnEnabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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


			btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
			btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
			btnDowntime.CssClass = "btn btn-sm btnDisabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnEnabled";
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

			btnShowBasicDetails.CssClass = "btn btn-sm btnEnabled";
			btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
			btnDowntime.CssClass = "btn btn-sm btnDisabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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

			btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
			btnImpactDetails.CssClass = "btn btn-sm btnEnabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
			btnDowntime.CssClass = "btn btn-sm btnDisabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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

			btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
			btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnEnabled";
			btnDowntime.CssClass = "btn btn-sm btnDisabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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

			btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
			btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
			btnDowntime.CssClass = "btn btn-sm btnEnabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnDisabled";
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


			btnShowBasicDetails.CssClass = "btn btn-sm btnDisabled";
			btnImpactDetails.CssClass = "btn btn-sm btnDisabled";
			btnRolloutPlan.CssClass = "btn btn-sm btnDisabled";
			btnDowntime.CssClass = "btn btn-sm btnDisabled";
			btnTaskAssociation.CssClass = "btn btn-sm btnEnabled";
			btnNext.Enabled = false;
			btnPrev.Enabled = true;

			btnSubmit.Visible = true;
			btnCancel.Visible = true;

		}
	}
	private DataTable GetDataForTicket(string ticket)
	{
		//	try
		//	{



		long CategoryFk;
		string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
		using (SqlConnection con = new SqlConnection(constr))
		{
			using (SqlCommand cmd = new SqlCommand("select top 1 * from vSDTicket where TicketNumber=@TicketNumber order by TicketNumber asc"))
			{
				cmd.CommandType = CommandType.Text;
				//   cmd.Parameters.AddWithValue("@TicketNumber", ddlOpenticket.SelectedValue);
				cmd.Parameters.AddWithValue("@TicketNumber", ticket);
				//cmd.Parameters.AddWithValue("@TicketNumber", "IN007195");
				cmd.Connection = con;
				con.Open();
			
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);
				return dt;
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
		//		if (ex.ToString().Contains("ThreadAbort"))
		//		{

		//		}
		//		else
		//		{
		//			var st = new StackTrace(ex, true);
		//			// Get the top stack frame
		//			var frame = st.GetFrame(0);
		//			// Get the line number from the stack frame
		//			var line = frame.GetFileLineNumber();
		//			inEr.InsertErrorLogsF(""
		//, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
		//			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

		//		}
		//		//			
		//	}

	}
	protected void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				string ticketValue = e.Item.DataItem as string;

				if (!string.IsNullOrEmpty(ticketValue))
				{
					// Assuming you have a method to get the data for this ticket
					DataTable dt = GetDataForTicket(ticketValue);

					if (dt != null && dt.Rows.Count > 0)
					{
						
						DetailsView DetailsCheckInAsset = e.Item.FindControl("DetailsCheckInAsset") as DetailsView;

						if (DetailsCheckInAsset != null)
						{
							DetailsCheckInAsset.DataSource = dt;
							DetailsCheckInAsset.DataBind();
						}
					}
				}
			}
		
		}
		catch (Exception ex)
		{
		//	msg.ReportError(ex.Message);

		}
	}
	DataTable allCategories;

}