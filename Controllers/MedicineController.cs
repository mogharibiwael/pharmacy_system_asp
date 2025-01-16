using Microsoft.AspNetCore.Mvc;
using PMS.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using DataAccess.Data;

namespace PMS.Controllers
{
    public class MedicineController : Controller
    {
        private readonly AppDbContext _context;

        public MedicineController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Console.WriteLine("======================dddddd=========");

            var medlist = _context.Medicines
                .Join(_context.Companies,
                      medicine => medicine.Manufacture_ID,
                      company => company.Id,
                      (medicine, company) => new Medicine
                      {
                          Id = medicine.Id,
                          Name = medicine.Name,
                          Manufacture_ID = company.Id,
                          Manufacture = company.Name,
                          Unit = medicine.Unit,
                          Type = medicine.Type,
                          Description = medicine.Description
                      })
                .ToList();

            foreach (var item in medlist)
            {
                Console.WriteLine($"Medicine: {item.Name}, Manufacture: {item.Manufacture}");
            }

            return View(medlist);
        }

        public IActionResult Delete(int id)
        {
            var medicine = _context.Medicines.Find(id);

            if (medicine != null)
            {
                _context.Medicines.Remove(medicine);
                _context.SaveChanges();

                TempData["Message"] = "Medicine Data Deleted!";
            }
            else
            {
                TempData["Message"] = "Delete Failed! Medicine not found.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateSubmit(Medicine medicine)
        {
            try
            {
                _context.Medicines.Add(medicine);
                _context.SaveChanges();

                TempData["Message"] = "Medicine Data Added Successfully!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var medicine = _context.Medicines.Find(id);

            if (medicine != null)
            {
                return View(medicine);
            }

            TempData["Message"] = "Medicine not found!";
            return RedirectToAction("Index");
        }

        public IActionResult Save(Medicine medicine)
        {
            var existingMedicine = _context.Medicines.Find(medicine.Id);

            if (existingMedicine != null)
            {
                existingMedicine.Unit = medicine.Unit;

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
                TempData["Message"] = "Edit Failed! Medicine not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
