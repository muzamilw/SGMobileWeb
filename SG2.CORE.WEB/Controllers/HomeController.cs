using klaviyo.net;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.klaviyo;
using SG2.CORE.MODAL.ViewModals.Customers;
using SG2.CORE.MODAL.ViewModals.Home;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SG2.CORE.WEB.Architecture;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using SG2.CORE.MODAL.DTO.QueueLogger;
using SG2.CORE.MODAL.DTO.SystemSettings;
using System.Globalization;
using SG2.CORE.MODAL.DTO.Customers;

namespace SG2.CORE.WEB.Controllers
{
    public class HomeController : BaseController
    {
        protected readonly CustomerManager _customerManager = new CustomerManager();
        protected readonly List<SystemSettingsDTO> SystemConfigs;
        //IRemoteProcedureClient rpcClient = null;

        public HomeController()
        {
            //rpcClient = new RemoteProcedureClient();
            _customerManager = new CustomerManager();
            SystemConfigs = SystemConfig.GetConfigs;
        }

        public ActionResult Index()
        {
            var indexVM = new IndexViewModel();
            indexVM.SignupWizardViewModel = new SignupWizardViewModel();
            //indexVM.SignupWizardViewModel.Cities = CommonManager.GetCityAndCountryData(null);
            indexVM.StripeApikey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            //var connectionString = ConfigurationManager.AppSettings["RabbitMQ:ConnectionString"];
            //var hostNames = ConfigurationManager.AppSettings["RabbitMQ:HostNames"].Split(',');
            //ConfigurationManager.AppSettings["RabbitMQ:Exchange"];

            string rpcSessionId = Guid.NewGuid().ToString();
            return View("Index");
        }

