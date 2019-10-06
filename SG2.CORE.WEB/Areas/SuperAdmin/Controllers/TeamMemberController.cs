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
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    public class TeamMemberController : BaseController
    {
        protected readonly TeamMemberManager _teamMemberManager;
        private readonly string _PageSize = string.Empty;
        protected readonly CustomerManager _customerManager;
        protected readonly List<SystemSettingsDTO> SystemConfigs;

        public TeamMemberController()
        {
            _customerManager = new CustomerManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            _teamMemberManager = new TeamMemberManager();
            SystemConfigs = SystemConfig.GetConfigs;
            ViewBag.SetMenuActiveClass = "TeamMember";
        }

        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null)
        {
            try
            {
                var teamMember = _teamMemberManager.GetTeamMemberData(SearchCriteria, _PageSize, page, StatusId);
                var totatRecord = teamMember.FirstOrDefault()?.TotalRecord ?? 0;


                IPagedList<TeamMemberListingViewModal> pageOrders = new StaticPagedList<TeamMemberListingViewModal>(teamMember, page, Convert.ToInt32(_PageSize), totatRecord);
                var model = new TeamMemberIndexViewModal()
                {
                    TeamMemberListing = pageOrders,
                    ApplicationStatuses = this.ApplicationStatuses,
                    PageNumber = pageOrders.PageNumber,
                    TotalRecord = pageOrders.PageCount,
                    SearchCriteria = SearchCriteria,
                    StatusId = StatusId
                };

                return View(model);

            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
        
        public ActionResult Add()
        {
            IList<SystemRoleDTO> roleList = _teamMemberManager.GetAllRoleList();
            AddTeamMemberViewModal model = new AddTeamMemberViewModal();
            model.RoleListing = roleList;
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AddTeamMemberViewModal model)
        {
            try
            {
                KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
                KlaviyoProfile klaviyoProfile = new KlaviyoProfile();
                if (ModelState.IsValid)
                {
                    var dl = _teamMemberManager.UpdatSystemUser(
                        new SystemUserDTO()
                        {
                            SystemUserId = model.SystemUserId,
                            Title = model.Title,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            StatusId = (Int16)GlobalEnums.SystemUserStatus.Active, //model.StatusId,
                            Email = model.Email,
                            Password = model.Password,
                            SystemRoleId = model.SystemRoleId,
                            HostUser = false,
                            CreatedBy = CDT.FirstName + "" + CDT.LastName,
                            CreatedOn = DateTime.Now,
                            ModifiedBy = CDT.FirstName + "" + CDT.LastName,
                            ModifiedOn = DateTime.Now,
                        }
                    );

                    var encryptData = CryptoEngine.Encrypt(dl.SystemUserId + "#" + DateTime.Now.Date);
                    string URL = HttpContext.Request.Url.Scheme.ToString() + "://" + HttpContext.Request.Url.Authority.ToString() + "/Home/VerifyEmail?token=" + Url.Encode(encryptData);

                    //eventAPI();
                    var list = new List<NotRequiredProperty>();
                    list.Add(new NotRequiredProperty("$email", model.Email));
                    list.Add(new NotRequiredProperty("$first_name ", model.FirstName));
                    list.Add(new NotRequiredProperty("$last_name ", model.FirstName));
                    list.Add(new NotRequiredProperty("URL", URL));
                    list.Add(new NotRequiredProperty("RESEND", false));
                    klaviyoProfile.email = model.Email;
                    

                    var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                    var _klavio_TeamMembersList = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_TeamMembersList").ToLower()).ConfigValue;

                    klaviyoAPI.PeopleAPI(list, _klaviyoPublishKey);
                    var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, _klavio_TeamMembersList);

                    TempData["Success"] = "Yes";
                    TempData["Message"] = "User created successfully.";
                    return RedirectToAction("Index", "TeamMember");
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    //TempData["Success"] = "False";
                    //TempData["Message"] = messages;
                    return View(model);
                }
            }
            catch (Exception)
            {

                throw;
            }
           
            
        }

        public ActionResult Update(string id)
        {
            SystemUserDTO systemUserDTO = new SystemUserDTO();
            UpdateTeamMemberViewModal model = new UpdateTeamMemberViewModal();
            int systemUserId = 0;
            if (id != null)
                systemUserId = Convert.ToInt32(CryptoEngine.Decrypt(id));

            systemUserDTO = _teamMemberManager.GetSystemUserId(systemUserId);

            IList<SystemRoleDTO> roleList = _teamMemberManager.GetAllRoleList();
            if (systemUserDTO != null)
            {
                model.RoleListing = roleList;
                model.Title = systemUserDTO.Title;
                model.SystemUserId = systemUserDTO.SystemUserId;
                model.FirstName = systemUserDTO.FirstName;
                model.LastName = systemUserDTO.LastName;
                model.Email = systemUserDTO.Email;
                model.Password = systemUserDTO.Password;
                model.StatusId = systemUserDTO.StatusId;
                model.SystemRoleId = systemUserDTO.SystemRoleId;
               
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(UpdateTeamMemberViewModal model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _teamMemberManager.UpdatSystemUser(new SystemUserDTO()
                    {
                        SystemUserId = model.SystemUserId,
                        Title = model.Title,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        StatusId = model.StatusId,
                        SystemRoleId = model.SystemRoleId,
                        CreatedBy = CDT.FirstName + "" + CDT.LastName,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = CDT.FirstName + "" + CDT.LastName,
                        ModifiedOn = DateTime.Now,
                    });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "System User updated successfully.";
                    return RedirectToAction("Index", "TeamMember");
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    //TempData["Success"] = "False";
                    //TempData["Message"] = messages;
                    return View(model);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult IsEmailExist(string Email, int SystemUserId = 0)
        {
            try
            {
                return _teamMemberManager.IsEmailExist(Email, SystemUserId) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}