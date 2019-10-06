using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.ViewModals.Backend.BotManager;
using SG2.CORE.WEB.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class BotMngrController : BaseController
    {
        protected readonly JVBoxManager _jVBoxManager;
        protected readonly QueueLoggerManager _queueLoggerManager;
        public BotMngrController()
        {
            _jVBoxManager = new JVBoxManager();
            _queueLoggerManager = new QueueLoggerManager();
            ViewBag.SetMenuActiveClass = "BotMngr";
        }
        // GET: SuperAdmin/BotMngr
        public ActionResult Index()
        {
            var jvBoxes = _jVBoxManager.GetJVBoxData(null, "200", 1, null);
            var model = new BotMngrViewModel()
            {
                JVBoxes = jvBoxes,
            };
            return View(model);
        }

        public ActionResult getBotdatabyServer(int id)
        {
            var jr = new JsonResult();
            try
            {
                var BotData = _queueLoggerManager.GetQueueAudit(id);
                if (BotData != null)
                {
                    jr.Data = new { ResultType = "Success", message = "", ResultData = BotData };
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

        public ActionResult getimageData(string id)
        {
            var jr = new JsonResult();
            try
            {
                var BotData = _queueLoggerManager.getimageData(id);
                if (BotData != null)
                {
                    jr.Data = new { ResultType = "Success", message = "", ResultData = BotData };
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

        public ActionResult DeleteBotData(string id)
        {
            var jr = new JsonResult();
            try
            {
                var BotDetailData = _queueLoggerManager.DeleteBotData(id);
                if (BotDetailData)
                {
                    jr.Data = new { ResultType = "Success", message = "", ResultData = BotDetailData };
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


        public ActionResult DeleteBotDetailData(int id)
        {
            var jr = new JsonResult();
            try
            {
                var BotDetailData = _queueLoggerManager.DeleteBotDetailData(id);
                if (BotDetailData)
                {
                    jr.Data = new { ResultType = "Success", message = "", ResultData = BotDetailData };
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
        public ActionResult getBotdetailData(string id)
        {
            var jr = new JsonResult();
            try
            {
                var BotDetailData = _queueLoggerManager.GetQueueAuditDetail(id);
                if (BotDetailData != null)
                {
                    jr.Data = new { ResultType = "Success", message = "", ResultData = BotDetailData };
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

    }
}