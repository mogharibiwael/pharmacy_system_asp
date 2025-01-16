using DataAccess.Data;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace PMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

     

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Auth(string email, string pwd)
        {
            try
            {
                // البحث عن المستخدم بناءً على البريد الإلكتروني وكلمة المرور
                var user = _appDbContext.Users
                    .FirstOrDefault(u => u.Email == email && u.Password == pwd);

                if (user != null)
                {
                    // إذا كان المستخدم موجودًا، يتم إنشاء الجلسة أو أي معالجة إضافية
                    string sessionValue = user.Email; // مثال على الجلسة
                    return RedirectToAction("Home");
                }
                else
                {
                    TempData["Message"] = "User Email or Password is Incorrect!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        public IActionResult Home()
        {
            var dashdata = new DashboardData
            {
                SaleCount = _appDbContext.Sales.Count(),
                StockCount = _appDbContext.Stocks.Count(),
                RetailCount = _appDbContext.Retails.Count(),
                Company = _appDbContext.Companies.Count(),
                RegisteredMed = _appDbContext.Medicines.Count(),
                UserCount = _appDbContext.Users.Count()
            };

            return View(dashdata);
        }

        public IActionResult UserManager()
        {
            List<Users> Users = _appDbContext.Users.ToList();

            return View(Users);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int GetSaleCount()
        {
            try
            {
                // حساب عدد السجلات في جدول المبيعات باستخدام LINQ
                return _appDbContext.Sales.Count();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                return 0;
            }
        }

        public int GetStockCount()
        {
            try
            {
                return _appDbContext.Stocks.Count();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                return 0;
            }
        }


        public int GetRetailCount()
        {
            try
            {
                return _appDbContext.Retails.Count();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                return 0;
            }
        }

        public int GetCompanyCount()
        {
            try
            {
                return _appDbContext.Companies.Count();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                return 0;
            }
        }


        public int GetMedCount()
        {
            try
            {
                return _appDbContext.Medicines.Count();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                return 0;
            }
        }


        public int GetUserCount()
        {
            try
            {
                return _appDbContext.Users.Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

    }
}
