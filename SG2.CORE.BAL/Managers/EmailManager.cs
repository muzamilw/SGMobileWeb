using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SG2.CORE.BAL.Managers
{
    

    public class EmailManager
    {
        /*private static void Main()
        {
            Execute().Wait();
        }*/

        public static async Task SendEmail(string toEmail, string toEmailName,EmailType emailType, Dictionary<string, string> dynamicTemplateData)
        {
            try
            {
                //

                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("info@socialplannerpro.com", "Social Planner Pro Team");

                string templateId = "";

                if (emailType == EmailType.EmailVerify)
                    templateId = "d-259189b77ced40279e9ba28011714ff2";
                else if (emailType == EmailType.ForgotPassword)
                    templateId = "d-df269fcb384a426cafbfaecac7b7e775";
                else if (emailType == EmailType.Welcome)
                    templateId = "d-6f3d377bdcaf4c548e9298b01268edf1";
                else if (emailType == EmailType.PlanUpgrade)
                    templateId = "d-877a1d5ca7aa43eb80fb284cf2a1681a";
                else if (emailType == EmailType.PlanDowngrade)
                    templateId = "d-1da290de356a45118ed0ee53819545b7";
                else if (emailType == EmailType.RunNotification)
                    templateId = "d-d9d531c7d6454dd18c5b4ae411b95150";



                var to = new EmailAddress(toEmail, toEmailName);
                var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);
                var response = await client.SendEmailAsync(msg);
                Console.WriteLine(response.Body);

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public enum EmailType
        {
            EmailVerify =1,
            ForgotPassword = 2,
            PlanUpgrade = 3,
            PlanDowngrade = 4,
            Welcome = 5,
            RunNotification = 6,
            HomePageContact = 6
        }
    }
}
