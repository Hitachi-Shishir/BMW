using Microsoft.Exchange.WebServices.Data;
using Microsoft.Identity.Client;
using System;
using System.Configuration;

/// <summary>
/// Summary description for ReadMailUsingOth
/// </summary>
public class ReadMailUsingOth
{



    static async System.Threading.Tasks.Task Main(string[] args)
    {

        // Using Microsoft.Identity.Client 4.22.0

        // Configure the MSAL client to get tokens
        var pcaOptions = new PublicClientApplicationOptions
        {
            ClientId = ConfigurationManager.AppSettings["appId"],
            TenantId = ConfigurationManager.AppSettings["tenantId"]
        };

        var pca = PublicClientApplicationBuilder
            .CreateWithApplicationOptions(pcaOptions).Build();

        // The permission scope required for EWS access
        var ewsScopes = new string[] { "https://outlook.office365.com/EWS.AccessAsUser.All" };

        try
        {
            Write2DB db = new Write2DB();
            Console.WriteLine("Reading mail");
            // Make the interactive token request
            var authResult = await pca.AcquireTokenInteractive(ewsScopes).ExecuteAsync();

            // Configure the ExchangeService with the access token
            var ewsClient = new ExchangeService();
            ewsClient.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            ewsClient.Credentials = new OAuthCredentials(authResult.AccessToken);

            // Make an EWS call
            var folders = ewsClient.FindFolders(WellKnownFolderName.MsgFolderRoot, new FolderView(10));
            //foreach (var folder in folders)
            //{
            //
            //	Console.WriteLine($"Folder: {folder.DisplayName}");
            foreach (EmailMessage email in ewsClient.FindItems(WellKnownFolderName.Inbox, new ItemView(100)))
            {
                //	db.Save(email);

            }
            //	}
        }
        catch (MsalException ex)
        {
            //Console.WriteLine($"Error acquiring access token: {ex}");
        }
        catch (Exception ex1)
        {
            //Console.WriteLine($"Error: {ex}");
        }

        if (System.Diagnostics.Debugger.IsAttached)
        {
            Console.WriteLine("Hit any key to exit...");
            Console.ReadKey();
        }
    }


}