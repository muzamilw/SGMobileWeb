using klaviyo.net;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.DTO.TeamMember;
using SG2.CORE.MODAL.klaviyo;
using SG2.CORE.MODAL.ViewModals.Backend.TeamMember;
using SG2.CORE.MODAL.ViewModals.Customers;
using SG2.CORE.WEB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    public class UserProfileController : BaseController
    {
        // GET: SuperAdmin/UserProfile
        protected readonly TeamMemberManager _teamMemberManager;
        protected readonly CustomerManager _customerManager;
        protected readonly List<SystemSettingsDTO> SystemConfigs;

        public UserProfileController()
        {
            _teamMemberManager = new TeamMemberManager();
            _customerManager = new CustomerManager();
            SystemConfigs = SystemConfig.GetConfigs;
        }

        public ActionResult UpdateUserProfile(string id)
        {
            if (!string.IsNullOrEmpty((string)TempData["Success"]))
            {
                ViewBag.Success = (string)TempData["Success"];
                ViewBag.Message = TempData["Message"];
            }

            SystemUserDTO systemUserDTO = new SystemUserDTO();
           
            int systemUserId = 0;
            if (id != null)
                systemUserId = Convert.ToInt32(CryptoEngine.Decrypt(id));


            systemUserDTO = _teamMemberManager.GetSystemUserId(systemUserId);

            IList<SystemRoleDTO> roleList = _teamMemberManager.GetAllRoleList();

            var userUpdate = new TeamMemberProfileDataViewModel()
            {
                RoleListing = roleList,
                Title = systemUserDTO.Title,
                SystemUserId = systemUserDTO.SystemUserId,
                FirstName = systemUserDTO.FirstName,
                LastName = systemUserDTO.LastName,
                Email = systemUserDTO.Email,
                StatusId = systemUserDTO.StatusId,
                SystemRoleId = systemUserDTO.SystemRoleId

            };
            TeamMemberProfileUpdateViewModal model = new TeamMemberProfileUpdateViewModal()
            {
                TMProfileUpdateVM = userUpdate,
                TMPasswordVM = new TeamMemberPasswordDataViewModel() {SystemUserId= systemUserDTO.SystemUserId,VerifyPasswordValidation=systemUserDTO.Password }

            };

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateUserPassword(TeamMemberPasswordDataViewModel model)
        {
            try
            {
                var jr = new JsonResult();
                if (ModelState.IsValid)
                {
                    if (_teamMemberManager.UpdateCustomerPassword(model.Password, model.SystemUserId))
                    {
                        jr.Data = new { ResultType = "Success", message = "Password updated successfully." };
                    }
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
        
        [HttpPost]
        public ActionResult UpdateUserProfile(TeamMemberProfileDataViewModel model)
        {
            try
            {
                var jr = new JsonResult();
                if (ModelState.IsValid)
                {
                    SystemUserDTO systemUserDTO = new SystemUserDTO();
                    systemUserDTO = _teamMemberManager.GetSystemUserId(model.SystemUserId);
                    var dl = _teamMemberManager.UpdatSystemUser(new SystemUserDTO()
                    {
                        SystemUserId = model.SystemUserId,
                        Title = model.Title,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        StatusId = systemUserDTO.StatusId,
                        SystemRoleId = systemUserDTO.SystemRoleId,
                        CreatedBy = CDT.FirstName + "" + CDT.FirstName,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = CDT.FirstName + "" + CDT.FirstName,
                        ModifiedOn = DateTime.Now,
                    });
                    jr.Data = new { ResultType = "Success", message = "Profile updated successfully." };
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
        
        public ActionResult VerifyEmail(string token)
        {
            if (!string.IsNullOrEmpty((string)TempData["Success"]))
            {
                ViewBag.Success = (string)TempData["Success"];
                ViewBag.Message = TempData["Message"];
            }


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

                    var totalHours = (sessionDateTime - currentDate).TotalHours;
                    if (totalHours < 0)
                    {


                        _customerManager.ActivateCustomerPassword(statusId, customerId);
                    }
                    model.CustomerId = customerId;
                }
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
                int customerId = model.CustomerId; // Convert.ToInt32(CryptoEngine.Decrypt(model.));
                var customer = _customerManager.GetCustomerDTOByCustomerId(customerId);
                var encryptData = CryptoEngine.Encrypt(customerId + "#" + System.DateTime.Now.Date);
                string URL = HttpContext.Request.Url.Scheme.ToString() + "://" + HttpContext.Request.Url.Authority.ToString() + "/VerifyEmail?token=" + Url.Encode(encryptData);

                var list = new List<NotRequiredProperty>();
                list.Add(new NotRequiredProperty("$email", customer.EmailAddress));
                list.Add(new NotRequiredProperty("URL", URL));
                list.Add(new NotRequiredProperty("RESEND", true));

                var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                klaviyoAPI.PeopleAPI(list, _klaviyoPublishKey);
                klaviyoProfile.email = customer.EmailAddress;
                //klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile);

                TempData["Success"] = "Yes";
                TempData["Message"] = "Email sent successfully.";

                return RedirectToAction("VerifyEmail", "Home");
            }
            catch (Exception exp)
            {
                throw exp;
            }



        }

        public JsonResult CheckCurrentPassword(string CurrentPassword,string VerifyPasswordValidation)
       {
            try
            {
                return VerifyPasswordValidation == CurrentPassword ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}