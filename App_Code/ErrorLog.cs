using System;
using System.IO;

public class ErrorLog
{
    public string ticket;

    public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
    public void Log(string msg, string Filename)
    {
        try
        {
            FileStream filestrm = new FileStream(fn + @"'" + Filename + "'" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter strmwriter = new StreamWriter(filestrm);
            strmwriter.BaseStream.Seek(0, SeekOrigin.End);
            strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
            strmwriter.Flush();
            strmwriter.Close();
        }
        catch (Exception ex)
        {

        }
    }
}