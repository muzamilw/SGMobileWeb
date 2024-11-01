﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.Statistics;
using SG2.CORE.MODAL.ViewModals.Backend.DashBoard;
using SG2.CORE.MODAL.ViewModals.Statistics;
using SG2.CORE.WEB.App_Start;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class DashboardController : BaseController
    {
        private readonly StatisticsManager _statisticsManager;
        private readonly NotificationManager _notificationManager;
        //private readonly JVBoxManager _jVBoxManager;

        public DashboardController()
        {
            _statisticsManager = new StatisticsManager();
            _notificationManager = new NotificationManager();
            //_jVBoxManager = new JVBoxManager();
            ViewBag.SetMenuActiveClass = "Dashboard";
        }

        public ActionResult Index()
        {
            try
            {
                //var jvBoxes = _jVBoxManager.GetJVBoxData(null, "200", 1, null);
                var model = new DashboardViewModel()
                {
                    dashboardListingModel = null,
                };
                return View(model);

            }
            catch (Exception EX)
            {

                throw EX;
            }
        }


        public ActionResult GetAdminReports()
        {
            var jr = new JsonResult();
            try
            {


                //AdminReportViewModel jvVBoxandProxyIPsData = _statisticsManager.GetJVBoxandProxyIPsData(DateTime.Now.AddMonths(-3), DateTime.Now);
                List<PlanListReportDTO> mostUsedProductData = _statisticsManager.GetMostUsedProductData(DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(1));

                StringBuilder planNames = new StringBuilder();
                StringBuilder planNamesWithUsages = new StringBuilder();
                //string planNamesWithUsages = string.Empty;
                string noOfAvailablePlans = string.Empty;
                if (mostUsedProductData != null)
                {

                    foreach (var item in mostUsedProductData)
                    {

                        planNames.AppendFormat(item.name);
                        planNames.Append(",");

                        planNamesWithUsages.AppendFormat(item.value);
                        planNamesWithUsages.Append(",");
                        //planNamesWithUsages = item.PlanValue.ToString();
                        //planNamesWithUsages = item.PlanValue;
                        //noOfAvailablePlans = item.NoOfAvailablePlans.ToString();
                    }
                }


                
                    jr.Data = new
                    {
                        ResultType = "Success",
                        message = "",
                        ResultData = new
                        {

                            AllSlotsOnJVBox = 1,//jvVBoxandProxyIPsData.AllSlotsOnJVBox.ToString(),
                            UsedSlotsOnJVBox = 1,//jvVBoxandProxyIPsData.UsedSlotsOnJVBox.ToString(),
                            FreeSlotsOnJVServer = 1,//jvVBoxandProxyIPsData.FreeSlotsOnJVServer.ToString(),
                            TotalUsedIPs = 1,//jvVBoxandProxyIPsData.TotalUsedIPs.ToString(),
                            AllAvailableIPs = 1,//jvVBoxandProxyIPsData.AllAvailableIPs.ToString(),
                            RemainingProxyIPs = 1,//jvVBoxandProxyIPsData.RemainingProxyIPs.ToString(),

                            PlanNames = mostUsedProductData.Select(x => x.name).ToArray(),
                            PlanNamesWithUsages = mostUsedProductData,
                            NoOfAvailablePlans = noOfAvailablePlans

                        },
                        EventsData = _statisticsManager.GetProfileEvents(DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(1)),
                        ActionsData = _statisticsManager.GetActionReport(DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(1))
                    };
               

            }
            catch (Exception exp)
            {
                throw exp;
            }

            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNotificationData(int id)
        {
            var jr = new JsonResult();
            try
            {
                var NotificationData = _notificationManager.GetNotificationByStatusId(1);
                if (NotificationData != null)
                {

                    jr.Data = new { ResultType = "Success", message = "", ResultData = NotificationData };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Error. Can not find the data." };
                }

                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public ActionResult ReadNotification(string id, short statusId)
        {
            var jr = new JsonResult();
            string messages = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    int notificationid = Convert.ToInt32(id);
                    bool User;
                    if (statusId == 53)
                    {
                        User= _notificationManager.DeleteNotification(notificationid, statusId);
                    }
                    else
                        {

                        User = _notificationManager.UpdateNotification(notificationid, statusId);
                    }
                    if (User)
                    {
                        jr.Data = new { ResultType = "Success", message = "Status updated successfully." };
                    }

                    else
                    {
                        messages = "Something went wrong";
                        jr.Data = new { ResultType = "Error", message = messages };
                    }

                }

                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }


        }

    }
}