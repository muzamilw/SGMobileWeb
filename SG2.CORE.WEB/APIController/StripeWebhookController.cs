using klaviyo.net;
using PagedList;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.DTO.TeamMember;
using SG2.CORE.MODAL.klaviyo;
using SG2.CORE.MODAL.ViewModals.Backend.TeamMember;
using SG2.CORE.WEB.Architecture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using Stripe;
using System.Web.Http;
using System.Web;
using SG2.CORE.MODAL.DTO.Customers;

namespace SG2.CORE.WEB.APIController
{
    [RoutePrefix("api/stripe")]
    public class StripeWebHookController : ApiController
    {
        protected readonly TeamMemberManager _teamMemberManager;
        private readonly string _PageSize = string.Empty;
        protected readonly CustomerManager _customerManager;
        protected readonly List<SystemSettingsDTO> SystemConfigs;

        public StripeWebHookController()
        {
            _customerManager = new CustomerManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            _teamMemberManager = new TeamMemberManager();
            SystemConfigs = SystemConfig.GetConfigs;
          
        }

        [AllowAnonymous]
        [Route("Hook")]
        [HttpPost]
        public IHttpActionResult Hook()
        {
            Stream req = HttpContext.Current.Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);

            var json = new StreamReader(req).ReadToEnd();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                // Handle the event
                if (stripeEvent.Type == Events.InvoicePaymentSucceeded)
                {
                    var invoice = stripeEvent.Data.Object as Invoice;

                    var profile = _customerManager.GetSocialProfileByStripeSubscriptionId(invoice.SubscriptionId);
                    if ( profile != null)
                    {
                        SocialProfile_PaymentsDTO paymentRec = new SocialProfile_PaymentsDTO();
                        paymentRec.SocialProfileId = profile.SocialProfileId;
                        paymentRec.StripeSubscriptionId = invoice.SubscriptionId;
                        paymentRec.Description = invoice.Subscription.Plan.Nickname;
                        paymentRec.Name = invoice.Subscription.Plan.Nickname;
                        paymentRec.Price = invoice.Subscription.Plan.Amount / 100;
                        //-- subDTO.Price = stripeSubscription.Plan.Amount;
                        paymentRec.StripePlanId = invoice.Subscription.Plan.Id;
                        paymentRec.SubscriptionType = invoice.Subscription.Plan.Interval;

                        paymentRec.StartDate = invoice.Subscription.CurrentPeriodStart.Value;
                        paymentRec.EndDate = invoice.Subscription.CurrentPeriodEnd.Value;
                        paymentRec.StatusId = (int)GlobalEnums.PlanSubscription.Active;
                        paymentRec.PaymentPlanId = profile.PaymentPlanId;
                        paymentRec.StripeInvoiceId = invoice.Id;
                        paymentRec.PaymentDateTime = DateTime.Now;

                        _customerManager.InsertSocialProfilePayment(paymentRec);
                    }
                    
                    
                    //handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == Events.InvoicePaymentFailed)
                {
                    var invoice = stripeEvent.Data.Object as Invoice;

                    var profile = _customerManager.GetSocialProfileByStripeSubscriptionId(invoice.SubscriptionId);
                    if (profile != null)
                    {
                        SocialProfile_PaymentsDTO paymentRec = new SocialProfile_PaymentsDTO();
                        paymentRec.SocialProfileId = profile.SocialProfileId;
                        paymentRec.StripeSubscriptionId = invoice.SubscriptionId;
                        paymentRec.Description = invoice.Subscription.Plan.Nickname;
                        paymentRec.Name = invoice.Subscription.Plan.Nickname;
                        paymentRec.Price = invoice.Subscription.Plan.Amount / 100;
                        //-- subDTO.Price = stripeSubscription.Plan.Amount;
                        paymentRec.StripePlanId = invoice.Subscription.Plan.Id;
                        paymentRec.SubscriptionType = invoice.Subscription.Plan.Interval;

                        paymentRec.StartDate = invoice.Subscription.CurrentPeriodStart.Value;
                        paymentRec.EndDate = invoice.Subscription.CurrentPeriodEnd.Value;
                        paymentRec.StatusId = (int)GlobalEnums.PlanSubscription.canceled;
                        paymentRec.PaymentPlanId = profile.PaymentPlanId;
                        paymentRec.StripeInvoiceId = invoice.Id;
                        paymentRec.PaymentDateTime = DateTime.Now;

                        _customerManager.InsertSocialProfilePayment(paymentRec);

                        _customerManager.UpdateSocialProfileStripeCustomer(profile.SocialProfileId, invoice.CustomerId, invoice.SubscriptionId, 1, GlobalEnums.PlanSubscription.Active);
                    }
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    var sub = stripeEvent.Data.Object as Subscription;

                    var profile = _customerManager.GetSocialProfileByStripeSubscriptionId(sub.Id);
                    if (profile != null)
                    {
                        SocialProfile_PaymentsDTO paymentRec = new SocialProfile_PaymentsDTO();
                        paymentRec.SocialProfileId = profile.SocialProfileId;
                        paymentRec.StripeSubscriptionId = sub.Id;
                        paymentRec.Description = sub.Plan.Nickname;
                        paymentRec.Name = sub.Plan.Nickname;
                        paymentRec.Price = sub.Plan.Amount / 100;
                        //-- subDTO.Price = stripeSubscription.Plan.Amount;
                        paymentRec.StripePlanId = sub.Plan.Id;
                        paymentRec.SubscriptionType = sub.Plan.Interval;

                        paymentRec.StartDate = sub.CurrentPeriodStart.Value;
                        paymentRec.EndDate = sub.CurrentPeriodEnd.Value;
                        paymentRec.StatusId = (int)GlobalEnums.PlanSubscription.canceled;
                        paymentRec.PaymentPlanId = profile.PaymentPlanId;
                        paymentRec.StripeInvoiceId = "";
                        paymentRec.PaymentDateTime = DateTime.Now;

                        _customerManager.InsertSocialProfilePayment(paymentRec);

                        _customerManager.UpdateSocialProfileStripeCustomer(profile.SocialProfileId, sub.CustomerId, sub.Id, 1, GlobalEnums.PlanSubscription.Active);
                    }
                }
                else if (stripeEvent.Type == Events.InvoicePaymentActionRequired)
                {
                    var invoice = stripeEvent.Data.Object as Invoice;

                    var profile = _customerManager.GetSocialProfileByStripeSubscriptionId(invoice.SubscriptionId);
                    if (profile != null)
                    {
                        SocialProfile_PaymentsDTO paymentRec = new SocialProfile_PaymentsDTO();
                        paymentRec.SocialProfileId = profile.SocialProfileId;
                        paymentRec.StripeSubscriptionId = invoice.SubscriptionId;
                        paymentRec.Description = invoice.Subscription.Plan.Nickname;
                        paymentRec.Name = invoice.Subscription.Plan.Nickname;
                        paymentRec.Price = invoice.Subscription.Plan.Amount / 100;
                        //-- subDTO.Price = stripeSubscription.Plan.Amount;
                        paymentRec.StripePlanId = invoice.Subscription.Plan.Id;
                        paymentRec.SubscriptionType = invoice.Subscription.Plan.Interval;

                        paymentRec.StartDate = invoice.Subscription.CurrentPeriodStart.Value;
                        paymentRec.EndDate = invoice.Subscription.CurrentPeriodEnd.Value;
                        paymentRec.StatusId = (int)GlobalEnums.PlanSubscription.canceled;
                        paymentRec.PaymentPlanId = profile.PaymentPlanId;
                        paymentRec.StripeInvoiceId = invoice.Id;
                        paymentRec.PaymentDateTime = DateTime.Now;

                        _customerManager.InsertSocialProfilePayment(paymentRec);

                        _customerManager.UpdateSocialProfileStripeCustomer(profile.SocialProfileId, invoice.CustomerId, invoice.SubscriptionId, 1, GlobalEnums.PlanSubscription.Active);
                    }
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    return BadRequest();
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}