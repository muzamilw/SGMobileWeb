using System;
using System.Web.Mvc;
using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.ViewModals.Customers;
using SG2.CORE.MODAL.DTO.Customers;
using System.Linq;
using klaviyo.net;
using System.Collections.Generic;
using SG2.CORE.MODAL.ViewModals.Home;
using Stripe;
using SG2.CORE.COMMON;
using static SG2.CORE.COMMON.GlobalEnums;
using SG2.CORE.MODAL.klaviyo;
using SG2.CORE.WEB.Architecture;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.DTO.Notification;
using System.Configuration;
using System.Threading.Tasks;

namespace SG2.CORE.WEB.Controllers
{
    public class AccountController : Controller
    {
        protected readonly CustomerManager _customerManager;
        protected readonly CommonManager _commonManager;
        protected readonly PlanInformationManager _planManager;
        protected readonly List<SystemSettingsDTO> SystemConfigs;
        protected readonly SessionManager _sessionManager;
        protected readonly NotificationManager _notManager;
        protected CustomerDTO CDT = null;

        //QueuePublisher<InstagramActionMessage> queue = new QueuePublisher<InstagramActionMessage>();
        public AccountController()
        {

            _customerManager = new CustomerManager();
            _commonManager = new CommonManager();
            _planManager = new PlanInformationManager();
            SystemConfigs = SystemConfig.GetConfigs;
            _sessionManager = new SessionManager();
            _notManager = new NotificationManager();
            CDT = (CustomerDTO)_sessionManager.Get(SessionConstants.Customer);

        }

        // GET: User
        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult ThankYou()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult SignUp(CustomerSignUpViewModel model)
        {
            var jr = new JsonResult();
            try
            {
                KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
                KlaviyoProfile klaviyoProfile = new KlaviyoProfile();

                if (ModelState.IsValid)
                {
                    string dbError = "";
                    var user = _customerManager.SignUpCustomer(new CustomerDTO()
                    {
                        CustomerId = model.CustomerId,
                        FirstName = model.FirstName,
                        SurName = model.UserName,
                        EmailAddress = model.EmailAddress,
                        StatusId = (int)CustomersStatus.Active,
                        GUID = Guid.NewGuid().ToString(),
                        LastLoginIP = HttpContext.Request.UserHostAddress,
                        Password = model.Password,
                        CreatedBy = model.FirstName
                    }, out dbError);

                    if (user != null)
                    {
                        jr.Data = new { ResultType = "Success", message = "User signup successfully. Please check you inbox to verify." };

                        Task.Run(() =>
                        {
                            var nt = new NotificationDTO()
                            {
                                Notification = NotificationMessages[(int)NotificationMessagesIndexes.NewSignup],
                                CreatedBy = model.EmailAddress,
                                CreatedOn = System.DateTime.Now,
                                Updatedby = model.EmailAddress,
                                UpdateOn = DateTime.Now,
                                SocialProfileId = user.SocialProfileId,
                                StatusId = (int)GeneralStatus.Unread
                            };
                            _notManager.AddNotification(nt);
                                                       
                            var encryptData = CryptoEngine.Encrypt(user.CustomerId + "#" + System.DateTime.Now.Date);
                            string URL = HttpContext.Request.Url.Scheme.ToString() + "://" + HttpContext.Request.Url.Authority.ToString() + "/Home/VerifyEmail?token=" + Url.Encode(encryptData);

                            //eventAPI();
                            List<NotRequiredProperty> list = new List<NotRequiredProperty>() {
                                new NotRequiredProperty("$email", model.EmailAddress),
                                new NotRequiredProperty("$first_name ", model.FirstName),
                                new NotRequiredProperty("$last_name ", model.SurName),
                                new NotRequiredProperty("URL", URL),
                                new NotRequiredProperty("RESEND", false),
                                new NotRequiredProperty("ISEMAILVERIFIED", false)
                            };
                            klaviyoProfile.email = model.EmailAddress;

                            var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                            var Klavio_NewSignups = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_NewSignups").ToLower()).ConfigValue;

                            klaviyoAPI.PeopleAPI(list, _klaviyoPublishKey);
                            var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_NewSignups);

                            //KlaviyoEvent ev = new KlaviyoEvent();

                            //ev.Event = "Email Verified";
                            //ev.Properties.NotRequiredProperties = list;
                            //ev.CustomerProperties.Email = CDT.EmailAddress;
                            //ev.CustomerProperties.FirstName = CDT.FirstName;
                            //ev.CustomerProperties.LastName = CDT.EmailAddress;

                            //klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);
                        });

                    }
                    else
                    {
                        jr.Data = new { ResultType = "Error", message = dbError };
                    }

                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    jr.Data = new { ResultType = "Error", message };
                }
                return jr;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return jr;
            }

        }

