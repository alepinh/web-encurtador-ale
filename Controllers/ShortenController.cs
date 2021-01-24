using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web_encurtador_ale.Models;

namespace web_encurtador_ale.Controllers
{
    public class ShortenController : Controller
    {
        private HttpContext _url_Long;

       
        public ShortenController(HttpContext Url_Long)
        {
            _url_Long = Url_Long;
        }

        public IActionResult Index()
        {
            HandleShortener.HandleShortenUrl(_url_Long);
            return View();
        }
    }
}