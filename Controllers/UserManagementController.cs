using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly AppDbContext _context;

        public UserManagementController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // جلب جميع المستخدمين
            List<Users> Users = _context.Users.ToList();
            return View(Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateSubmit(Users user)
        {
            try
            {
                // إضافة المستخدم الجديد
                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["Message"] = "User Added Successfully!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            // البحث عن المستخدم المراد تعديله
            var user = _context.Users.Find(id);

            if (user != null)
            {
                return View(user);
            }

            TempData["Message"] = "User not found!";
            return RedirectToAction("Index");
        }

        public IActionResult Save(Users user)
        {
            var existingUser = _context.Users.Find(user.Id);

            if (existingUser != null)
            {
                // تحديث بيانات المستخدم
                existingUser.First_Name = user.First_Name;
                existingUser.Last_Name = user.Last_Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Mobile = user.Mobile;
                existingUser.Address = user.Address;

                try
                {
                    _context.SaveChanges();
                    TempData["Message"] = "User Edited Successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"Error: {ex.Message}";
                }
            }
            else
            {
                TempData["Message"] = "Edit Failed! User not found.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);

            if (user != null)
            {
                try
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();

                    TempData["Message"] = "User Deleted!";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"Error: {ex.Message}";
                }
            }
            else
            {
                TempData["Message"] = "Delete Failed! User not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
