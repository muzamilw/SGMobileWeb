using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static async Task SendEmail(string toEmail, string toEmailName,EmailType emailType)
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
            var dynamicTemplateData = new Dictionary<string, string>
            {
                {"first_name", "John"},
                {"last_name", "Snow"},
            };

            var to = new EmailAddress(toEmail, toEmailName);
            /*var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);*/

           var msg =  MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);
           var response = await client.SendEmailAsync(msg);
           Console.WriteLine(response.Body);

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
