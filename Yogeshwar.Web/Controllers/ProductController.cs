﻿using Microsoft.AspNetCore.Mvc;

namespace Yogeshwar.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEdit()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}