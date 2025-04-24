using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmGenAIReports : System.Web.UI.Page
{
    public string GetDatabaseName()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
        return builder.InitialCatalog;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
    }

    protected void btnGenReport_Click(object sender, EventArgs e)
    {
        try
        {
            lblPyoutput.Text = "";
            string output = RunPythonKB();
            string[] arr = output.Split(new string[] { "$$$" }, StringSplitOptions.None);
            string filename = "";
            if (arr.Length > 0)
            {
                filename = arr[1];
                output = arr[0];
            }
            ViewState["FilenName"] = filename;
            string script = $@"typewriterEffect(`{output.Replace("`", "\\`")}`, '{lblPyoutput.ClientID}', 10);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "TypewriterEffect", script, true);
        }
        catch { }
    }
    protected string RunPythonKB()
    {
        string ConncetionString = GetDatabaseName();
        string output = "";
        try
        {
            Process cmdProcess = new Process();
            ProcessStartInfo cmdStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            cmdProcess.StartInfo = cmdStartInfo;
            cmdProcess.Start();

            using (StreamWriter sw = cmdProcess.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("C:\\Users\\Administrator\\Documents\\Abhishek_GenAI\\Report_GenAI_Bat.bat " + Convert.ToString(ConncetionString) + " " + "\"" + Convert.ToString(txtRequirement.Text.Trim()) + "\"");

                    sw.WriteLine("exit");
                }
            }
            output = cmdProcess.StandardOutput.ReadToEnd();
            output = output.Replace("C:\\Users\\Administrator\\Documents\\Abhishek_GenAI\\Report_GenAI_Bat.bat " + Convert.ToString(ConncetionString) + " " + "\"" + Convert.ToString(txtRequirement.Text.Trim()) + "\"", "").Trim();
            string error = cmdProcess.StandardError.ReadToEnd();
            cmdProcess.WaitForExit();
            if (!string.IsNullOrEmpty(error))
            {
                //	MessageBox.Show("Error occurred:\n\n{error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //	MessageBox.Show("Commands and script executed successfully. Output:\n\n{output}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show("An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return output;
    }

    protected void btnDownloadReport_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = Convert.ToString(ViewState["FilenName"]);
            string nfile = filePath.Replace("'", "").Trim();
            filePath = nfile;
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path is empty or invalid");
            }
            string fileName = Path.GetFileName(filePath);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }
            string contentType = GetContentType(Path.GetExtension(fileName));
            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            Response.AddHeader("Content-Length", new FileInfo(filePath).Length.ToString());
            Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Response.TransmitFile(filePath);
            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Download error: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                "alert('Unable to download file. Please try again later.');", true);
        }
    }
    private string GetContentType(string fileExtension)
    {
        switch (fileExtension.ToLower())
        {
            case ".docx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".pdf":
                return "application/pdf";
            case ".xlsx":
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            case ".txt":
                return "text/plain";
            default:
                return "application/octet-stream";
        }
    }
}