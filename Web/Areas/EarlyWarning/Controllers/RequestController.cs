﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Cats.Areas.EarlyWarning.Models;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using Cats.Helpers;
using Cats.ViewModelBinder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Workflow = Cats.Models.Constant.WORKFLOW;


namespace Cats.Areas.EarlyWarning.Controllers
{
    public class RequestController : Controller
    {
        //
        // GET: /EarlyWarning/RegionalRequest/

        private IRegionalRequestService _regionalRequestService;
        private IFDPService _fdpService;
        private IRegionalRequestDetailService _regionalRequestDetailService;
        private ICommonService _commonService;

        public RequestController(IRegionalRequestService reliefRequistionService
                                 , IFDPService fdpService,
                                 IRegionalRequestDetailService reliefRequisitionDetailService,
                                ICommonService commonService
            )
        {
           _regionalRequestService = reliefRequistionService;
           _fdpService = fdpService;
            _regionalRequestDetailService = reliefRequisitionDetailService;
            _commonService = commonService;
        }



      

        public ViewResult SubmittedRequest(int id)
        {
            ViewBag.Months = new SelectList(RequestHelper.GetMonthList(), "Id", "Name");
            ViewBag.RegionID = new SelectList(_commonService.GetAminUnits(t => t.AdminUnitTypeID == 2), "AdminUnitID",
                                              "Name");
            ViewBag.Status = new SelectList(_commonService.GetStatus(Workflow.REGIONAL_REQUEST), "StatusID",
                                            "Description");

            var requests = _regionalRequestService.Get(t => t.Status == id, null, "AdminUnit,Program");
            var statuses = _commonService.GetStatus(WORKFLOW.REGIONAL_REQUEST);

            return View(RequestViewModelBinder.BindRegionalRequestListViewModel(requests, statuses));
        }




        [HttpGet]
        public ActionResult New()
        {
            
            PopulateLookup();
            ViewBag.SeasonID = new SelectList(_commonService.GetSeasons(), "SeasonID", "Name");
         
            return View();
        }
        private RegionalRequest CretaeRegionalRequest(HRDPSNPPlanInfo hrdpsnpPlanInfo)
        {
            var regionalRequest = new RegionalRequest();
            regionalRequest.Status = (int)RegionalRequestStatus.Draft;
            regionalRequest.RequistionDate = DateTime.Today;
            regionalRequest.Year = hrdpsnpPlanInfo.HRDPSNPPlan.Year;
            regionalRequest.Month = hrdpsnpPlanInfo.HRDPSNPPlan.Month;
            regionalRequest.RegionID = hrdpsnpPlanInfo.HRDPSNPPlan.RegionID;
            regionalRequest.ProgramId = hrdpsnpPlanInfo.HRDPSNPPlan.ProgramID;

            regionalRequest.RegionalRequestDetails = (from item in hrdpsnpPlanInfo.BeneficiaryInfos
                                                      select new RegionalRequestDetail()
                                                                 {
                                                                     Beneficiaries=item.Beneficiaries,
                                                                     Fdpid=item.FDPID
                                                                 }).ToList();
            _regionalRequestService.AddRegionalRequest(regionalRequest);

            return regionalRequest;
        }
        private void PopulateLookup()
        {
            ViewBag.RegionID = new SelectList(_commonService.GetAminUnits(t => t.AdminUnitTypeID == 2), "AdminUnitID", "Name");
            ViewBag.ProgramId = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name");
            ViewBag.Month = new SelectList(RequestHelper.GetMonthList(), "ID", "Name");
            ViewBag.RationID = new SelectList(_commonService.GetRations(), "RationID", "RefrenceNumber");
           
        }
        private void PopulateLookup(RegionalRequest regionalRequest)
        {
            ViewBag.RegionID = new SelectList(_commonService.GetAminUnits(t => t.AdminUnitTypeID == 2), "AdminUnitID", "Name", regionalRequest.RegionID);
            ViewBag.ProgramId = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name", regionalRequest.ProgramId);
            ViewBag.Month = new SelectList(RequestHelper.GetMonthList(), "ID", "Name", regionalRequest.Month);
            ViewBag.RationID = new SelectList(_commonService.GetRations(), "RationID", "RefrenceNumber", regionalRequest.RationID);
           
        }
        //
        // GET: /ReliefRequisitoin/Details/5


