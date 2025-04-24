using System;
using System.IO;
using context = System.Web.HttpContext;
/// <summary>
/// Summary description for ExceptionLogging
/// </summary>
public static class ExceptionLogging
{

    private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation;

    public static void SendErrorToText(Exception ex)
    {
        var line = Environment.NewLine + Environment.NewLine;

        ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
        Errormsg = ex.GetType().Name.ToString();
        extype = ex.GetType().ToString();
        exurl = context.Current.Request.Url.ToString();
        ErrorLocation = ex.Message.ToString();

        try
        {
            string filepath = context.Current.Server.MapPath("~/ErrorLogFiles/");  //Text File Path

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);

            }
            filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
            if (!File.Exists(filepath))
            {

                File.Create(filepath).Dispose();
                //using (StreamWriter sw = File.AppendText(filepath))
                //{
                //    string error = "Log Written Date:" + " " + "Error Line No :" + " " + "Error Message:" + " " + "Exception Type:" + " "  + " Error Page Url:" + " " + "User Host IP:" + " " + "Error Location :" + " ";
                //    string error1 = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line + "Error Location :" + " " + ErrorLocation + line ;
                //    sw.WriteLine(error);
                //    sw.WriteLine(error1);
                //    sw.Flush();
                //    sw.Close();
                //}
            }

            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    //string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    //sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    //sw.WriteLine("-------------------------------------------------------------------------------------");
                    //sw.WriteLine(line);
                    //sw.WriteLine(error);
                    //sw.WriteLine("--------------------------------*End*------------------------------------------");
                    //sw.WriteLine(line);

                    string error = DateTime.Now.ToString() + "   " + ErrorlineNo + "   " + Errormsg + "   " + extype + "   " + exurl + "   " + hostIp + "   " + ErrorLocation + "   ";
                    sw.WriteLine(error);
                    sw.Flush();
                    sw.Close();

                }

            }
        }
        catch (Exception e)
        {
            e.ToString();

        }
    }

}