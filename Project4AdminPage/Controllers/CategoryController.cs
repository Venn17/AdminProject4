﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4AdminPage.Models;

namespace Project4AdminPage.Controllers
{
    [Route("categories")]
    public class CategoryController : Controller
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

                var data = await client.GetStringAsync("api/categories");
                int start = (page - 1) * 5;
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(data);
                List<Category> datas = categories.Skip(start).Take(5).ToList();
                int totalPage = categories.Count() / 5;
                if (categories.Count() % 5 > 0)
                {
                    totalPage = totalPage + 1;
                }
                ViewBag.totalPage = totalPage;
                ViewBag.currentPage = page;
                return View(datas);
            }
            return RedirectToAction("Login","Login");
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

                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Name")] string name)
        {
            Category category = new Category();
            category.Name = name;
            category.Status = true;
            if (!ModelState.IsValid)
            {
                return NotFound("Không có thông tin !");
            }
            client.BaseAddress = new Uri(host_api);
            var result = await client.PostAsJsonAsync("api/categories", category);

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

                var result = await client.GetStringAsync("api/categories/" + id);
                Category category = JsonConvert.DeserializeObject<Category>(result);
                return View(category);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(Category category)
        {
            client.BaseAddress = new Uri(host_api);
            var result = await client.PutAsJsonAsync<Category>("api/categories/" + category.Id,category);
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

                await client.DeleteAsync("api/categories/" + id);
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

                var data = await client.GetStringAsync("api/categories/search?search=" + key);
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(data);
                return View("Index", categories);
            }
            return RedirectToAction("Login", "Login");
        }

    }
}