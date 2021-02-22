using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
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
                var from = new EmailAddress("info@connectwizz.net", "ConnectWizz Team");

                string templateId = "";

                if (emailType == EmailType.EmailVerify)
                {
                    templateId = "d-2c7af748c62645789007cd1aefeeb36c";
                }
                else if (emailType == EmailType.ForgotPassword)
                {
                    templateId = "d-c622d898a3d647db89f0d9b02c13c342";
                }
                else if (emailType == EmailType.Welcome)
                {
                    templateId = "d-7d37b554eacb4827a4aca95b3bcb96da";
                }
                else if (emailType == EmailType.PlanUpgrade)
                {
                    templateId = "d-c504a226dd3946fb99ea824d958c4b46";
                }
                else if (emailType == EmailType.PlanDowngrade)
                {
                    templateId = "d-1da290de356a45118ed0ee53819545b7";
                }
                else if (emailType == EmailType.RunNotification)
                {
                    templateId = "d-acb7d12c54c848e18d9912d9435183ae";
                }
                else if (emailType == EmailType.HomePageContact)
                {
                    templateId = "d-de0e38e3b1ca45088cf2c1a6f245b41f";
                }
                else if (emailType == EmailType.profileDeleted)
                {
                    templateId = "d-344696b1415e4ec59f24d56b35102549";
                }
                else if (emailType == EmailType.newSignup)
                {
                    templateId = "d-b0a00fdba5454c0db059c515a5a407a3";
                }
                else if (emailType == EmailType.CreditCardEntered)
                {
                    templateId = "d-b4098dda5f7c460a90af066f07921f67";
                }


                EmailAddress to = null;

                
                SendGridMessage msg = null;
                if (emailType == EmailType.error)
                {
                    to = new EmailAddress(toEmail, toEmailName);
                    msg=  MailHelper.CreateSingleEmail(from, to, "CW Error", dynamicTemplateData.ToString(), dynamicTemplateData.ToString());
                }
                else if (emailType == EmailType.info)
                {
                    to = new EmailAddress(toEmail, toEmailName);
                    msg = MailHelper.CreateSingleEmail(from, to, "CW information", dynamicTemplateData.ToString(), dynamicTemplateData.ToString());
                }
                else if (emailType == EmailType.newSignup || emailType == EmailType.CreditCardEntered)
                {
                    var slist = toEmail.Split(',');
                    var tolist = new List<EmailAddress>();
                    foreach (var item in slist)
                    {
                        tolist.Add(new EmailAddress(item, "CW Team"));
                    }

                    
                    msg = MailHelper.CreateSingleTemplateEmailToMultipleRecipients(from, tolist , templateId, dynamicTemplateData);
                }
                else
                {
                    to = new EmailAddress(toEmail, toEmailName);
                    msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);
                }
                var response = await client.SendEmailAsync(msg);
                Console.WriteLine(response.Body);

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public static async Task SendEmailsmtp(string toEmail, string toEmailName, EmailType emailType, Dictionary<string, string> dynamicTemplateData)
        {
            try
            {
                //

               

             
                var from = new MailAddress("info@connectwizz.net", "ConnectWizz Team");
                string subject = "";
                string mailBody = "";

                if (emailType == EmailType.EmailVerify)
                {
                    mailBody = File.ReadAllText(HostingEnvironment.MapPath(@"~/emails/verify your email.html"));
                    subject = "SocialPlanner Pro - Verify your account";
                }
                else if (emailType == EmailType.ForgotPassword)
                {
                    mailBody = File.ReadAllText(HostingEnvironment.MapPath(@"~/emails/verify your email.html"));
                    subject = "Password Reset - SocialPlanner Pro";
                }
                else if (emailType == EmailType.Welcome)
                {
                    mailBody = File.ReadAllText(HostingEnvironment.MapPath(@"~/emails/verify your email.html"));
                    subject = "Congratulations !! Your account is active and ready to get started";
                }
                else if (emailType == EmailType.PlanUpgrade)
                {
                    mailBody = File.ReadAllText(HostingEnvironment.MapPath(@"~/emails/verify your email.html"));
                    subject = "Plan Upgraded, Tax Invoice Enclosed";
                }
                else if (emailType == EmailType.PlanDowngrade)
                {
                    mailBody = File.ReadAllText(HostingEnvironment.MapPath(@"~/emails/verify your email.html"));
                }
                else if (emailType == EmailType.RunNotification)
                {
                    mailBody = File.ReadAllText(HostingEnvironment.MapPath(@"~/emails/newsession.html"));
                    subject = "Your Growth Session will start in 2 hours";
                }

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = from;
                mailMessage.To.Add(new MailAddress(toEmail, toEmailName));
                mailMessage.Body = mailBody;
                mailMessage.Subject = "subject";

                
                using (var smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = false;
                    var credential = new NetworkCredential
                    {
                        UserName = "info@connectwizz.net",  // replace with valid value
                        Password = "Sh@rp2020"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "mail.connectwizz.net";
                    smtp.Port = 2525;
                    smtp.EnableSsl = false;
                    await smtp.SendMailAsync(mailMessage);
                    
                }


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
            HomePageContact = 7,
            profileDeleted = 8,
            error = 9,
            info = 10,
            newSignup= 11,
            CreditCardEntered = 12,


        }
    }
}
