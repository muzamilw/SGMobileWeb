using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.ViewModals.Backend.ActionBoard;
using SG2.CORE.WEB.App_Start;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class ActionBoardController : BaseController
    {
        protected readonly CustomerManager _customerManager;
        //protected readonly ProxyManager _proxyManager;

        public ActionBoardController()
        {
            _customerManager = new CustomerManager();
            //_proxyManager = new ProxyManager();
            ViewBag.SetMenuActiveClass = "ActionBoard";
        }

        // GET: SuperAdmin/ActionBoard
        public ActionResult Index()
        {
            //var JVStatusesData = _customerManager.GetJVStatuses();
            //var countries = CommonManager.GetCountries();
            var cities = CommonManager.GetCities();
            var model = new ActionBoardViewModel()
            {
                //JVStatuses = JVStatusesData,
                //MPBoxList = _customerManager.GetMPBoxes(),
                //Countries = countries,
                Cities = cities

            };
            return View(model);
        }
       
        public ActionResult SetSocialProfileUnArchive(string id)
        {
            var jr = new JsonResult();
            try
            {
                var SPId = HttpUtility.UrlDecode(id);
                int socialPrpfileId = Convert.ToInt32((CryptoEngine.Decrypt(SPId)));
                var JVListingData = _customerManager.SetSocialProfileArchive(socialPrpfileId, 0);
                if (JVListingData == true)
                {
                    jr.Data = new { ResultType = "Success", message = "Profile has been unarchived successfully" };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Error. Can not find the details." };
                }

                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult SetSocialProfileArchive(int id,int status)
        {
            var jr = new JsonResult();
            try
            {
                var JVListingData = _customerManager.SetSocialProfileArchive(id, status);
                if (JVListingData == true)
                {
                    jr.Data = new { ResultType = "Success", message = "" };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Error. Can not find the details." };
                }

                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult GetProfileByJVStatusId(int id)
        {
            var jr = new JsonResult();
            try
            {
                var JVListingData = _customerManager.GetActionBoardData(id);
                if (JVListingData != null)
                {
                    jr.Data = new { ResultType = "Success", message = "", ResultData = JVListingData };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Error. Can not find the details." };
                }

                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaveUpdateUserDataIndividually(string value, string fieldName, string customerId, string SocialProfileId)
        {
            var jr = new JsonResult();
            try
            {
                if (!string.IsNullOrEmpty(customerId))
                {
                    if (fieldName == "Comment")
                    {

                        value = this.CDT.UserName + " (" + DateTime.Now.ToString("dd MMM, yyyy HH:mm") + "): " + value;
                    }

                    var Cus = HttpUtility.UrlDecode(customerId);
                    var SPId = HttpUtility.UrlDecode(SocialProfileId);
                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int custId = Convert.ToInt32((CryptoEngine.Decrypt(Cus)));
                    int socialPrpfileId = Convert.ToInt32((CryptoEngine.Decrypt(SPId)));
                    var User = _customerManager.SaveUpdateUserDataIndividually(value, fieldName, custId, socialPrpfileId);

                    var JVDetailData = _customerManager.GetUserComment(custId);
                    jr.Data = new { ResultType = "Success", message = "", ResultData = JVDetailData };
                }
                //return _customerManager.IsEmailExist(EmailAddress, id) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult GetProfileDetailById(string id)
        {
            var jr = new JsonResult();
            try
            {

                int SocialPId = Convert.ToInt32(CryptoEngine.Decrypt(id));
                var JVDetailData = _customerManager.GetSpecificUserTargettingInformation(0, SocialPId);
                string PWTOENG = "";
                StringBuilder sb = new StringBuilder();
                CultureInfo enGB = CultureInfo.CreateSpecificCulture("en-GB");
                if (JVDetailData != null)
                {
                    sb.Append("Name: ").Append(Indent(50)).Append(JVDetailData.UserName);
                    //sb.AppendFormat(Indent(60), "{0}", );
                    sb.AppendLine();
                    sb.Append("Insta Username: ").Append(Indent(32)).Append(JVDetailData.InstaUser);
                    sb.AppendLine();
                    sb.Append("Insta Password: ").Append(Indent(32)).Append(JVDetailData.InstaPassword);

                    sb.AppendLine();

                    sb.Append("Hash Tag want to engage: ");
                    sb.AppendLine();

                    if(!string.IsNullOrEmpty(JVDetailData.Preference1))
                    { 
                    String[] strlist = JVDetailData.Preference1.Split(',');

                        foreach (String s in strlist)
                        {
                            sb.Append(Indent(63)).Append(s);
                            sb.AppendLine();
                        }
                    }

                    sb.Append("Hash Tag do not want to engage: ");
                    sb.AppendLine();

                    if (!string.IsNullOrEmpty(JVDetailData.Preference2))
                    {
                        String[] strPrf2 = JVDetailData.Preference2.Split(',');

                        foreach (String s in strPrf2)
                        {
                            sb.Append(Indent(63)).Append(s);
                            sb.AppendLine();
                        }
                    }
                    


                    sb.Append("Locations to focus: ");

                    sb.AppendLine();

                    if (!string.IsNullOrEmpty(JVDetailData.Preference3))
                    {
                        String[] strPrf3 = JVDetailData.Preference3.Split(',');

                        foreach (String s in strPrf3)
                        {
                            sb.Append(Indent(63)).Append(s);
                            sb.AppendLine();
                        }
                    }
                    
                    sb.Append("Follow Unfollow activites: ").Append(Indent(16)).Append(JVDetailData.Preference5 == 1 ? "Yes" : " No");
                    sb.AppendLine();
                    if (JVDetailData.Preference6 == 1)
                    {
                        PWTOENG = "Male";
                    }
                    else if (JVDetailData.Preference6 == 2)
                    {
                        PWTOENG = "Female";
                    }
                    else
                    {
                        PWTOENG = "Both";
                    }
                    sb.Append("People want to engage: ").Append(Indent(20)).Append(PWTOENG);
                    sb.AppendLine();
                    sb.Append("Include business account:").Append(Indent(16)).Append(JVDetailData.Preference7 == 1 ? "Yes" : "No");
                    sb.AppendLine();
                    sb.Append("Turn on AI:").Append(Indent(43)).Append(JVDetailData.Preference10 == 1 ? "Yes" : "No");
                    sb.AppendLine();

                    sb.Append("Direct competitors: ");
                    sb.AppendLine();
                    if (!string.IsNullOrEmpty(JVDetailData.Preference9))
                    {
                        String[] strPrf9 = JVDetailData.Preference9.Split(',');

                        foreach (String s in strPrf9)
                        {
                            sb.Append(Indent(63)).Append(s);
                            sb.AppendLine();
                        }
                    }

                    var targetpref = new
                    {
                        JVBoxStatusId = JVDetailData.Status,
                        TargetPreferences = sb.ToString(),
                        SPId = JVDetailData.SPId,
                        SocialProfileId = HttpUtility.UrlEncode(CryptoEngine.Encrypt(JVDetailData.SPId.ToString())),
                        CustomerId = HttpUtility.UrlEncode(CryptoEngine.Encrypt(JVDetailData.Id.ToString())),
                        InstaUser = JVDetailData.InstaUser ?? "",
                        Email = JVDetailData.Email ?? "",
                        Notes = JVDetailData.Notes,
                        Country = JVDetailData.Country,
                        City = JVDetailData.City,
                        MPBox = JVDetailData.MPBox,
                        IP = JVDetailData.IP==null?JVDetailData.IP:JVDetailData.IP+":"+JVDetailData.ProxyPort,
                        ProxyId = JVDetailData.ProxyId,
                        IsArchived=JVDetailData.IsArchived
                    };

                    jr.Data = new { ResultType = "Success", message = "", ResultData = targetpref };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Error. Can not find the details." };
                }

                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Indent(int count)
        {
            return "".PadLeft(count);
        }

        public ActionResult GetCityByCountryId(short countryId)
        {
            var Cities = CommonManager.GetCities().Where(m => m.CountryId == countryId).ToList();
            return Json(Cities, JsonRequestBehavior.AllowGet);
        }

    }
}