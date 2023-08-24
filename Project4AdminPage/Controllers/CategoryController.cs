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
    [Route("categories")]
    public class CategoryController : Controller
    {
        string host_api = "http://localhost:50041/";
        HttpClient client = new HttpClient();

        public async Task<IActionResult> Index()
        {
            client.BaseAddress = new Uri(host_api);
            var data = await client.GetStringAsync("api/categories");
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(data);
            return View(categories);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
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
            var result = await client.GetStringAsync("api/categories/" + id);
            Category category = JsonConvert.DeserializeObject<Category>(result);
            return View(category);
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
            await client.DeleteAsync("api/categories/" + id);
            return RedirectToAction("Index");
        }

        [Route("search")]
        public async Task<IActionResult> Search(string key)
        {
            if (key == "" || key == null)
            {
                return RedirectToAction("Index");
            }
            client.BaseAddress = new Uri(host_api);
            var data = await client.GetStringAsync("api/categories/search?search="+ key);
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(data);
            return View("Index",categories);
        }

    }
}