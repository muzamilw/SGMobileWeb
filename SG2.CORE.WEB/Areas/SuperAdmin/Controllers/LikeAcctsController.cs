using PagedList;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.LikeyAccount;
using SG2.CORE.MODAL.ViewModals.Backend.LikeyAccount;
using SG2.CORE.WEB.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class LikeAcctsController : BaseController
    {
        protected readonly LikeyAccountManager _likeyAccountManager;
        private readonly string _PageSize = string.Empty;
        protected readonly CustomerManager _customerManager;

        public LikeAcctsController()
        {
            _customerManager = new CustomerManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            _likeyAccountManager = new LikeyAccountManager();
            ViewBag.SetMenuActiveClass = "LikeAccts";
        }
       
        public ActionResult GetCityByCountryId(short countryId)
        {
           var Cities = CommonManager.GetCities().Where(m=> m.CountryId== countryId).ToList();            
            return Json(Cities, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null)
        {
            try
            {
                var likeyAccounts = _likeyAccountManager.GetLikeyAccountData(SearchCriteria, _PageSize, page, StatusId);
                var totatRecord = likeyAccounts.FirstOrDefault()?.TotalRecord ?? 0;
                IPagedList<LikeyAccountListingViewModal> pageOrders = new StaticPagedList<LikeyAccountListingViewModal>(likeyAccounts, page, Convert.ToInt32(_PageSize), totatRecord);
                var model = new LikeyAccountIndexViewModal()
                {
                    LikeyAccountListing = pageOrders,
                    ApplicationStatuses = this.ApplicationStatuses,
                    TotalRecord = pageOrders.PageCount,
                    PageNumber= pageOrders.PageNumber,
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

        public ActionResult UpdateLikeyAccount(string id)
        {
            LikeyAccountDTO likeyAccountDTO = new LikeyAccountDTO();
            UpdateLikeyAccountViewModel model = new UpdateLikeyAccountViewModel
            {
                //Countries = CommonManager.GetCountries(),
                Statuses = this.ApplicationStatuses
            };
            int likeyAccountId = 0;
            if (id != null)
                likeyAccountId = Convert.ToInt32(CryptoEngine.Decrypt(id));

            likeyAccountDTO = _likeyAccountManager.GetJLikeyAccountById(likeyAccountId);
            
            if (likeyAccountDTO != null)
            {
                model.Cities = CommonManager.GetCities().Where(m => m.CountryId == Convert.ToInt16(likeyAccountDTO.Country)).ToList();
                model.LikeyAccountId = likeyAccountDTO.LikeyAccountId;
                model.InstaUserName = likeyAccountDTO.InstaUserName;
                model.InstaPassword = likeyAccountDTO.InstaPassword;
                model.Country = likeyAccountDTO.Country;
                model.City = likeyAccountDTO.City;
                model.Gender = likeyAccountDTO.Gender;
                model.HashTag = likeyAccountDTO.HashTag;
                model.StatusId = likeyAccountDTO.StatusId;
            }

            return View(model);


        }

        [HttpPost]
        public ActionResult UpdateLikeyAccount(UpdateLikeyAccountViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _likeyAccountManager.UpdateLikeyAccount(new LikeyAccountDTO()
                    {
                        LikeyAccountId = model.LikeyAccountId,
                        InstaUserName = model.InstaUserName,
                        InstaPassword = model.InstaPassword,
                        Gender = model.Gender,
                        Country = model.Country,
                        City = model.City,
                        HashTag = model.HashTag,
                        StatusId = model.StatusId
                    });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "Likey Account updated successfully.";
                    return RedirectToAction("Index", "LikeAccts");
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

        public ActionResult Delete(string id)
        {
            var jr = new JsonResult();

            int LikeyAccountId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            var DJVB = _likeyAccountManager.DeleteLikeyAccount(LikeyAccountId);
            if (DJVB)
            {
                jr.Data = new { ResultType = "Success", message = "Item deleted successfully." };
               
            }
            else
            {
                jr.Data = new { ResultType = "Error", message = "Error. Item no deleted. Please contact admin." };
               
            }
            return Json(jr, JsonRequestBehavior.AllowGet);


           // return RedirectToAction("Index", "LikeAccts");
        }

        

      public JsonResult IsInstaUserNameExists(string InstaUserName, int LikeyAccountId = 0)
        {
            try
            {
                return _likeyAccountManager.IsInstaUserNameExists(InstaUserName, LikeyAccountId) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Detail()
        {
            LikeyAccountDTO likeyAccountDTO = new LikeyAccountDTO();
            LikeyAccountViewModal model = new LikeyAccountViewModal
            {
                //Countries = CommonManager.GetCountries(),
                Cities = new List<CityDTO>(),
                Statuses = this.ApplicationStatuses
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(LikeyAccountViewModal model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _likeyAccountManager.UpdateLikeyAccount(new LikeyAccountDTO()
                    {
                        LikeyAccountId = model.LikeyAccountId,
                        InstaUserName = model.InstaUserName,
                        InstaPassword = model.InstaPassword,
                        Gender = model.Gender,
                        Country = model.Country,
                        City = model.City,
                        HashTag = model.HashTag,
                        StatusId = model.StatusId
                    });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "Likey Account updated successfully.";
                    return RedirectToAction("Index", "LikeAccts");
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

    }
}