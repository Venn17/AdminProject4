using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4AdminPage.Models;

namespace Project4AdminPage.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        string host_api = "http://localhost:50041/";
        HttpClient client = new HttpClient();

        public async Task<IActionResult> Index(int page = 1)
        {
            client.BaseAddress = new Uri(host_api);
            var _data = await client.GetStringAsync("api/logineds");
            List<Logined> logined = JsonConvert.DeserializeObject<List<Logined>>(_data);
            if (logined.Count() > 0)
            {
                var cus = logined[0];
                var user = await client.GetStringAsync("api/logineds/" + cus.UserID);
                Users u = JsonConvert.DeserializeObject<Users>(user);
                ViewBag.Logined = u;

                var data = await client.GetStringAsync("api/users");
                int start = (page - 1) * 5;
                List<Users> users = JsonConvert.DeserializeObject<List<Users>>(data);
                List<Users> data_page = users.Skip(start).Take(5).ToList();
                int totalPage = users.Count() / 5;
                if (users.Count() % 5 > 0)
                {
                    totalPage = totalPage + 1;
                }
                ViewBag.totalPage = totalPage;
                ViewBag.currentPage = page;
                return View(data_page);
            }
            return RedirectToAction("Login", "Login");
        }

        
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Name", "Email","Phone","Address","Password","Avatar")] Users u)
        {
            client.BaseAddress = new Uri(host_api);
            if (ModelState.IsValid)
            {
                FileStream fileStream;
                var file = HttpContext.Request.Form.Files;
                if (file != null && file[0].Length > 0)
                {
                    var data = file[0];
                    var fileName = data.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", fileName);
                    fileStream = new FileStream(path, FileMode.Create);
                    data.CopyTo(fileStream);
                    u.Avatar = fileName;
                    u.Role = 0;
                    var result = await client.PostAsJsonAsync("api/users", u);
                }
            }
            return RedirectToAction("Index");
        }

        [Route("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            client.BaseAddress = new Uri(host_api);
            var _data = await client.GetStringAsync("api/logineds");
            List<Logined> logined = JsonConvert.DeserializeObject<List<Logined>>(_data);
            if (logined.Count() > 0)
            {
                var cus = logined[0];
                var user = await client.GetStringAsync("api/logineds/" + cus.UserID);
                Users u = JsonConvert.DeserializeObject<Users>(user);
                ViewBag.Logined = u;

                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(Users c)
        {
            client.BaseAddress = new Uri(host_api);
            if (ModelState.IsValid)
            {
                FileStream fileStream;
                var file = HttpContext.Request.Form.Files;
                if (file.Count == 0)
                {
                    var getUser = await client.GetStringAsync("api/users/" + c.Id);
                    Users old_user = JsonConvert.DeserializeObject<Users>(getUser);
                    c.Avatar = old_user.Avatar;
                }
                else
                {
                    var data = file[0];
                    var fileName = data.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", fileName);
                    fileStream = new FileStream(path, FileMode.Create);
                    data.CopyTo(fileStream);
                    c.Avatar = fileName;
                }
                var result = await client.PutAsJsonAsync<Users>("api/users/" + c.Id, c);
            }
            return RedirectToAction("Index");
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            client.BaseAddress = new Uri(host_api);
            var _data = await client.GetStringAsync("api/logineds");
            List<Logined> logined = JsonConvert.DeserializeObject<List<Logined>>(_data);
            if (logined.Count() > 0)
            {
                var cus = logined[0];
                var user = await client.GetStringAsync("api/logineds/" + cus.UserID);
                Users u = JsonConvert.DeserializeObject<Users>(user);
                ViewBag.Logined = u;

                await client.DeleteAsync("api/users/" + id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Login");
        }

        [Route("search")]
        public async Task<IActionResult> Search(string key, int productID)
        {
            client.BaseAddress = new Uri(host_api);
            var _data = await client.GetStringAsync("api/logineds");
            List<Logined> logined = JsonConvert.DeserializeObject<List<Logined>>(_data);
            if (logined.Count() > 0)
            {
                var cus = logined[0];
                var user = await client.GetStringAsync("api/logineds/" + cus.UserID);
                Users u = JsonConvert.DeserializeObject<Users>(user);
                ViewBag.Logined = u;

                var data = await client.GetStringAsync("api/users");
                if (key == "" || key == null)
                {
                    data = await client.GetStringAsync("api/users/search?search=" + key);
                }
                List<Users> c = JsonConvert.DeserializeObject<List<Users>>(data);
                return View("Index", c);
            }
            return RedirectToAction("Login", "Login");
        }
    }
}