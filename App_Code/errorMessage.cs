using System.Diagnostics;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for errorMessage
/// </summary>
public class errorMessage
{
    public string ms;
    public void ReportError(string Message)
    {
        StackFrame CallStack = new StackFrame(1, true);
        var page = HttpContext.Current.CurrentHandler as Page;
        ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert(\"" + string.Format("Message: {0}\\n\\n", Message)
        + "File: " + CallStack.GetFileName()
        + "Line No." + CallStack.GetFileLineNumber() + "\");", true);

    }
    public void ReportError1(string Message)
    {
        StackFrame CallStack = new StackFrame(1, true);

        //ms =page+ string.Format("Message: {0}\\n\\n", Message)+ "File: " + CallStack.GetFileName() + "Line No." + CallStack.GetFileLineNumber();

        ms = "Error: " + Message + "<br /> File: " + CallStack.GetFileName() + "<br /> Line: " + CallStack.GetFileLineNumber() + "<br />";


    }

}