using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmUpdateUserNotes : System.Web.UI.Page
{
	InsertErrorLogs inEr = new InsertErrorLogs();
	public enum MessageType { success, error, info, warning };
	protected void ShowMessage(MessageType type, string Message)
	{
		ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
	}
	protected override void OnInit(EventArgs e)
	{
		//try
		//{
		//Change your condition here

		if (Session["Popup"] != null)
		{
			if (Session["Popup"].ToString() == "Insert")
			{
				ShowMessage(MessageType.success, "Ticket Generated  Successfully!!");


			}
			if (Session["Popup"].ToString() == "Update")
			{
				ShowMessage(MessageType.success, "Ticket Updated Successfully!!");


			}

			Session.Remove("Popup");
			Session.Remove("CategoryID");

		}


		//}
		//catch (Exception ex)
		//{
		//	ExceptionLogging.SendErrorToText(ex);

		//}
	}
	public string TicketId;
	public string DeskRef;
	protected void Page_Load(object sender, EventArgs e)
	{
		//	try
		//{
		


		if (!IsPostBack)
		{
            if (Request.QueryString["TicketId"] != null)
            {

                TicketId = Request.QueryString["TicketId"].ToString();
                DeskRef = Request.QueryString["Desk"].ToString();
                Session["UserName"] = "User Update Remarks";
                FillSummary();
                ShowTicketAttachment();
                UpdateTicketPanel();

                lblTicket.Text = " Number :" + TicketId;

            }
            else
            {
                Response.Redirect("/frmhelpdeskcommon.aspx");
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
		//			Response.Redirect("~/Error/Error.html");

		//		}
		//	}


	}

	public static string DeskName;

	private void FillSummary()
	{
		try
		{
	


		
			string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
			using (SqlConnection con = new SqlConnection(constr))
			{
				using (SqlCommand cmd = new SqlCommand("select top 1 * from vSDTicket where TicketNumber=@TicketNumber and OrgId=@OrgId"))
				{
					cmd.CommandType = CommandType.Text;
					//cmd.Parameters.AddWithValue("@TicketNumber", ddlOpenticket.SelectedValue);
					cmd.Parameters.AddWithValue("@TicketNumber", Request.QueryString["TicketId"].ToString());
					cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
					//cmd.Parameters.AddWithValue("@TicketNumber", "IN007195");
					cmd.Connection = con;
					con.Open();
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						using (DataTable dt = new DataTable())
						{
							sda.Fill(dt);
							if (dt.Rows.Count > 0)
							{
								hdfldDesk.Value = dt.Rows[0]["ServiceDesk"].ToString();
								DeskName = dt.Rows[0]["ServiceDesk"].ToString();
								//	ddlRequestType.Items.FindByText(DeskName).Selected = true;

							
								//ddlRequestType.Items.FindByText("Incident").Selected = true;
								//ddlRequestType_SelectedIndexChanged(sender, e);
								txtSummary.Text = dt.Rows[0]["Summary"].ToString();
								//txtticketNumber.Text = dt.Rows[0]["TicketNumber"].ToString();
								txtSubmitterName.Text = dt.Rows[0]["SubmitterName"].ToString();
								txtSubmitterEmail.Text = dt.Rows[0]["SubmitterEmail"].ToString();
                                FillStage();

                                if (ddlStage.Items.FindByValue(dt.Rows[0]["sdStageFK"].ToString()) != null)
								{
									ddlStage.SelectedValue = dt.Rows[0]["sdStageFK"].ToString();
								}
								FillStatus();
								if (ddlStatus.Items.FindByValue(dt.Rows[0]["sdStatusFK"].ToString()) != null)
								{
									ddlStatus.SelectedValue = dt.Rows[0]["sdStatusFK"].ToString();
								}
							
				

					
								if (dt.Rows[0]["status"].ToString().ToLower().Contains("closed") || dt.Rows[0]["status"].ToString().ToLower().Contains("resolved"))
								{
									pnlTicket.Enabled = false;
							
								}
								else
								{
									pnlTicket.Enabled = true;
								}
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
			if (ex.ToString().Contains("ThreadAbort"))
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
			//			
		}

	}


    protected void ShowTicketAttachment()
    {


        try
        {

            DataTable SD_TicketAttach = new FillSDFields().ShowTicketAttachment(Request.QueryString["TicketId"].ToString(), Request.QueryString["NamelyId"].ToString()); ;

            if (SD_TicketAttach.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvTicketAttachment.DataSource = (object)SD_TicketAttach;
                this.gvTicketAttachment.DataBind();
            }
            else
            {
                this.gvTicketAttachment.DataSource = (object)null;
                this.gvTicketAttachment.DataBind();
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
                Response.Redirect("~/Error/Error.html");

            }

        }

    }

    protected void gvTicketAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath = e.CommandArgument.ToString();
            LinkButton lnk = (LinkButton)e.CommandSource as LinkButton;
            GridViewRow row = lnk.NamingContainer as GridViewRow;
            string pagename = gvTicketAttachment.DataKeys[row.RowIndex].Value.ToString();


            // Ensure that the Filepath is not null or empty
            if (!string.IsNullOrEmpty(filePath))
            {
                // Map the relative file path to a physical path
                string path = Server.MapPath(filePath);

                // Ensure the file exists before attempting to download
                if (System.IO.File.Exists(path))
                {
                    // Get the file name and content type
                    string fileName = Path.GetFileName(filePath);
                    string contentType = GetContentType(fileName); // Ensure this method returns correct MIME types

                    // Prepare the response for downloading the file
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.AddHeader("Content-Length", new FileInfo(path).Length.ToString());
                    Response.ContentType = contentType;

                    // Use TransmitFile for zip files, otherwise use WriteFile
                    if (contentType == "application/zip")
                    {
                        // Efficient for large files
                        Response.TransmitFile(path);
                    }
                    else
                    {
                        // Write the file to the response
                        Response.WriteFile(path);
                    }

                    Response.Flush(); // Send the data to the client

                    // Instead of Response.End(), use CompleteRequest to avoid ThreadAbortException
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    // Handle the case where the file doesn't exist
                    throw new FileNotFoundException("File not found: " + path);
                }
            }
            else
            {
                throw new ArgumentException("File path is empty or null.");
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

        }
    }
    private string GetContentType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".pdf":
                return "application/pdf";
            case ".xls":
            case ".xlsx":
                return "application/vnd.ms-excel";
            case ".doc":
            case ".docx":
                return "application/msword";
            case ".txt":
                return "text/plain";
            case ".zip":
                return "application/zip";
            default:
                return "application/octet-stream";
        }
    }

    private void FillStage()
	{

		try
		{

			DataTable SD_Status = new SDTemplateFileds().FillStage(hdfldDesk.Value, "518100112");

			ddlStage.DataSource = SD_Status;
			ddlStage.DataTextField = "StageCodeRef";
			ddlStage.DataValueField = "id";
			ddlStage.DataBind();
			ddlStage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Stage----------", "0"));


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
				Response.Redirect("~/Error/Error.html");

			}
		}
	}

	private void FillStatus()
	{

		try
		{

			DataTable SD_Priority = new SDTemplateFileds().FillStatus(hdfldDesk.Value,ddlStage.SelectedValue ,"518100112"); ;

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
				Response.Redirect("~/Error/Error.html");

			}

		}
	}



	Random r = new Random();

	protected void btnCancel_Click(object sender, EventArgs e)
	{
		Response.Redirect(Request.Url.AbsoluteUri);
	}


	protected void UpdateTicketDetails()
	{
		AddTicketNotes();
	
		

		using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
		{

			using (SqlCommand cmd = new SqlCommand("SD_spSDIncident_userUpdate", con))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.Parameters.AddWithValue("@Ticketref", Request.QueryString["TicketId"].ToString());


                if (Session["UploadedFiles"] != null)
                {
                    FileUpload(Request.QueryString["TicketId"].ToString());
                }
             
                cmd.Parameters.AddWithValue("@organizationFK", Request.QueryString["NamelyId"].ToString());
				cmd.Parameters.AddWithValue("@UserName",txtSubmitterName.Text);
				

				cmd.Parameters.AddWithValue("@TickNotes", System.Web.HttpUtility.HtmlDecode(txtNotes.Value));
			
				cmd.Parameters.AddWithValue("@Option", "UpdateTicket");
				con.Open();
				int res = cmd.ExecuteNonQuery();
				if (res > 0)
				{
					
				
						ADDMailinDB(Request.QueryString["TicketID"].ToString());
					
					Session["Popup"] = "Update";
                    Session.Remove("UploadedTempFilePath");
                    Session.Remove("UploadedFiles");
			
				}

			}
		}

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('Ticket Notes Updated..');window.location.href='" + Request.RawUrl + "';", true);

    }

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
            string tempFilePath = Convert.ToString(Session["UploadedTempFilePath"]);
            DataTable dt = Session["UploadedFiles"] as DataTable;
            //List<string> fileNames = Session["UploadedFileNames"] as List<string>;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SD_spTicketFileupload", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure; // Indicate it's a stored procedure
                    cmd.CommandTimeout = 3600;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // Create directory for the ticket if it doesn't exist
                        string finalPath = Server.MapPath("/TicketAttachment/518100112/") + Ticketref;
                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        // Process each file in the list
                        //foreach (string fileName in dt.Rows)
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["fileName"])))
                            {
                                string sourceFile = Path.Combine(tempFilePath, Convert.ToString(dt.Rows[i]["fileName"]));
                                string destinationFile = Path.Combine(finalPath, Convert.ToString(dt.Rows[i]["fileName"]));

                                try
                                {
                                    if (File.Exists(sourceFile))
                                    {
                                        File.Move(sourceFile, destinationFile);

                                        // Clear previous parameters
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@TicketID", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@Ticketref", Ticketref);
                                        cmd.Parameters.AddWithValue("@Filepath", "/TicketAttachment/518100112/" + Ticketref + "/" + Convert.ToString(dt.Rows[i]["fileName"]));
                                        cmd.Parameters.AddWithValue("@EntryDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@InsertDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@UpdateBy", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@UpdateDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@Filename", Convert.ToString(dt.Rows[i]["fileName"]));
                                        cmd.Parameters.AddWithValue("@OrgRef", Request.QueryString["NamelyId"].ToString());
                                        cmd.Parameters.AddWithValue("@Option", "AddTicketAttach");

                                        // Execute the database command for each file
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Log or handle the file move exception
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
    protected void btnUpdate_Click(object sender, EventArgs e)
	{
		//try
		//{
			//UpdateTicketDetails();
	
				UpdateTicketDetails();
			


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
		//		Response.Redirect("~/Error/Error.html");

		//	}

		//}
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
					cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
					cmd.Parameters.AddWithValue("@Option", "AssigneUpdate");
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
				Response.Redirect("~/Error/Error.html");

			}
		}
	}
	public static string Filepath;


	protected void AddTicketNotes()
	{


		try
		{

			DataTable SD_Resolution = new FillSDFields().FillTicketNotes(Request.QueryString["TicketId"].ToString(), "518100112"); ;

			if (SD_Resolution.Rows.Count > 0)
			{
				//  this.lb.Text = dataTable.Rows.Count.ToString();
				this.gvTicketNotes.DataSource = (object)SD_Resolution;
				this.gvTicketNotes.DataBind();
			}
			else
			{
				this.gvTicketNotes.DataSource = (object)null;
				this.gvTicketNotes.DataBind();
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
				Response.Redirect("~/Error/Error.html");

			}

		}

	}

	protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
	{

	}

	protected void lnkDownload_Click(object sender, EventArgs e)
	{

		try
		{
			  // Ensure that the Filepath is not null or empty
    if (!string.IsNullOrEmpty(Filepath))
    {
        // Map the relative file path to a physical path
        string path = Server.MapPath(Filepath);
        
        // Ensure the file exists before attempting to download
        if (System.IO.File.Exists(path))
        {
            // Get the file name and content type
            string fileName = Path.GetFileName(Filepath);
            string contentType = GetContentType(fileName); // Ensure this method returns correct MIME types

            // Prepare the response for downloading the file
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.AddHeader("Content-Length", new FileInfo(path).Length.ToString());
            Response.ContentType = contentType;

            // Use TransmitFile for zip files, otherwise use WriteFile
            if (contentType == "application/zip")
            {
                // Efficient for large files
                Response.TransmitFile(path);
            }
            else
            {
                // Write the file to the response
                Response.WriteFile(path);
            }

            Response.Flush(); // Send the data to the client

            // Instead of Response.End(), use CompleteRequest to avoid ThreadAbortException
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        else
        {
            // Handle the case where the file doesn't exist
            throw new FileNotFoundException("File not found: " + path);
        }
    }
    else
    {
        throw new ArgumentException("File path is empty or null.");
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

		}
	}
	public static int AssigneClick;
	public static string ToEmail;


	protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
	{
	
		FillStatus();
	}


 protected void ImgbtnBack_Click(object sender, ImageClickEventArgs e)
    {
        string myurl = "/frmMyTickets.aspx?&redirected=true&Desk=" + Request.QueryString["Desk"].ToString();

        Response.Redirect(myurl, false);
        //Response.Redirect("~/HelpDesk/frmEnduserTicketDetails.aspx");
    }


    protected void UpdateTicketPanel()
    {
        try
        {
            pnlTicket.Visible = true;

          
         //   btnUpdateTickView.CssClass = "btn btn-sm btnEnabled";
            pnlViewNotes.Visible = false;
          //  btnViewNotes.CssClass = "btn btn-sm btnDisabled";


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

    protected void btnUpdateTickView_Click(object sender, EventArgs e)
    {
      //  Response.AppendHeader("Refresh", "1;url=frmEditTicketbyAssigne.aspx?TicketId=" + Request.QueryString["TicketId"] + "&redirected=false&Desk=" + Request.QueryString["Desk"] + "&NamelyId=" + Request.QueryString["NamelyId"] + "");
        UpdateTicketPanel();
    }
    protected void btnViewNotes_Click(object sender, EventArgs e)
    {
        try
        {
        

            pnlViewNotes.Visible = true;
        //    btnViewNotes.CssClass = "btn btn-sm btnEnabled";
            btnUpdateTickView.Enabled = true;

            pnlTicket.Visible = false;                   
            AddTicketNotes();

       //     btnUpdateTickView.CssClass = "btn btn-sm btnDisabled";
      

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


}