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


                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("info@socialplannerpro.com", "Social Planner Pro Team");

                string templateId = "";

                if (emailType == EmailType.EmailVerify)
                    templateId = "d-259189b77ced40279e9ba28011714ff2";
                else if (emailType == EmailType.ForgotPassword)
                    templateId = "d-df269fcb384a426cafbfaecac7b7e775";
                else if (emailType == EmailType.UpgradeSubscription)
                    templateId = "";

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
            Invoice = 3,
            UpgradeSubscription = 4
        }
    }
}
