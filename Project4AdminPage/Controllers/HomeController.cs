using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4AdminPage.Models;

namespace Project4AdminPage.Controllers
{
    public class HomeController : Controller
    {
        string host_api = "http://localhost:50041/";
        HttpClient client = new HttpClient();
        public async Task<IActionResult> Index()
        {
            client.BaseAddress = new Uri(host_api);
            var data = await client.GetStringAsync("api/logineds");
            List<Logined> logined = JsonConvert.DeserializeObject<List<Logined>>(data);
            if(logined.Count() > 0)
            {
                var cus = logined[0];
                var user = await client.GetStringAsync("api/logineds/"+cus.UserID);
                Users u = JsonConvert.DeserializeObject<Users>(user);
                ViewBag.Logined = u;
                return View(u);
            }
            return View("~/Views/Login/Login.cshtml");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
