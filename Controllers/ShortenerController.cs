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
    public class ShortenerController : Controller
    {
        private readonly ILogger<ShortenerController> _logger;

        public ShortenerController(ILogger<ShortenerController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Shorten()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult shorten(HttpContext contex)
        {
            return (IActionResult) HandleShortenUrl(contex);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static Task HandleShortenUrl(HttpContext context)
        {

            // Perform basic form validation
            if (!context.Request.HasFormContentType || !context.Request.Form.ContainsKey("url"))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return context.Response.WriteAsync("Cannot process request");
            }

            context.Request.Form.TryGetValue("url", out var formData);
            var requestedUrl = formData.ToString();

            // Test our URL
            if (!Uri.TryCreate(requestedUrl, UriKind.Absolute, out Uri result))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return context.Response.WriteAsync("Could not understand URL.");
            }

            var url = result.ToString();
            // Ask for LiteDB and persist a short link
           // var liteDB = context.RequestServices.GetService<ILiteDatabase>();
           // var links = liteDB.GetCollection<ShortLink>(BsonAutoId.Int32);

            // Temporary short link 
            var entry = new ShortLink
            {
                Url = url
            };

            // Insert our short-link
            //links.Insert(entry);

            
            var urlChunk = entry.GetUrlChunk();
            var responseUri = $"{context.Request.Scheme}://{context.Request.Host}/{urlChunk}";
            context.Response.Redirect($"/#{responseUri}");
            return Task.CompletedTask;

        }
    }
}
