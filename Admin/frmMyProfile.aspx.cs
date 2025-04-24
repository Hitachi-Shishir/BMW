using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

public partial class Admin_frmMyProfile : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    errorMessage msg = new errorMessage();
    Util obj = new Util();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            chkTheme.InputAttributes["class"] = "form-check-input";
            rbdLightTheme.InputAttributes["class"] = "form-check-input";
            rbdBlueTheme.InputAttributes["class"] = "form-check-input";
            rbdDarkTheme.InputAttributes["class"] = "form-check-input";
            rbdSemiDarkTheme.InputAttributes["class"] = "form-check-input";
            rbdBoderedTheme.InputAttributes["class"] = "form-check-input";
            if (!IsPostBack)
            {
                getPrevThemeData();
            }
            if (Session["LoginName"] != null && Session["UserScope"] != null)
            {
                if (!Page.IsPostBack)
                {

                    FillAssetDetails();

                    FillImage();
                }
            }
            else
            {
                Response.Redirect("../Default.aspx");
            }
            try
            {
                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    if (FileUpload1.HasFile)
                    {
                        btnUpload_Click();
                    }
                }
            }
            catch
            {
            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        }
    }
    public void getPrevThemeData()
    {
        if (Convert.ToString(Session["UserRole"]).ToUpper() == "ADMIN" || Convert.ToString(Session["UserRole"]).ToUpper() == "MASTER")
        {
            toggle.Visible = true;
            chkTheme.Visible = true;
            DataTable dt = obj.getdata(Convert.ToString(Session["SD_OrgID"]), Convert.ToString(Session["UserID"]));
            string ThemeModify = Convert.ToString(dt.Rows[0]["ThemeModify"]);
            string Theme = Convert.ToString(dt.Rows[0]["Theme"]);
            if (ThemeModify.ToUpper() == "TRUE")
            {
                chkTheme.Checked = true;
            }
            else
            {
                chkTheme.Checked = false;
            }
            if(Theme== "blue-theme")
            {
                rbdBlueTheme.Checked = true;
            }
            else if (Theme == "light")
            {
                rbdLightTheme.Checked = true;
            }
            else if (Theme == "dark")
            {
                rbdDarkTheme.Checked = true;
            }
            else if (Theme == "semi-dark")
            {
                rbdSemiDarkTheme.Checked = true;
            }
            else
            {
                rbdBoderedTheme.Checked = true;
            }
            string sql = "select Theme from SD_User_Master where UserID='" + Convert.ToString(Session["UserID"]) + "'  and Org_ID='" + Convert.ToString(Session["SD_OrgID"]) + "'";
            string theme = Convert.ToString(database.GetScalarValue(sql));
            if (theme != null)
            {
                string script = $"document.documentElement.setAttribute('data-bs-theme', '{theme}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTheme", script, true);
            }
        }
    }
    protected void FillImage()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select  * from SD_User_Master  where  LoginName=@Username", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Username", Session["UserName"].ToString());
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {

                                if (string.IsNullOrEmpty(dt.Rows[0]["FileData"].ToString()))
                                {
                                    img.ImageUrl = "~/dist/img/user1-128x128.jpg";
                                }

                                else
                                {
                                    string imageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dt.Rows[0]["FileData"]);
                                    //txtAccountManagerEmail.Text = dt.Rows[0]["FileData"].ToString();

                                    if (string.IsNullOrEmpty(imageUrl))
                                    {
                                        img.ImageUrl = "~/dist/img/user1-128x128.jpg";
                                    }
                                    else
                                    {
                                        img.ImageUrl = imageUrl;
                                        showimg.ImageUrl = imageUrl;
                                    }

                                }
                            }
                            else
                            {
                                img.ImageUrl = "~/dist/img/user1-128x128.jpg";

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
    private void FillAssetDetails()
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(" select *  FROM  SD_vUser where LoginName=@Username", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Username", Session["UserName"].ToString());

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            lblEmpId.Text = Convert.ToString(dt.Rows[0]["EmpID"]);
                            lblUserName.Text = Convert.ToString(dt.Rows[0]["UserName"]);
                            lblProfile.Text = Convert.ToString(dt.Rows[0]["UserName"]);
                            lblEmailID.Text = Convert.ToString(dt.Rows[0]["EmailID"]);
                            lblLoginID.Text = Convert.ToString(dt.Rows[0]["LoginName"]);
                            lblUserRole.Text = Convert.ToString(dt.Rows[0]["UserRole"]);
                            lblContactNo.Text = Convert.ToString(dt.Rows[0]["ContactNo"]);
                            lblDesignation.Text = Convert.ToString(dt.Rows[0]["Designation"]);
                            lblDomainType.Text = Convert.ToString(dt.Rows[0]["DomainType"]);

                            //DetailsCheckInAsset.DataSource = dt;
                            //DetailsCheckInAsset.DataBind();
                        }
                        else
                        {
                            DetailsCheckInAsset.DataSource = dt;
                            DetailsCheckInAsset.DataBind();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }

    protected void btnUpload_Click()
    {
        Byte[] bytes;
        string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
        string contentType = FileUpload1.PostedFile.ContentType;

        string filePath = FileUpload1.PostedFile.FileName;

        System.Drawing.Image image = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
        using (System.Drawing.Image thumbnail = image.GetThumbnailImage(120, 120, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                thumbnail.Save(memoryStream, ImageFormat.Png);
                bytes = new Byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(bytes, 0, (int)bytes.Length);
            }
        }
        //using (Stream fs = FileUpload1.PostedFile.InputStream)
        //{
        //	using (BinaryReader br = new BinaryReader(fs))
        //	{
        //byte[] bytes = br.ReadBytes((Int32)fs.Length);
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            string query = @"update  SD_User_Master set FileName=@FileName,@FileType=@FileType,   FileData  = @FileData where LoginName=@Username";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@FileName", filename);
                cmd.Parameters.AddWithValue("@FileType", contentType);
                cmd.Parameters.AddWithValue("@FileData", bytes);
                cmd.Parameters.AddWithValue("@Username", Session["UserName"].ToString());
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + HttpUtility.JavaScriptStringEncode("Uploaded Successfully !") + "');window.location ='frmMyProfile.aspx';", true);
            }
        }
    }
    public bool ThumbnailCallback()
    {
        return false;
    }
    protected void Theme_CheckedChanged(object sender, EventArgs e)
    {
        string theme = string.Empty;

        // Determine which theme is selected
        if (rbdBlueTheme.Checked)
        {
            theme = "blue-theme";
        }
        else if (rbdLightTheme.Checked)
        {
            theme = "light";
        }
        else if (rbdDarkTheme.Checked)
        {
            theme = "dark";
        }
        else if (rbdSemiDarkTheme.Checked)
        {
            theme = "semi-dark";
        }
        else if (rbdBoderedTheme.Checked)
        {
            theme = "bodered-theme";
        }
        if (chkTheme.Checked == true)
        {
            ThemeChangeCommon(theme, "");
        }
        else
        {
            ThemeChangeCommon(theme, Convert.ToString(Session["UserID"]));
        }
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void DetailsCheckInAsset_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
    {

    }

    protected void chkTheme_CheckedChanged(object sender, EventArgs e)
    {
        string sql = "";
        DataTable dt = obj.getdata(Convert.ToString(Session["SD_OrgID"]), Convert.ToString(Session["UserID"]));
        string theme = Convert.ToString(dt.Rows[0]["theme"]);
        if (string.IsNullOrEmpty(theme))
        {
            theme = "blue-theme";
        }
        if (chkTheme.Checked == true)
        {
            sql = "update SD_User_Master set ThemeModify=1 where Org_ID='" + Convert.ToString(Session["SD_OrgID"]) +"'";
            ThemeChangeCommon(theme, "");
        }
        else
        {
             sql = "update SD_User_Master set ThemeModify=0 where Org_ID='" + Convert.ToString(Session["SD_OrgID"]) + "'";
        }
        database.ExecuteNonQuery(sql);
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    public void ThemeChangeCommon(string theme, string user)
    {
        string sql = "update SD_User_Master set Theme='" + theme + "' where Org_ID='" + Convert.ToString(Session["SD_OrgID"]) + "'";
        if (user != "")
        {
            sql = sql + " and UserID = '" + user + "'";
        }
        database.ExecuteNonQuery(sql);
    }
   
}