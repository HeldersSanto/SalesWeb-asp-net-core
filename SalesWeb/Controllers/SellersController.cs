﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWeb.Services;

namespace SalesWeb.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _selerService;

        public SellersController(SellerService ss)
        {
            _selerService = ss;
        }
        public IActionResult Index()
        {
            var list = _selerService.FindAll();
            return View(list);
        }
    }
}