﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Areas.Procurement.Models;
using Cats.Helpers;
using Cats.Services.EarlyWarning;
using Cats.Services.Procurement;
using System;
using Cats.Models;
namespace Cats.Areas.Procurement.Controllers
{
    public class BidController : Controller
    {
        // GET: /Procurement/Bid/
        private IBidService _bidService;
        private IBidDetailService _bidDetailService;
        private IAdminUnitService _adminUnitService;
        private IStatusService _statusService;
        private ITransportBidPlanService _transportBidPlanService;
        private ITransportBidPlanDetailService _transportBidPlanDetailService;

        public BidController(IBidService bidService, IBidDetailService bidDetailService,
                             IAdminUnitService adminUnitService,
                             IStatusService statusService,
                             ITransportBidPlanService transportBidPlanService,
                             ITransportBidPlanDetailService transportBidPlanDetailService)
        {
            this._bidService = bidService;
            this._bidDetailService = bidDetailService;
            this._adminUnitService = adminUnitService;
            this._statusService = statusService;
            this._transportBidPlanService = transportBidPlanService;
            this._transportBidPlanDetailService = transportBidPlanDetailService;
        }

        public ActionResult Index()
        {
            var bids = _bidService.Get(m => m.StatusID== 1);
            return View(bids.ToList());
        }
       
        [HttpGet]
        public ActionResult Index(string bidNumber="", bool open = true, bool closed = false, bool canceled = false, bool approved = false)
        {
            var allStatusTypes = _statusService.GetAllStatus().Select(m => m.Name).ToList();
            ViewBag.BidStatusTypes = allStatusTypes;
            
            var statusTypesList = "";
            if (open) statusTypesList += "Open;";
            if (closed) statusTypesList += "Closed;";
            if (canceled) statusTypesList += "Canceled;";
            if (approved) statusTypesList += "Approved;";

            var listOfStatusTypes = statusTypesList.Split(';');

            var filteredBids = _bidService.Get(b => b.BidNumber.StartsWith(bidNumber) &&(listOfStatusTypes.Contains(b.Status.Name)));
            return View(filteredBids.ToList());
        }
        
        public ActionResult Create(int id=0)
        {
            
            var bid = new Bid();
           
            var regions = _adminUnitService.FindBy(t => t.AdminUnitTypeID == 2);
            ViewBag.StatusID = new SelectList(_statusService.GetAllStatus(), "StatusID", "Name",bid.StatusID=1);
            var bidDetails = (from detail in regions
                              select new BidDetail()
                              {
                                          RegionID = detail.AdminUnitID,
                                          AmountForReliefProgram = 0,
                              }).ToList();
            bid.BidDetails = bidDetails;
                ViewBag.BidPlanID = id;
                ViewBag.TransportBidPlanID = new SelectList(_transportBidPlanService.GetAllTransportBidPlan(), "TransportBidPlanID", "ShortName", id);
            return View(bid);
        }

        [HttpPost]
        public ActionResult Create(Bid bid)
        {
            if(ModelState.IsValid)
            {
                var regions = _adminUnitService.FindBy(t => t.AdminUnitTypeID == 2);
                var bidDetails = (from detail in regions
                                  select new BidDetail()
                                      {
                                          RegionID = detail.AdminUnitID,
                                          AmountForReliefProgram =(decimal) _transportBidPlanDetailService.GetRegionPlanTotal(bid.TransportBidPlanID,detail.AdminUnitID,1),
                                          AmountForPSNPProgram = (decimal)_transportBidPlanDetailService.GetRegionPlanTotal(bid.TransportBidPlanID, detail.AdminUnitID, 2),
                                          BidDocumentPrice = 0,
                                          CPO = 0,

                                      }).ToList();
                bid.BidDetails = bidDetails;
                bid.StatusID = 1;
                _bidService.AddBid(bid);
                return RedirectToAction("Edit", "Bid", new {id = bid.BidID});
            }
            ViewBag.StatusID = new SelectList(_statusService.GetAllStatus(), "StatusID", "Name");
            ViewBag.BidPlanID = bid.TransportBidPlanID;
            ViewBag.TransportBidPlanID = new SelectList(_transportBidPlanService.GetAllTransportBidPlan(), "TransportBidPlanID", "ShortName", bid.TransportBidPlanID);
                
            return View(bid);
        }