        public ActionResult HowWeWork()
        {
            ViewBag.CustomerDTO = new CustomerDTO();
            return View();
        }
        public ActionResult Pricing()
        {
            ViewBag.CustomerDTO = new CustomerDTO();
            return View();
        }
        public ActionResult HowWeWork2()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "SocialPlannerPro";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult VerifyEmail(string token)
        {

            KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
            KlaviyoProfile klaviyoProfile = new KlaviyoProfile();
            ViewBag.Title = "Email Verification";
            ResendEmailViewModel model = new ResendEmailViewModel();
            try
            {
                if (token != null)
                {
                    var decryptData = CryptoEngine.Decrypt(token);
                    var splitdata = decryptData.Split('#');
                    int customerId = Convert.ToInt32(splitdata[0]);
                    short statusId = 1;
                    DateTime sessionDateTime = Convert.ToDateTime(splitdata[1]);
                    DateTime currentDate = DateTime.Now;
                    _customerManager.ActivateCustomerPassword(statusId, customerId);
                    model.CustomerId = customerId;
                    var totalHours = (currentDate - sessionDateTime).TotalHours;
                    if (totalHours <= 24)
                    {
                        var customer = _customerManager.GetCustomerDTOByCustomerId(customerId);

                        if (customer != null)
                        {

                            KlaviyoEvent ev = new KlaviyoEvent();

                            List<NotRequiredProperty> list = new List<NotRequiredProperty>
                            {
                                new NotRequiredProperty("$email", customer.EmailAddress),
                                new NotRequiredProperty("$first_name ", customer.FirstName),
                                new NotRequiredProperty("$last_name ", customer.SurName),
                                new NotRequiredProperty("RESEND", false),
                                new NotRequiredProperty("ISEMAILVERIFIED", true)
                            };
                            ev.Event = "Email Verified";
                            ev.Properties.NotRequiredProperties = list;
                            ev.CustomerProperties.Email = customer.EmailAddress;
                            ev.CustomerProperties.FirstName = customer.FirstName;
                            ev.CustomerProperties.LastName = customer.SurName;

                            klaviyoProfile.email = customer.EmailAddress;

                            var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                            var Klavio_NewSignups = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_NewSignups").ToLower()).ConfigValue;
                            var Klavio_FreeCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_FreeCustomers").ToLower()).ConfigValue;
                            
                            klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);

                            klaviyoAPI.Klaviyo_DeleteFromList(customer.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_NewSignups);
                            var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_FreeCustomers);


                            model.CustomerId = customer.CustomerId;
                            ViewBag.CustomerDTO = customer;
                        }
                        return View(model);

                    }
                    else
                    {

                        return RedirectToAction("ResendVerifyEmail", "Home", model);

                    }


                }
                else
                {

                    return RedirectToAction("index", "Home");

                }

            }
            catch (Exception exp)
            {
                throw exp;

            }

        }

        public ActionResult ResendVerifyEmail(ResendEmailViewModel model)
        {

            try
            {
                //  return RedirectToAction("Home", "VPSSupplier");

            }
            catch (Exception exp)
            {
                throw exp;
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult ResendEmail(ResendEmailViewModel model)
        {
            try
            {
                KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
                KlaviyoProfile klaviyoProfile = new KlaviyoProfile();
                KlaviyoEvent ev = new KlaviyoEvent();

                int customerId = model.CustomerId; // Convert.ToInt32(CryptoEngine.Decrypt(model.));
                var customer = _customerManager.GetCustomerDTOByCustomerId(customerId);
                var encryptData = CryptoEngine.Encrypt(customerId + "#" + System.DateTime.Now.Date);
                string URL = HttpContext.Request.Url.Scheme.ToString() + "://" + HttpContext.Request.Url.Authority.ToString() + "/Home/VerifyEmail?token=" + Url.Encode(encryptData);
                List<NotRequiredProperty> list = new List<NotRequiredProperty>() {
                        new NotRequiredProperty("$email", customer.EmailAddress),
                        new NotRequiredProperty("$first_name ", customer.FirstName),
                        new NotRequiredProperty("$last_name ", customer.SurName),
                        new NotRequiredProperty("RESEND", false),
                        new NotRequiredProperty("ISEMAILVERIFIED", true),
                        new NotRequiredProperty("URL", URL)
                    };

                var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;

                klaviyoAPI.PeopleAPI(list, _klaviyoPublishKey);
                ev.Event = "Resend Varification Email";
                ev.Properties.NotRequiredProperties = list;
                ev.CustomerProperties.Email = customer.EmailAddress;
                ev.CustomerProperties.FirstName = customer.FirstName;
                ev.CustomerProperties.LastName = customer.SurName;
                klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);

                TempData["Success"] = "Yes";
                TempData["Message"] = "Email sent successfully.";

                return RedirectToAction("VerifyEmail", "Home");
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public ActionResult ResetPassword(string token)
        {


            ResetUserPasswordViewModel model = new ResetUserPasswordViewModel();
            try
            {
                if (token != null)
                {
                    var decryptData = CryptoEngine.Decrypt(token);
                    var splitdata = decryptData.Split('#');
                    if (string.IsNullOrWhiteSpace(splitdata[0]))
                    {
                        ViewBag.Success = "Error";
                        ViewBag.Message = "Incorrect Token, Customer information missing";

                        return View(model);
                    }

                    int customerId = Convert.ToInt32(splitdata[0]);



                    //DateTime sessionDateTime = DateTime.ParseExact(splitdata[1], "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture); // Convert.ToDateTime(splitdata[1]);
                    //DateTime currentDate = DateTime.Now;
                    //var totalHours = (currentDate - sessionDateTime).TotalHours;
                    //if (totalHours <= 2)
                    //{

                    var customer = _customerManager.GetCustomerDTOByCustomerId(customerId);
                    if (customer == null)
                    {
                        ViewBag.Success = "Error";
                        ViewBag.Message = "Customer not found";

                        return View(model);
                    }
                    ViewBag.CustomerDTO = customer;
                    model.CustomerId = customer.CustomerId;

                    //    if (!string.IsNullOrEmpty((string)TempData["Success"]))
                    //    {
                    ViewBag.Success = (string)TempData["Success"];
                    ViewBag.Message = TempData["Message"];
                    //    }
                    //    // model.Password = customer;
                    //}

                }
            }
            catch (Exception exp)
            {
                throw exp;
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateUserPassword(ResetUserPasswordViewModel model)
        {
            try
            {
                var jr = new JsonResult();
                if (ModelState.IsValid)
                {
                    if (_customerManager.UpdateCustomerPassword(model.Password, model.CustomerId))
                    {
                        //this.CDT.Password = model.Password;
                        jr.Data = new { ResultType = "Success", message = "Password updated successfully." };
                    }
                    //return RedirectToAction("PasswordUpdated", "Home");
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    jr.Data = new { ResultType = "Error", message = messages };

                }
                return jr;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult PasswordUpdated()
        {
            try
            {
                //  return RedirectToAction("Home", "VPSSupplier");

            }
            catch (Exception exp)
            {
                throw exp;
            }


            return View();
        }

        public string ScreenShot()
        {
            string SigBase64;
            System.Drawing.Rectangle bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
                }

                System.IO.MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                SigBase64 = Convert.ToBase64String(byteImage);
            }
            //return SigBase64;

            return SigBase64;

        }

        public Image LoadImage(string Base64Image)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            byte[] bytes = Convert.FromBase64String(Base64Image);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                image.Save("C://test1.png", ImageFormat.Png);
            }

            return image;
        }

        public ActionResult TestAPi()
        {
            //InsertAuditDetail(new QueueAuditDetailDTO
            //{
            //    CreatedBy = "JV Bot",
            //    CreatedDate = DateTime.Now,
            //    StepName = "Session Created",
            //    TransactionId ="1235",
            //    StepStatus = (int)GlobalEnums.QueueStatus.Completed,
            //    StepDetail = "Session created successfully.",
            //    StepError = null
            //});

            string jsonResponse = string.Empty;
            bool result;
            string endpoint = string.Format(
                "{0}api/QueueLogger/UpdateQueueAuditStatus/{1}/{2}",
                "http://tooezee.com/",
                "e2c0bc2d-2b50-4568-91d8-2d9cbd9e",
                "44"
                );

            HttpWebRequest request = GetWebRequestObject(endpoint);

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            result = JsonConvert.DeserializeObject<bool>(new StreamReader(stream).ReadToEnd());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new EmptyResult();
        }

        public static void InsertAuditDetail(QueueAuditDetailDTO queueAuditDetail)
        {
            string json = JsonConvert.SerializeObject(
                queueAuditDetail,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });

            string jsonResponse = string.Empty;
            string endpoint = string.Format(
                "{0}",
                "https://localhost:44361/api/QueueLogger/InsertQueueAuditDetail"
               );

            bool result;
            HttpWebResponse httpResponse;
            try
            {
                HttpWebRequest request = GetWebRequestObject(endpoint);
                request.ContentType = "application/json";
                request.Method = "POST";
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byte1 = encoding.GetBytes(json);
                request.ContentLength = byte1.Length;

                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                using (httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        result = JsonConvert.DeserializeObject<bool>(new StreamReader(stream).ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetTargetPreferenceQueueStatus(int profileId, short queueStatus, string updateBy)
        {
            string jsonResponse = string.Empty;
            bool result;
            string endpoint = string.Format(
                "{0}api/QueueStatus/SetTargetPreferenceQueueStatus?profileId={1}&queueStatus={2}&updateBy={3}",
                "https://localhost:44361/",
                profileId,
                queueStatus,
                updateBy
                );

            HttpWebRequest request = GetWebRequestObject(endpoint);

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            result = JsonConvert.DeserializeObject<bool>(new StreamReader(stream).ReadToEnd());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult Agency()
        {
            return View();
        }
        private static HttpWebRequest GetWebRequestObject(string endPoint)
        {
            HttpWebRequest request = WebRequest.Create(endPoint) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = false;
            request.Timeout = 15 * 60000;
            return request;
        }

    }
}