        [HttpGet]
        public ActionResult Edit(int id)
        {

            var regionalRequest =
                _regionalRequestService.Get(t => t.RegionalRequestID == id, null,
                                            "RegionalRequestDetails,RegionalRequestDetails.Fdp," +
                                            "RegionalRequestDetails.Fdp.AdminUnit,RegionalRequestDetails.Fdp.AdminUnit.AdminUnit2")
                    .
                    FirstOrDefault();
            if (regionalRequest == null)
            {
                return HttpNotFound();
            }
            PopulateLookup(regionalRequest);
            return View(regionalRequest);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(RegionalRequest regionalRequest)
        {
            var requId = 0;
            if (regionalRequest != null && ModelState.IsValid)
            {
                var target = _regionalRequestService.FindById(regionalRequest.RegionalRequestID);
                target = RequestViewModelBinder.BindRegionalRequest(regionalRequest, target);

                _regionalRequestService.EditRegionalRequest(target);
                return RedirectToAction("Allocation", "Request", new { id = regionalRequest.RegionalRequestID });
            }


            PopulateLookup(regionalRequest);
            return View(regionalRequest);
        }

        public ActionResult ApproveRequest(int id)
        {
            _regionalRequestService.ApproveRequest(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult New(HRDPSNPPlan hrdpsnpPlan)
        {
            if (ModelState.IsValid)
            {

                var psnphrdPlanInfo = _regionalRequestService.PlanToRequest(hrdpsnpPlan);
              //  RedirectToAction("PreparePlan");
                return View("PreparePlan", psnphrdPlanInfo);
            }
            ViewBag.SeasonID = new SelectList(_commonService.GetSeasons(), "SeasonID", "Name");
            PopulateLookup();
            return View(hrdpsnpPlan);
        }
        public ActionResult PreparePlan(HRDPSNPPlanInfo psnphrdPlanInfo)
        {
            //CretaeRegionalRequest(psnphrdPlanInfo);
            return View(psnphrdPlanInfo);
        }
        [HttpGet]
        public ActionResult RequestFromPlan()
        {// CretaeRegionalRequest(psnphrdPlanInfo);
            return RedirectToAction("New");
        }
        [HttpPost]
        public ActionResult RequestFromPlan(HRDPSNPPlanInfo psnphrdPlanInfo)
        {
            CretaeRegionalRequest(psnphrdPlanInfo);
            ViewBag.message = "Request Created";
            return RedirectToAction("Index");
        }

        #region Regional Request Detail


        public ActionResult Allocation(int id)
        {
            ViewBag.RequestID = id;
            var request =
                _regionalRequestService.Get(t => t.RegionalRequestID == id, null, "AdminUnit,Program").FirstOrDefault();
            var statuses = _commonService.GetStatus(WORKFLOW.REGIONAL_REQUEST);
            var requestModelView = RequestViewModelBinder.BindRegionalRequestViewModel(request, statuses);
            var requestDetails = _regionalRequestDetailService.Get(t => t.RegionalRequestID == id);
            var requestDetailCommodities = (from item in requestDetails select item.RequestDetailCommodities).FirstOrDefault();

            ViewData["AllocatedCommodities"] = (from itm in requestDetailCommodities select new Commodity() { CommodityID = itm.CommodityID });
            ViewData["AvailableCommodities"] = _commonService.GetCommodities();

            return View(requestModelView);
        }
        public ActionResult Details(int id)
        {

            ViewBag.RequestID = id;

            var request =
               _regionalRequestService.Get(t => t.RegionalRequestID == id, null, "AdminUnit,Program,Ration").FirstOrDefault();
           
            if (request == null)
            {
                return HttpNotFound();
            }
            var statuses = _commonService.GetStatus(WORKFLOW.REGIONAL_REQUEST);
            var requestModelView = RequestViewModelBinder.BindRegionalRequestViewModel(request, statuses);

            var requestDetails = _regionalRequestDetailService.Get(t => t.RegionalRequestID == id, null, "RequestDetailCommodities,RequestDetailCommodities.Commodity").ToList();
            var dt = RequestViewModelBinder.TransposeData(requestDetails);
            ViewData["Request_main_data"] = requestModelView;
            return View(dt);
        }
        public ActionResult Details_Read([DataSourceRequest] DataSourceRequest request, int id)
        {

            ViewBag.RequestID = id;
            var requestDetails = _regionalRequestDetailService.Get(t => t.RegionalRequestID == id, null, "FDP,FDP.AdminUnit,FDP.AdminUnit.AdminUnit2,RequestDetailCommodities,RequestDetailCommodities.Commodity").ToList();
            var dt = RequestViewModelBinder.TransposeData(requestDetails);
            return Json(dt.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
  

        public ActionResult Allocation_Read([DataSourceRequest] DataSourceRequest request, int id)
        {

            var requestDetails = _regionalRequestDetailService.FindBy(t => t.RegionalRequestID == id);
            var requestDetailViewModels = (from dtl in requestDetails select BindRegionalRequestDetailViewModel(dtl));
            return Json(requestDetailViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        private RegionalRequestDetailViewModel BindRegionalRequestDetailViewModel(RegionalRequestDetail regionalRequestDetail)
        {
            return new RegionalRequestDetailViewModel()
                       {
                           Beneficiaries = regionalRequestDetail.Beneficiaries,
                           FDP = regionalRequestDetail.Fdp.Name,
                           Fdpid = regionalRequestDetail.Fdpid,
                           RegionalRequestID = regionalRequestDetail.RegionalRequestID,
                           RegionalRequestDetailID = regionalRequestDetail.RegionalRequestDetailID,
                           Woreda = regionalRequestDetail.Fdp.AdminUnit.Name,
                           Zone = regionalRequestDetail.Fdp.AdminUnit.AdminUnit2.Name
                       };

        }
        private RegionalRequestDetail BindRegionalRequestDetail(RegionalRequestDetailViewModel regionalRequestDetailViewModel)
        {
            return new RegionalRequestDetail()
                               {
                                   Beneficiaries = regionalRequestDetailViewModel.Beneficiaries,
                                   Fdpid = regionalRequestDetailViewModel.Fdpid,
                                   RegionalRequestID = regionalRequestDetailViewModel.RegionalRequestID,
                                   RegionalRequestDetailID = regionalRequestDetailViewModel.RegionalRequestDetailID
                               };
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Create([DataSourceRequest] DataSourceRequest request, RegionalRequestDetailViewModel regionalRequestDetailViewModel)
        {
            if (regionalRequestDetailViewModel != null && ModelState.IsValid)
            {
                _regionalRequestDetailService.AddRegionalRequestDetail(BindRegionalRequestDetail(regionalRequestDetailViewModel));
            }

            return Json(new[] { regionalRequestDetailViewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Update([DataSourceRequest] DataSourceRequest request, RegionalRequestDetailViewModel regionalRequestDetail)
        {
            if (regionalRequestDetail != null && ModelState.IsValid)
            {
                var target = _regionalRequestDetailService.FindById(regionalRequestDetail.RegionalRequestDetailID);
                if (target != null)
                {
                    target.Beneficiaries = regionalRequestDetail.Beneficiaries;
                    _regionalRequestDetailService.EditRegionalRequestDetail(target);
                }
            }

            return Json(new[] { regionalRequestDetail }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Destroy([DataSourceRequest] DataSourceRequest request,
                                                  RegionalRequestDetail regionalRequestDetail)
        {
            if (regionalRequestDetail != null)
            {
                _regionalRequestDetailService.DeleteById(regionalRequestDetail.RegionalRequestDetailID);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult Commodity_Read([DataSourceRequest] DataSourceRequest request, int id)
        {

            var requestDetails = _regionalRequestDetailService.Get(t => t.RegionalRequestID == id);
            var requestDetailCommodities = (from item in requestDetails select item.RequestDetailCommodities).FirstOrDefault();

            var commodities = (from itm in requestDetailCommodities select new RequestDetailCommodityViewModel() { CommodityID = itm.CommodityID, RequestDetailCommodityID = itm.RequestCommodityID });
            ViewData["AvailableCommodities"] = _commonService.GetCommodities();

            return Json(commodities.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Commodity_Create([DataSourceRequest] DataSourceRequest request, RequestDetailCommodityViewModel commodity, int id)
        {
            if (commodity != null && ModelState.IsValid)
            {
                //try
                //{
                _regionalRequestDetailService.AddRequestDetailCommodity(commodity.CommodityID, id);
                //}
                //catch(Exception ex)
                //{

                //}
            }

            return Json(new[] { commodity }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Commodity_Update([DataSourceRequest] DataSourceRequest request, RequestDetailCommodityViewModel commodity)
        {
            if (commodity != null && ModelState.IsValid)
            {
                var target = _regionalRequestDetailService.UpdateRequestDetailCommodity(commodity.CommodityID,
                                                                                      commodity.RequestDetailCommodityID);
            }

            return Json(new[] { commodity }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Commodity_Destroy([DataSourceRequest] DataSourceRequest request,
                                                  RequestDetailCommodityViewModel commodity, int id)
        {
            if (commodity != null)
            {
                _regionalRequestDetailService.DeleteRequestDetailCommodity(commodity.CommodityID, id);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        #endregion


        #region Reguest

        public ActionResult Index()
        {
            var regions = _commonService.GetAminUnits(t=>t.AdminUnitTypeID==2);
            ViewData["adminunits"] = regions;
            var programs = _commonService.GetPrograms();
            ViewData["programs"] = programs;
            
            return View();
        }

        public ActionResult Request_Read([DataSourceRequest] DataSourceRequest request)
        {

            var requests = _regionalRequestService.GetAllRegionalRequest();
            var statuses = _commonService.GetStatus(WORKFLOW.REGIONAL_REQUEST);
            var requestViewModels = RequestViewModelBinder.BindRegionalRequestListViewModel(requests, statuses);
            return Json(requestViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        
       
      

     
       
        #endregion

    }








}