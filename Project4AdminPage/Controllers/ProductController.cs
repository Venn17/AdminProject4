using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4AdminPage.Models;

namespace Project4AdminPage.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {

        string host_api = "http://localhost:50041/";
        HttpClient client = new HttpClient();

        //Firebase
        //private static string ApiKey = "AIzaSyC8wSuH-Kv7IyTWuKwrFa31Sv0hlFkSIAs";
        //private static string Bucket = "reactapi-3e80d.appspot.com";
        //private static string AuthEmail = "vienavtb@gmail.com";
        //private static string AuthPassword = "Congvien2002";

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

                var data = await client.GetStringAsync("api/products");
                int start = (page - 1) * 4;
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(data);
                List<Product> datas = products.Skip(start).Take(4).ToList();
                int totalPage = products.Count() / 4;
                if (products.Count() % 4 > 0)
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

                var data = await client.GetStringAsync("api/categories");
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(data);
                ViewBag.Category = categories;
                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Name,Price,SalePrice,Image,Description,CategoryID")] Product p)
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
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", fileName);
                    fileStream = new FileStream(path, FileMode.Create);
                    data.CopyTo(fileStream);
                    p.Image = fileName;
                    p.Sold = 0;
                    var result = await client.PostAsJsonAsync("api/products", p);
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

                var result = await client.GetStringAsync("api/products/" + id);
                Product c = JsonConvert.DeserializeObject<Product>(result);
                var data = await client.GetStringAsync("api/categories");
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(data);
                ViewBag.Category = categories;
                return View(c);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(Product p)
        {
            client.BaseAddress = new Uri(host_api);
            if (ModelState.IsValid)
            {
                FileStream fileStream;
                var file = HttpContext.Request.Form.Files;
                if(file.Count == 0)
                {
                    var getProduct = await client.GetStringAsync("api/products/" + p.Id);
                    Product pro = JsonConvert.DeserializeObject<Product>(getProduct);
                    p.Image = pro.Image;
                }
                else
                {
                    var data = file[0];
                    var fileName = data.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", fileName);
                    fileStream = new FileStream(path, FileMode.Create);
                    data.CopyTo(fileStream);
                    p.Image = fileName;
                }
                var result = await client.PutAsJsonAsync<Product>("api/products/" + p.Id, p);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> Detail(int? id)
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

                var result = await client.GetStringAsync("api/products/" + id);
                Product data = JsonConvert.DeserializeObject<Product>(result);
                var cate = await client.GetStringAsync("api/categories");
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(cate);
                ViewBag.Category = categories;
                return View(data);
            }
            return RedirectToAction("Login", "Login");
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

                await client.DeleteAsync("api/products/" + id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Login");
        }

        [Route("search")]
        public async Task<IActionResult> Search(string key)
        {
            if (key == "" || key == null)
            {
                return RedirectToAction("Index");
            }
            client.BaseAddress = new Uri(host_api);
            var _data = await client.GetStringAsync("api/logineds");
            List<Logined> logined = JsonConvert.DeserializeObject<List<Logined>>(_data);
            if (logined.Count() > 0)
            {
                var cus = logined[0];
                var user = await client.GetStringAsync("api/logineds/" + cus.UserID);
                Users u = JsonConvert.DeserializeObject<Users>(user);
                ViewBag.Logined = u;

                var data = await client.GetStringAsync("api/products/search?search=" + key);
                List<Product> c = JsonConvert.DeserializeObject<List<Product>>(data);
                return View("Index", c);
            }
            return RedirectToAction("Login", "Login");
        }

        [Route("sort")]
        public async Task<IActionResult> Sort(string sort,string type)
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

                if (sort == "" || sort == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (type == "" || type == null)
                    {
                        type = "ASC";
                    }
                }
                var data = await client.GetStringAsync("api/products/sort?sort=" + sort + "&type=" + type);
                List<Product> c = JsonConvert.DeserializeObject<List<Product>>(data);
                return View("Index", c);
            }
            return RedirectToAction("Login", "Login");
        }

        //[HttpGet("upload")]
        //public async void UpLoad(FileStream stream,string fileName)
        //{
        //    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
        //    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

        //    var cancellation = new CancellationTokenSource();

        //    var task = new FirebaseStorage(
        //        Bucket,
        //        new FirebaseStorageOptions
        //        {
        //            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
        //            ThrowOnCancel = true
        //        })
        //        .Child("images")
        //        .Child(fileName)
        //        .PutAsync(stream, cancellation.Token);

        //    try
        //    {
        //        string link = await task;
        //    }catch(Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //}
    }
}