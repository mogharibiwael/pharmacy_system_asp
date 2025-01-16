using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using System.Linq;


namespace PMS.Controllers
{
    public class CompanyController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public CompanyController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }
        public IActionResult Index()
        {   
            List<Company> company = _appDbContext.Companies.ToList();

            return View(company);
        }


        public IActionResult Delete(int id)
        {
            // البحث عن الكيان (Company) المطلوب حذفه
            var company = _appDbContext.Companies.FirstOrDefault(c => c.Id == id);

            if (company != null)
            {
                // حذف الكيان من قاعدة البيانات
                _appDbContext.Companies.Remove(company);
                _appDbContext.SaveChanges(); // حفظ التغييرات في قاعدة البيانات

                TempData["Message"] = "Company Deleted!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Delete Failed! Company not found.";
                return RedirectToAction("Index");
            }
        }


        public IActionResult Create()
        {
            return View();
        }


        public IActionResult CreateSubmit(Company company)
        {
            try
            {
                // إضافة الكيان الجديد إلى قاعدة البيانات
                _appDbContext.Companies.Add(company);
                _appDbContext.SaveChanges();

                TempData["Message"] = "Company Added Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // في حالة وجود خطأ
                TempData["Message"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        public IActionResult Edit(int id)
        {
            // البحث عن الشركة بواسطة المفتاح الأساسي
            var company = _appDbContext.Companies.Find(id);

            if (company != null)
            {
                return View(company); // إرسال بيانات الشركة إلى العرض
            }
            else
            {
                TempData["Message"] = "Company not found!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Save(Company company)
        {
            try
            {
                // البحث عن الشركة الموجودة
                var existingCompany = _appDbContext.Companies.Find(company.Id);

                if (existingCompany != null)
                {
                    // تحديث بيانات الشركة
                    existingCompany.Contact = company.Contact;
                    existingCompany.Address = company.Address;

                    _appDbContext.SaveChanges();

                    TempData["Message"] = "Data Edited Successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Edit Failed! Company not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


    }
}
