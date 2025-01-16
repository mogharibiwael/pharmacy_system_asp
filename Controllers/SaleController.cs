using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Controllers
{
    public class SaleController : Controller
    {
        private readonly AppDbContext _context;

        public SaleController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // جلب قائمة المبيعات من قاعدة البيانات
            var Sales = _context.Sales
                .Select(sale => new Sales
                {
                    sale_id = sale.sale_id,
                    sale_date = sale.sale_date,
                    sale_name = sale.sale_name,
                    sale_type = sale.sale_type,
                    sale_value = sale.sale_value,
                    sale_description = sale.sale_description
                })
                .ToList();

            return View(Sales);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateSubmit(Sales saledetails)
        {
            try
            {
                // إضافة سجل المبيعات إلى قاعدة البيانات
                _context.Sales.Add(saledetails);
                _context.SaveChanges();

                TempData["Message"] = "Sale Record Added Successfully!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var sale = _context.Sales.Find(id);

            if (sale != null)
            {
                try
                {
                    _context.Sales.Remove(sale);
                    _context.SaveChanges();

                    TempData["Message"] = "Sale Deleted!";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error: " + ex.Message;
                }
            }
            else
            {
                TempData["Message"] = "Sale Delete Failed! Sale not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
