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
using System.Text.Encodings.Web;

namespace web_encurtador_ale.Controllers
{
    public class ShortenController : Controller
    {
        //private HttpContext _url_Long = null;

       
        public ShortenController()
        {
           // _url_Long = Url_Long;
        }

        public ActionResult<string> Shortening()
        {

            //var https = HttpContext.Request.IsHttps;
            //var caminho = HttpContext.Request.Path;
            //var status = HttpContext.Response.StatusCode;
            //var conexao = HttpContext.Connection.ToString();
            //string teste = https + "\r\n" + caminho + "\r\n" + status + "\r\n" + conexao;

            //return teste;


            HandleShortener.HandleShortenUrl(HttpContext);
            return View();
        }
    }
}