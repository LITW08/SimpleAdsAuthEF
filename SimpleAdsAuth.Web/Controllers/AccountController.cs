﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using SimpleAdsAuth.Data;
using SimpleAdsAuth.Web.Models;

namespace SimpleAdsAuth.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;
        
        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult EmailAvailable(string email)
        {
            var db = new UserDb(_connectionString);
            bool available = db.IsEmailAvailable(email);
            var vm = new EmailAvailableViewModel { IsAvailable = available };
            return Json(vm);
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var db = new UserDb(_connectionString);
            
            db.AddUser(user, password);
            return Redirect("/account/login");
        }

        public IActionResult Login()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Message = TempData["Error"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new UserDb(_connectionString);
            var user = db.Login(email, password);
            if (user == null)
            {
                TempData["Error"] = "Invalid login!";
                return Redirect("/account/login");
            }

            //this code logs in the current user!
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/newad");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
