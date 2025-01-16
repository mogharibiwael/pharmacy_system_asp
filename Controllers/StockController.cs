using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Controllers
{
    public class StockController : Controller
    {
        private readonly AppDbContext _context;

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // جلب قائمة المخزون مع الانضمام إلى جدول الأدوية
            var stocklist = _context.Stocks
                .Join(
                    _context.Medicines,
                    stock => stock.MedicineID,
                    medicine => medicine.Id,
                    (stock, medicine) => new Stock
                    {
                        Id = stock.Id,
                        MedicineID = stock.MedicineID,
                        MedicineName = medicine.Name,
                        Quantity = stock.Quantity,
                        Capacity = stock.Capacity
                    }
                ).ToList();

            return View(stocklist);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateSubmit(Stock stock)
        {
            try
            {
                _context.Stocks.Add(stock);
                _context.SaveChanges();

                TempData["Message"] = "Stock Data Added Successfully!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock != null)
            {
                return View(stock);
            }

            TempData["Message"] = "Stock not found!";
            return RedirectToAction("Index");
        }

        public IActionResult Save(Stock stock)
        {
            var existingStock = _context.Stocks.Find(stock.Id);

            if (existingStock != null)
            {
                existingStock.Quantity = stock.Quantity;

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
                TempData["Message"] = "Edit Failed! Stock not found.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var stock = _context.Stocks.Find(id);

            if (stock != null)
            {
                try
                {
                    _context.Stocks.Remove(stock);
                    _context.SaveChanges();
                    TempData["Message"] = "Stock Record Deleted!";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error: " + ex.Message;
                }
            }
            else
            {
                TempData["Message"] = "Delete Failed! Stock not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
