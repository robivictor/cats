﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Services.EarlyWarning;
using System.Web.Mvc;
using Cats.Models.ViewModels;


namespace Cats.Controllers
{
    public class DashboardController : Controller
    {
        public DashboardController()
        {
            this._IDashboardService = new Cats.Services.EarlyWarning.DashboardService();
        }
        private readonly IDashboardService _IDashboardService;

        public ActionResult RequestsById(int RegionId=10)
        {
            var model = _IDashboardService.RegionalRequests(RegionId);
            return PartialView("_Requests", model);
        }

        public ActionResult Requests()
        {
            var model = _IDashboardService.Requests();
            return PartialView("_Requests", model);
        }
        
        public ActionResult RegionalRequestsById(int RegionId)
        {
            return Json(_IDashboardService.RegionalRequestsByRegionID(RegionId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PieRequests()
        {
            return Json(_IDashboardService.PieRegionalRequests(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult BarBeneficiaries()
        {

            return Json(_IDashboardService.BarNoOfBeneficiaries(), JsonRequestBehavior.AllowGet);
        }
    }
}