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

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
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
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    //handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    //handlePaymentMethodAttached(paymentMethod);
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