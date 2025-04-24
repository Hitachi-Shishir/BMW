public static class ErrorsCase
{
    public static string ShowErrorType(string LoginName, string hdnCategoryID, string ddlRequestType,
        string ddlPriority, string txtDescription, string txtSummary)
    {
        string error = "";
        if (LoginName == "NA")
        {
            error = "*Login Name Cannot be Empty !";
            return error;
        }
        if (hdnCategoryID == "" || hdnCategoryID == "0")
        {
            error = "*Please Select Category !";
            return error;
        }
        if (ddlRequestType == "0" || ddlRequestType == "")
        {
            error = "*Please Select RequestType !";
            return error;
        }
        if (ddlPriority == "0" || ddlPriority == "")
        {
            error = "*Please Select Priority !";
            return error;
        }
        if (System.Web.HttpUtility.HtmlEncode(txtDescription).ToString() == "")
        {
            error = "*Description Can Not be Empty !";
            return error;
        }
        if (txtSummary == "")
        {
            error = "*Summary Can Not be Empty !";
            return error;
        }
        return error;
    }

    public static string ShowErrorTypeSR(string TicketRef, string orgid, string UserID, string OrgName)
    {
        string error = "";
        if (TicketRef == "")
        {
            error = "Ticket ID cannot be Empty !";
            return error;
        }
        if (orgid == "")
        {
            error = "Org ID cannot be Empty !";
            return error;
        }
        if (UserID == "")
        {
            error = "UserID ID cannot be Empty !";
            return error;
        }
        if (OrgName == "")
        {
            error = "OrgName cannot be Empty !";
            return error;
        }
        return error;
    }

}