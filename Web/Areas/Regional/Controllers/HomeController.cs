﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Services.Dashboard;

namespace Cats.Areas.Regional.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Regional/Home/
        public ActionResult Index()
        {
            ViewBag.RegionID = 2;
            ViewBag.RegionName = "Afar";
            return View();
        }

       
    }
}