        [HttpGet]
        public ViewResult Login()
        {
            return View(new CustomerLoginViewModel());
        }

        public ActionResult Logout()
        {
            _sessionManager.Clear();
            HttpContext.Items["isAuthentication"] = false;
            HttpContext.Items["isAuthentication"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(CustomerLoginViewModel model)
        {
            try
            {
                var jr = new JsonResult();
                if (ModelState.IsValid)
                {
                    string errorMesage = "";
                    var resp = _customerManager.LoginUser(model.EmailAddress, model.Password, ref errorMesage);

                    if (resp.Item1)
                    {
                        var usr = (CustomerDTO)_sessionManager.Get(SessionConstants.Customer);
                        HttpContext.Items["isAuthentication"] = true;
                        jr.Data = new { ResultType = "Success", message = "User successfully autheticated.", ResultData = usr.DefaultSocialProfileId };
                    }
                    else
                    {
                        jr.Data = new { ResultType = "Error", message = errorMesage };
                    }
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Please enter valid email or password." };
                }
                return jr;
            }
            catch (Exception ex)
            {
                var jr = new JsonResult();
                jr.Data = new { ResultType = "Error", message = "Unable to login" };
                return jr;
            }
        }

        public JsonResult IsEmailExist(string EmailAddress, int id = 0)
        {
            try
            {
                return _customerManager.IsEmailExist(EmailAddress, id) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SignupWizard(SignupWizardViewModel model)
        {
            KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
            KlaviyoProfile klaviyoProfile = new KlaviyoProfile();
            var jr = new JsonResult();
            if (ModelState.IsValid)
            {
                CustomerAndPreferenceDTO customerAndPreference = new CustomerAndPreferenceDTO();
                customerAndPreference.Preference1 = model.Preference1;
                customerAndPreference.Preference2 = model.Preference2;
                customerAndPreference.Preference3 = model.Preference3;
                // customerAndPreference.Preference4 = model.Preference5
                customerAndPreference.Preference5 = model.Preference5;
                customerAndPreference.Preference6 = model.Preference6;
                //customerAndPreference.InstaUser = model.InstaUser;
                //customerAndPreference.InstaPassword = model.InstaPassword;
                customerAndPreference.PreferLocation = model.CityId;
                customerAndPreference.StatusId = 1;
                customerAndPreference.PhoneNumber = model.PhoneNumber;
                customerAndPreference.Password = model.Password;
                customerAndPreference.Lastname = model.Lastname;
                customerAndPreference.FirstName = model.FirstName;
                customerAndPreference.GUID = Guid.NewGuid().ToString();
                customerAndPreference.LastLoginIP = HttpContext.Request.UserHostAddress;
                customerAndPreference.EmailAddress = model.EmailAddress;

                var user = _customerManager.SignupCustomerProfileAndPreference(customerAndPreference);

                if (user != null)
                {
                    //var encryptData = CryptoEngine.Encrypt(user.CustomerId + "#" + System.DateTime.Now.Date);
                    //string URL = HttpContext.Request.Url.Scheme.ToString() + "://" + HttpContext.Request.Url.Authority.ToString() + "/Home/VerifyEmail?token=" + Url.Encode(encryptData);

                    ////eventAPI();
                    //var list = new List<NotRequiredProperty>
                    //{
                    //    new NotRequiredProperty("$email", model.EmailAddress),
                    //    new NotRequiredProperty("$first_name ", model.FirstName),
                    //    new NotRequiredProperty("$last_name ", model.FirstName),
                    //    new NotRequiredProperty("URL", URL),
                    //    new NotRequiredProperty("RESEND", false)
                    //};
                    //klaviyoProfile.email = model.EmailAddress;
                    //klaviyoAPI.PeopleAPI(list);
                    //var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile);

                    HttpContext.Items["isAuthentication"] = true;
                    jr.Data = new { ResultType = "Success", message = "User signup successfully. Please check you inbox to verify.", data = user };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Oops! Please contact administrator." };
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                jr.Data = new { ResultType = "Error", message };
            }
            return jr;

        }


        //[HttpPost]
        //public ActionResult SubscriptionWizard(SignupWizardViewModel model)
        //{
        //    bool anyError = true;
        //    var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
        //    StripeConfiguration.SetApiKey(_stripeApiKey);
        //    var stripeDefaultPlan = _planManager.GetAllSocialGrowthPlans().ToList().FirstOrDefault(x => x.PlantypeName == "Likey" && (x.IsDefault != null && x.IsDefault == true));
        //    var _googleApiKey = SystemConfigs.First(x => x.ConfigKey == "GoogleMapApiKey").ConfigValue;

        //    var jr = new JsonResult();
        //    try
        //    {
        //        //if (ModelState.IsValid)
        //        //{


        //        //-- Create Customer into stripe
        //        var customerCreateOptions = new CustomerCreateOptions
        //        {
        //            Description = "Customer for SocialPlannerPro" + this.CDT.EmailAddress,
        //            //SourceToken = model.stripeToken,
        //            Name = this.CDT.FirstName + " " + this.CDT.SurName,
        //            Email = this.CDT.EmailAddress,
        //        };
        //        var customerService = new CustomerService();
        //        Customer stripeCustomer = customerService.Create(customerCreateOptions);

        //        //-- Save StripeCustomerId int our database getting from stripe service 
        //        _customerManager.UpdateSocialProfileStripeCustomer(this.CDT.CustomerId, stripeCustomer.Id);
        //        this.CDT.StripeCustomerId = stripeCustomer.Id;
        //        _sessionManager.Set(SessionConstants.Customer, this.CDT);

        //        //Create Subscription into stripe.
        //        var stripeItems = new List<SubscriptionItemOption> {
        //              new SubscriptionItemOption {
        //                Plan = stripeDefaultPlan.StripePlanId,
        //                Quantity= 1
        //              }
        //            };
        //        var stripeSubscriptionCreateOptions = new SubscriptionCreateOptions
        //        {
        //            Customer = stripeCustomer.Id,
        //            Items = stripeItems,
        //            //Billing = Billing.ChargeAutomatically,
        //            BillingThresholds = { }
        //        };
        //        var stripeSubscriptionService = new SubscriptionService();
        //        Subscription stripeSubscription = stripeSubscriptionService.Create(stripeSubscriptionCreateOptions);

        //        // Subscription successfull save into our DB
        //        if (stripeSubscription != null)
        //        {
        //            this.CDT.StripePlanId = stripeDefaultPlan.StripePlanId;
        //            _sessionManager.Set(SessionConstants.Customer, this.CDT);

        //            SocialProfile_PaymentsDTO subDTO = new SocialProfile_PaymentsDTO();
        //            //subDTO.CustomerId = this.CDT.CustomerId;
        //            subDTO.StripeSubscriptionId = stripeSubscription.Id;
        //            subDTO.Description = stripeSubscription.Plan.Nickname;
        //            subDTO.Name = stripeSubscription.Plan.Nickname;
        //            subDTO.Price = stripeSubscription.Plan.Amount;
        //            subDTO.StripePlanId = stripeDefaultPlan.StripePlanId;
        //            subDTO.SubscriptionType = stripeSubscription.Plan.Interval;
        //            subDTO.StartDate = stripeSubscription.StartDate ?? DateTime.Now;
        //            subDTO.EndDate = ((DateTime)stripeSubscription.StartDate).AddMonths(1);
        //            subDTO.StatusId = (int)GlobalEnums.PlanSubscription.Active;
        //            subDTO.PaymentPlanId = stripeDefaultPlan.PlanId;
        //            subDTO.SocialProfileId = this.CDT.SocialProfileId;

        //            _customerManager.InsertSubscription(subDTO);

        //            ////_customerManager.AssignJVBoxToCustomer(this.CDT.CustomerId, this.CDT.SocialProfileId);
        //            ////var cityId = _customerManager.GetTargetedCityIdByCustomerId(this.CDT.CustomerId, this.CDT.SocialProfileId);
        //            ////if (cityId > 0)
        //            ////{
        //            ////    var city = CommonManager.GetCityAndCountryData(cityId).FirstOrDefault();
        //            ////    if (city != null)
        //            ////    {
        //            ////        _commonManager.AssignedNearestProxyIP(this.CDT.CustomerId, city.CountyCityName.Replace(",", ""), this.CDT.SocialProfileId, _googleApiKey);
        //            ////    }
        //            ////}
        //            anyError = false;
        //        }

        //        if (!anyError)
        //        {
        //            jr.Data = new { ResultType = "Success", message = "User signup successfully. Please check you inbox to verify.", data = true };
        //        }
        //        else
        //        {
        //            jr.Data = new { ResultType = "Error", message = "Error! Please contact administrator." };
        //        }

        //        //}
        //        //else
        //        //{
        //        //    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //        //    jr.Data = new { ResultType = "Error", message = message };
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return jr;
        //}

        [HttpPost]
        public ActionResult ForgetPassword(string username)
        {


            KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
            KlaviyoProfile klaviyoProfile = new KlaviyoProfile();
            KlaviyoEvent ev = new KlaviyoEvent();

            var jr = new JsonResult();

            var customer = _customerManager.GetCustomerByEmail(username);

            if (customer != null)
            {

                var encryptData = CryptoEngine.Encrypt(customer.CustomerId + "#" + System.DateTime.Now.Date);
                string URL = HttpContext.Request.Url.Scheme.ToString() + "://" + HttpContext.Request.Url.Authority.ToString() + "/Home/ResetPassword?token=" + Url.Encode(encryptData);

                List<NotRequiredProperty> list = new List<NotRequiredProperty>
                    {
                        new NotRequiredProperty("$email", customer.EmailAddress),
                        new NotRequiredProperty("$first_name ", customer.FirstName),
                        new NotRequiredProperty("$last_name ", customer.SurName),
                        new NotRequiredProperty("RESEND", false),
                        new NotRequiredProperty("ISEMAILVERIFIED", true),
                        new NotRequiredProperty("ResetPasswordURL", URL)
                    };
                ev.Event = "Forget Password";
                ev.Properties.NotRequiredProperties = list;
                ev.CustomerProperties.Email = customer.EmailAddress;
                ev.CustomerProperties.FirstName = customer.FirstName;
                ev.CustomerProperties.LastName = customer.SurName;

                var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                // update  Profile
                klaviyoAPI.PeopleAPI(list, _klaviyoPublishKey);
                klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);


                //HttpContext.Items["isAuthentication"] = true;
                jr.Data = new { ResultType = "Success", message = "Email with password reset instructions sent successfully to the email address. Please check your inbox.", data = customer };
            }
            else
            {
                jr.Data = new { ResultType = "Error", message = "Invalid Email Address" };
            }

            return jr;

        }

    }
}