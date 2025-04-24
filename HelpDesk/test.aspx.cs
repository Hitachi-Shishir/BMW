using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HelpDesk_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //test
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        // Perform validation or other logic before advancing the stepper
        if (IsValidStep1()) // Custom validation for step 1
        {
            // If valid, call the JavaScript to move to the next step
            ScriptManager.RegisterStartupScript(this, GetType(), "stepNext", "stepper.next();", true);
        }
        else
        {
            // Handle the validation error or notify the user
        }
    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        // Move to the previous step
        ScriptManager.RegisterStartupScript(this, GetType(), "stepPrev", "stepper.previous();", true);
    }

    // Custom validation logic for the first step
    private bool IsValidStep1()
    {
      

        return true;
    }


}