using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Controllers
{
    public class RetailController : Controller
    {
        private readonly AppDbContext _context;

        public RetailController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // جلب قائمة البيع بالتجزئة
            List<Retail> retailer = _context.Retails.ToList();
            return View(retailer);
        }

        public IActionResult Delete(int id)
        {
            var retail = _context.Retails.Find(id);

            if (retail != null)
            {
                _context.Retails.Remove(retail);
                _context.SaveChanges();

                TempData["Message"] = "Retailer Deleted!";
            }
            else
            {
                TempData["Message"] = "Delete Failed! Retailer not found.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateSubmit(Retail retail)
        {
            try
            {
                _context.Retails.Add(retail);
                _context.SaveChanges();

                TempData["Message"] = "Retailer Added Successfully!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var retail = _context.Retails.Find(id);

            if (retail != null)
            {
                return View(retail);
            }

            TempData["Message"] = "Retailer not found!";
            return RedirectToAction("Index");
        }

        public IActionResult Save(Retail retail)
        {
            var existingRetail = _context.Retails.Find(retail.Id);

            if (existingRetail != null)
            {
                existingRetail.Tel = retail.Tel;
                existingRetail.Address = retail.Address;

                try
                {
                    _context.SaveChanges();
                    TempData["Message"] = "Data Edited Successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error: " + ex.Message;
                }
            }
            else
            {
                TempData["Message"] = "Edit Failed! Retailer not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
