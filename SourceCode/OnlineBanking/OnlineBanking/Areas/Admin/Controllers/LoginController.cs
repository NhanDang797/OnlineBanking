using OnlineBanking.Areas.Admin.Dao;
using OnlineBanking.Areas.Admin.Models;
using OnlineBanking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBanking.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminModel model) {
            if (ModelState.IsValid) {
                var dao = new LoginDao();

                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result) {
                    var user = dao.GetUserByName(model.UserName);
                    var userSession = new AdminModel();
                    userSession.UserName = user.LoginName;

                    Session.Add("ADMIN_SESSION", userSession);

                    return RedirectToAction("Index", "Admin");
                }
               
            }
            ModelState.AddModelError("", "Login Failed");
            return View("Login");

        }

    }
}