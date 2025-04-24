using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmChangeOrgLogo : System.Web.UI.Page
{
    errorMessage msg = new errorMessage();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            {

               

					//FillAssetDetails();

					//FillImage();
					FillOrganization();
				
               

            }
       
    }
	protected void FillImage()
	{
		
			using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand(@"SD_spOrgLogo", con))
				{
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Orgid", ddlOrg.SelectedValue.ToString());
						cmd.Parameters.AddWithValue("@Option", "GetLogo");
						using (DataTable dt = new DataTable())
						{
							sda.Fill(dt);
							if (dt.Rows.Count > 0)
							{

	string imageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dt.Rows[0]["FileData"]);
								//txtAccountManagerEmail.Text = dt.Rows[0]["FileData"].ToString();

								if (string.IsNullOrEmpty(imageUrl))
								{
									//img.ImageUrl = "C:\ServiceDeskLive\ServiceDeskLive\Images\";
								}
								else
								{
									img1.ImageUrl = imageUrl;
								}

							}
							else
							{
								//img.ImageUrl = "~/Images/2008logo2.gif";
							}
						}
					}
				}
			}
		
	}
private void FillOrganization()
	{

		

			DataTable SD_Org = new FillSDFields().FillOrganization(); ;

			ddlOrg.DataSource = SD_Org;
			ddlOrg.DataTextField = "OrgName";
			ddlOrg.DataValueField = "Org_ID";
			ddlOrg.DataBind();
			ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Organization----------", "0"));


		
		
	}
	private void FillOrganisation1()
	{

		try
		{
			using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
			{

				con.Open();
				using (SqlCommand cmd = new SqlCommand("select id,organizationName from vSystemOrganizations", con))
				{
					cmd.CommandType = CommandType.Text;
					using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
					{
						using (DataSet ds = new DataSet())
						{
							DataTable dt = new DataTable();
							adp.SelectCommand.CommandTimeout = 900;
							// cmd.Parameters.AddWithValue("@Option", "FillOrganisations");
							adp.Fill(ds);
							dt = ds.Tables[0];

							ddlOrg.DataSource = dt;
							ddlOrg.DataTextField = "organizationName";
							ddlOrg.DataValueField = "id";
							ddlOrg.DataBind();
							ddlOrg.Items.Insert(0, new ListItem("----------Select Organization----------", "0"));
							con.Close();
						}
					}
				}
			}
		}

		catch (Exception ex)
		{
			msg.ReportError(ex.Message);
		}
	}
	private void FillAssetDetails()
	{
		

				string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
				using (SqlConnection con = new SqlConnection(constr))
				{
					using (SqlCommand cmd = new SqlCommand(" select top 1  *  FROM  SD_vUser where Org_ID=@Orgid", con))
					{
						cmd.CommandType = CommandType.Text;
						cmd.Parameters.AddWithValue("@Orgid", ddlOrg.SelectedValue.ToString());
					
						using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
						{
							DataTable dt = new DataTable();
							sda.Fill(dt);
							if (dt.Rows.Count > 0)
							{

								DetailsCheckInAsset.DataSource = dt;
								DetailsCheckInAsset.DataBind();
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

	protected void btnUpload_Click(object sender, EventArgs e)
	{
		Byte[] bytes;
		string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
		string contentType = FileUpload1.PostedFile.ContentType;

		string filePath = FileUpload1.PostedFile.FileName;
	
		System.Drawing.Image image = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
		using (System.Drawing.Image thumbnail = image.GetThumbnailImage(228, 68, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
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
					string query = @"SD_spOrgLogo";
					using (SqlCommand cmd = new SqlCommand(query))
					{
						cmd.Connection = con;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@FileName", filename);
						cmd.Parameters.AddWithValue("@FileType", contentType);
						cmd.Parameters.AddWithValue("@FileData", bytes);
						cmd.Parameters.AddWithValue("@Orgid", ddlOrg.SelectedValue.ToString());
						cmd.Parameters.AddWithValue("@Option", "UploadLogo");
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						Response.Redirect(Request.Url.AbsoluteUri);
					}
				}
		//	}
		//}
		//string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
		//string contentType = FileUpload1.PostedFile.ContentType;

		

	
	}
	public bool ThumbnailCallback()
	{
		return false;
	}

	protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
	{
		//FillAssetDetails();
		FillImage();
	}
}