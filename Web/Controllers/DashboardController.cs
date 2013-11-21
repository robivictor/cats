﻿using System.Collections.Generic;
using System.Linq;
using Cats.Services.EarlyWarning;
using System.Web.Mvc;
using Cats.Services.Common;
using Cats.Services.Security;


namespace Cats.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _IDashboardService;
        private readonly  INeedAssessmentSummaryService _INeedAssessmentSummaryService;
        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly IUserAccountService _userAccountService;
        private readonly INotificationService _notificationService;
        public DashboardController(INotificationService notificationService, IUserAccountService userAccountService)
        {
            _notificationService = notificationService;
            _userAccountService = userAccountService;
            this._IDashboardService = new Cats.Services.EarlyWarning.DashboardService();
            this._INeedAssessmentSummaryService = new Cats.Services.Common.NeedAssessmentSummaryService();
        }

        public ActionResult RequestsById(int RegionId=10)
        {
            var model = _IDashboardService.RegionalRequests(RegionId);
            return PartialView("_Requests", model);
        }

        public JsonResult dRegionalMonthlyRequests()
        {
           return Json(_IDashboardService.Requests(),JsonRequestBehavior.AllowGet);
        }

        public ActionResult Requests()
        {
            var model = _IDashboardService.Requests();
            return PartialView("_Requests", model);
        }

        public JsonResult RegionalMonthlyRequests() {
            return Json(_IDashboardService.RMRequests(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegionalRequestsById(int RegionId)
        {
            return Json(_IDashboardService.RegionalRequestsByRegionID(RegionId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PieRequests()
        {
            return Json(_IDashboardService.PieRegionalRequests(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReliefRequisitionBasedOnStatus()
        {
            return Json(_IDashboardService.RequisitionBasedOnStatus(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult BarBeneficiaries()
        {
            return Json(_IDashboardService.BarNoOfBeneficiaries(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult BarNeedAssessment(string regionName) {
            return Json(_INeedAssessmentSummaryService.NeedAssessmentByRegion(regionName), JsonRequestBehavior.AllowGet);
        }
        
        // int y = DateTime.Now.Year;

        public JsonResult BarNeedAssessmentbY(int year = 2013)
        {
            return Json(_INeedAssessmentSummaryService.NeedAssessmentByYear(year), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getYears()
        {
            return Json(_INeedAssessmentSummaryService.GetYears(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult BarRegionalReqDetailCommodity()
        {
                return Json(_IDashboardService.RegionalRequestsBeneficiary(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ZonalBeneficiaries(string RegionName)
        {
            return Json(_IDashboardService.ZonalBeneficiaries(_IDashboardService.getRegionId(RegionName)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ZonalMonthlyBeneficiaries(string RegionName, string ZoneName)
        {
            return Json(_IDashboardService.ZonalMonthlyBeneficiaries(RegionName, ZoneName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnreadNotifications()
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            var notifications = _IDashboardService.GetUnreadNotifications();
            var roles = _userAccountService.GetUserPermissions(user).Select(a => a.Roles).ToList();
            var allUserRollsInAllApplications = new List<string>();

            foreach (var app in roles)
            {
                allUserRollsInAllApplications.AddRange(app.Select(role => role.RoleName));
            }
            var totalUnread = _notificationService.GetAllNotification().Where(n => n.IsRead == false && allUserRollsInAllApplications.Contains(n.RoleName)).ToList();
            return Json(totalUnread, JsonRequestBehavior.AllowGet);
        }

        
    }
}