using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4AdminPage.Models;

namespace Project4AdminPage.Controllers
{
    [Route("coupons")]
    public class CouponController : Controller
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

                var data = await client.GetStringAsync("api/coupons");

                int start = (page - 1) * 6;
                List<Coupon> coupons = JsonConvert.DeserializeObject<List<Coupon>>(data);
                List<Coupon> datas = coupons.Skip(start).Take(6).ToList();
                int totalPage = coupons.Count() / 6;
                if (coupons.Count() % 4 > 0)
                {
                    totalPage = totalPage + 1;
                }
                ViewBag.totalPage = totalPage;
                ViewBag.currentPage = page;
                return View(datas);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult>Create()
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
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Name","Description","Percent")] string name,string description,int percent)
        {
            Coupon coupon = new Coupon();
            coupon.Name = name;
            coupon.Description = description;
            coupon.Percent = percent;
            coupon.Status = true;
            if (!ModelState.IsValid)
            {
                return NotFound("Không có thông tin !");
            }
            client.BaseAddress = new Uri(host_api);
            var result = await client.PostAsJsonAsync("api/coupons", coupon);

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

                var result = await client.GetStringAsync("api/coupons/" + id);
                Coupon c = JsonConvert.DeserializeObject<Coupon>(result);
                return View(c);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(Coupon c)
        {
            client.BaseAddress = new Uri(host_api);
            var result = await client.PutAsJsonAsync<Coupon>("api/coupons/" + c.Id, c);
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

                await client.DeleteAsync("api/coupons/" + id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Login");
        }

        [Route("search")]
        public async Task<IActionResult> Search(string key)
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

                if (key == "" || key == null)
                {
                    return RedirectToAction("Index");
                }
                var data = await client.GetStringAsync("api/coupons/search?search=" + key);
                List<Coupon> c = JsonConvert.DeserializeObject<List<Coupon>>(data);
                return View("Index", c);
            }
            return RedirectToAction("Login", "Login");
        }
    }
}