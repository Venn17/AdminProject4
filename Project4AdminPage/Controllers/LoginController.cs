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
    public class LoginController : Controller
    {
        string host_api = "http://localhost:50041/";
        HttpClient client = new HttpClient();
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([Bind("name","email","phone","address","password","confirm")] string name,string email,string phone,string address,string password,string confirm)
        {
            List<String> error = new List<String>();
            if(name == "" || name == null)
            {
                error.Add("Tên không được để trống");
            }
            if (email == "" || email == null)
            {
                error.Add("Email không được để trống");
            }
            if (phone == "" || phone == null)
            {
                error.Add("Số điện thoại không được để trống");
            }
            if (address == "" || address == null)
            {
                error.Add("Địa chỉ không được để trống");
            }
            if (password == "" || password == null)
            {
                error.Add("Mật khẩu không được để trống");
            }
            if (confirm != password)
            {
                error.Add("Mật khẩu xác nhận không khớp");
            }
            if(error.Count() == 0)
            {
                client.BaseAddress = new Uri(host_api);
                Users u = new Users();
                u.Name = name;
                u.Email = email;
                u.Phone = phone;
                u.Address = address;
                u.Password = password;
                u.Role = 0;
                var result = await client.PostAsJsonAsync("api/users", u);
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = error.First();
                return View("Register");
            }
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(Login login)
        {
            client.BaseAddress = new Uri(host_api);
            var data = await client.PostAsJsonAsync("api/users/checkLogin",login);
            if(data.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.Message = "Tài Khoản Không Tồn Tại !";
            return View("Login");
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            client.BaseAddress = new Uri(host_api);
            await client.GetStringAsync("api/logineds/logout");
            return RedirectToAction("Index");
        }
    }
}