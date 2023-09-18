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
    [Route("sizes")]
    public class SizeController : Controller
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

                var data = await client.GetStringAsync("api/sizes");
                int start = (page - 1) * 5;
                List<Sizes> sizes = JsonConvert.DeserializeObject<List<Sizes>>(data);
                List<Sizes> data_page = sizes.Skip(start).Take(5).ToList();
                int totalPage = sizes.Count() / 5;
                if (sizes.Count() % 5 > 0)
                {
                    totalPage = totalPage + 1;
                }
                ViewBag.totalPage = totalPage;
                ViewBag.currentPage = page;
                var product = await client.GetStringAsync("api/products");
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(product);
                ViewBag.Product = products;
                return View(data_page);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create()
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

                var product = await client.GetStringAsync("api/products");
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(product);
                ViewBag.Product = products;
                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Name", "ProductID")] string name, int productID)
        {
            Sizes s = new Sizes();
            s.Name = name;
            s.ProductID = productID;
            if (!ModelState.IsValid)
            {
                return NotFound("Không có thông tin !");
            }
            client.BaseAddress = new Uri(host_api);
            var result = await client.PostAsJsonAsync("api/sizes", s);

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

                var result = await client.GetStringAsync("api/sizes/" + id);
                Sizes c = JsonConvert.DeserializeObject<Sizes>(result);
                var product = await client.GetStringAsync("api/products");
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(product);
                ViewBag.Product = products;
                return View(c);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(Sizes c)
        {
            client.BaseAddress = new Uri(host_api);
            var result = await client.PutAsJsonAsync<Sizes>("api/sizes/" + c.Id, c);
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

                await client.DeleteAsync("api/sizes/" + id);
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

                var data = await client.GetStringAsync("api/sizes");
                if (key == "" || key == null)
                {
                    if (productID == 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        data = await client.GetStringAsync("api/sizes/search?productId=" + productID);
                    }
                }
                else
                {
                    if (productID == 0)
                    {
                        data = await client.GetStringAsync("api/sizes/search?search=" + key);
                    }
                    else
                    {
                        data = await client.GetStringAsync("api/sizes/search?search=" + key + "&productId=" + productID);
                    }
                }
                List<Sizes> c = JsonConvert.DeserializeObject<List<Sizes>>(data);

                var product = await client.GetStringAsync("api/products");
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(product);
                ViewBag.Product = products;
                return View("Index", c);
            }
            return RedirectToAction("Login", "Login");
        }
    }
}