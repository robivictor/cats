﻿using Cats.Services.EarlyWarning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Data.UnitWork;

namespace Cats.Controllers
{
    
    public class HomeController : Controller
    {

        private IRegionalRequestService _regionalRequestService;
        private IReliefRequisitionService _reliefRequistionService; 
       
        private IUnitOfWork _unitOfWork = new UnitOfWork();
             

        public HomeController() {
            _regionalRequestService = new RegionalRequestService(_unitOfWork);
            _reliefRequistionService = new ReliefRequisitionService(_unitOfWork);
        }

        //
        // GET: /Home/
        [Authorize]

        public ActionResult Index(int regionId=4)
        {

            //var req = _reliefRequistionService.FindBy(t => t.RegionID == regionId);
            var req = _regionalRequestService.FindBy(t => t.RegionID == regionId);
            ////ViewBag.Requests = req;
            return View();
            //return Json(req, JsonRequestBehavior.AllowGet);
           // return Json(req, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegionalMonthlyRequests()
        {
            var request = _unitOfWork.RegionalRequestRepository.FindById(2);
            return Json(request, JsonRequestBehavior.AllowGet);
        }
    }
}