        public ActionResult Edit(int id)
        {
            
            var bid = _bidService.Get(m => m.BidID == id, null, "BidDetails").FirstOrDefault();
            ViewBag.BidNumber = bid.BidNumber;
            ViewBag.StartDate = bid.StartDate;
            ViewBag.EndDate = bid.EndDate;
            ViewBag.OpeningDate = bid.OpeningDate;
            var bidDetails = bid.BidDetails;
            var input = ( from detail in bidDetails
                          select new BidDetailsViewModel
                              {
                                  BidDetailID=detail.BidDetailID,
                                  BidID = detail.BidID,
                                  Region=detail.AdminUnit.Name,
                                  Edit=new BidDetailsViewModel.BidDetailEdit()
                                      {
                                          Number=detail.BidDetailID,
                                          AmountForReliefProgram=detail.AmountForReliefProgram,
                                          AmountForPSNPProgram=detail.AmountForPSNPProgram,
                                          BidDocumentPrice=detail.BidDocumentPrice,
                                          CPO=detail.CPO,
                                          AmountForReliefProgramPlanned = (decimal)_transportBidPlanDetailService.GetRegionPlanTotal(bid.TransportBidPlanID, detail.AdminUnit.AdminUnitID, 1),
                                          AmountForPSNPProgramPlanned = (decimal)_transportBidPlanDetailService.GetRegionPlanTotal(bid.TransportBidPlanID, detail.AdminUnit.AdminUnitID, 2)

                                      } 
                              }
                        );
            
            return View(input);
        }

        [HttpPost]
        public ActionResult Edit(List<BidDetailsViewModel.BidDetailEdit> input)
        {
            var bidId = 0;
            foreach (var bidDetailEdit in input)
            {
                var bidDetail = _bidDetailService.FindById(bidDetailEdit.Number);
                bidId = bidDetail.BidID;
                bidDetail.AmountForReliefProgram = bidDetailEdit.AmountForReliefProgram;
                bidDetail.AmountForPSNPProgram = bidDetailEdit.AmountForPSNPProgram;
                bidDetail.BidDocumentPrice = bidDetailEdit.BidDocumentPrice;
                bidDetail.CPO = bidDetailEdit.CPO;
            }
            _bidDetailService.Save();
            return RedirectToAction("Edit", "Bid", new {id = bidId});
        }

        public ViewResult Details(int id=0)
        {
            Bid bid = _bidService.Get(t => t.BidID == id, null, "BidDetails").FirstOrDefault();
            ViewBag.BidStatus = new SelectList(_statusService.GetAllStatus(), "StatusID", "Name",bid.StatusID);
            return View(bid);
            
        }
        public ActionResult EditBidStatus(int id)
        {
            Bid bid = _bidService.Get(t => t.BidID == id, null, "BidDetails").FirstOrDefault();
            ViewBag.StatusID = new SelectList(_statusService.GetAllStatus(), "StatusID", "Name",bid.StatusID);
            ViewBag.TransportBidPlanID = new SelectList(_transportBidPlanService.GetAllTransportBidPlan(),
                                                        "TransportBidPlanID", "ShortName", bid.TransportBidPlanID);
            return View(bid);
        }
        [HttpPost]
        public ActionResult EditBidStatus(Bid bid)
        {
            if (ModelState.IsValid)
            {
                _bidService.EditBid(bid);
                return RedirectToAction("Index");
            }
            ViewBag.StatusID = new SelectList(_statusService.GetAllStatus(), "StatusID", "Name", bid.StatusID);
            ViewBag.TransportBidPlanID = new SelectList(_transportBidPlanService.GetAllTransportBidPlan(),
                                                        "TransportBidPlanID", "ShortName", bid.TransportBidPlanID);
            return View(bid);
        }

        public ActionResult ApproveBid(int id)
        {
             var bid = _bidService.FindById(id);
             bid.StatusID = 4;
             _bidService.EditBid(bid);
             return RedirectToAction("Index");
        } 
    }
}