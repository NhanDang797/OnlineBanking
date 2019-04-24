using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBanking.Common;
using OnlineBanking.Dao;
using OnlineBanking.Models;

namespace OnlineBanking.Controllers
{
    public class ProfileController : Controller
    {
        OnlineBankingDbContext db = new OnlineBankingDbContext();

        // GET: Profile
        public ActionResult Profile()
        {
            ViewBag.Title = "Profile's customer";
            //Check session
            if (Session["Cus_Session"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //Show profile
            int id = Convert.ToInt32(Session["Cus_Session"]);
            var c = new CustomerDao().Detail(id);
            ViewBag.FirstName = c.FirstName;
            ViewBag.Lastname = c.LastName;
            ViewBag.Email = c.Email;
            ViewBag.Phone = c.PhoneNumber;
            if (c.Gender.ToString().Equals("True"))
            {
                ViewBag.Gender = "Male";
            }
            else
            {
                ViewBag.Gender = "Female";
            }
            ViewBag.Address = c.Address;
            TempData["address"] = c.Address;

            return View();
        }


        // GET: Update
        public ActionResult Update()
        {
            ViewBag.Address = TempData["address"];
            return View();
        }

        // Post: Update
        [HttpPost]
        public ActionResult Update(ProfileModel cus)
        {
            ViewBag.Title = "Update profile";
            //Check session
            if (Session["Cus_Session"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var cusDao = new CustomerDao();
            int id = Convert.ToInt32(Session["Cus_Session"]);
            var c = new CustomerDao().Detail(id);
            ViewBag.Address = c.Address;
            if (ModelState.IsValid)
            {
                //Update profiler
                bool result = cusDao.UpdateProfile(id, cus.Address, Encryptor.MD5Hash(cus.LoginPassword), Encryptor.MD5Hash(cus.ConfirmPassword), Encryptor.MD5Hash(cus.TransactionPassword), Encryptor.MD5Hash(cus.ConfirmTransactionPassword));
                if (result)
                {
                    TempData["msg"] = "<script>alert('Change succesfully');</script>";
                    return View();
                }
            }

            return View(cus);
        }
    }
}