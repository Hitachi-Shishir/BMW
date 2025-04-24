using OtpNet;
using QRCoder;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;

/// <summary>
/// Summary description for GoogleAuthenticator
/// </summary>
public class GoogleAuthenticator
{
    public static string GenerateSecretKey()
    {
        var otpKey = KeyGeneration.GenerateRandomKey(20);
        return Base32Encoding.ToString(otpKey);
    }
    public static string GetOTPAuthUrl(string issuer, string user, string secretKey)
    {
        string format = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        return string.Format(format, HttpUtility.UrlEncode(issuer), HttpUtility.UrlEncode(user), HttpUtility.UrlEncode(secretKey));
    }
    public static string GenerateQRCode(string data)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeImage = qrCode.GetGraphic(10);

        using (MemoryStream ms = new MemoryStream())
        {
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] qrCodeBytes = ms.ToArray();
            string base64String = Convert.ToBase64String(qrCodeBytes);
            return base64String;

        }
    }

    public static string UserSecretKey;
    public static string GetKeyAgainstUser(string UserID)
    {

        string constr1 = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con1 = new SqlConnection(constr1))
        {
            con1.Open();
            using (SqlCommand cmd = new SqlCommand("SD_spMFA", con1))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Option", "GetUserWiseSecretKey");

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {

                            UserSecretKey = dt.Rows[0]["SecretKey"].ToString();
                        }



                    }
                }
            }
        }
        return UserSecretKey;
    }

}