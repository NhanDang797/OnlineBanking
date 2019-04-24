using OnlineBanking.Dao;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OnlineBanking.Common;
using OnlineBanking.Models;

namespace OnlineBanking.Controllers
{
    public class LoginController : Controller
    {
        //Add "block"
        int block;

        // GET: Login
        public ActionResult Login()
        {
            //Create a code captcha
            Session["Captcha"] = GetRandomText(5);
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(LoginModel cus)
        {
            block = Convert.ToInt32(Session["block"]);
            var cusDao = new CustomerDao();
            if (ModelState.IsValid)
            {
                //Captcha
                string CaptchaTest = Session["Captcha"].ToString();
                if (!cus.Captcha.Equals(CaptchaTest))
                {
                    ModelState.AddModelError("Captcha", "Incorrect captcha code!");
                    Session["Captcha"] = GetRandomText(5);
                    return View(cus);
                }

                //Login
                //Check "block"
                if (block < 3)
                {
                    var result = cusDao.Login(cus.Email,   Encryptor.MD5Hash (cus.LoginPassword));
                    if (result == 1)
                    {
                        var cusSession = cusDao.GetByEmail(cus.Email);
                        //Add sesstion
                        Session["Cus_Session"] = cusSession.CustomerId;
                        return RedirectToAction("AccountList" ,"Account" );
                    }
                    else if (result == 2)
                    {
                        ModelState.AddModelError("", "Your account has been locked (" + cus.Email + ")");
                        Session["Captcha"] = GetRandomText(5);
                        return View(cus);
                    }
                    else if (result == 3)
                    { 
                        //"block" increased 1
                        block++;
                        Session["block"] = Convert.ToInt32(block);
                        ModelState.AddModelError("", "Login Failed");
                        // Session["Captcha"] = GetRandomText(5);
                        return View(cus);
                    }
                }
                else
                { 

                    // Get id by email
                    var cusBlock = cusDao.GetByEmail(cus.Email);

                    // Update status to lock
                    cusDao.BlockCustomer(cusBlock.CustomerId);

                    TempData["block"] = "<script>alert('Your account has been locked (" + cus.Email + ")');</script>";

                    // Session = 0
                    Session["block"] = 0;
                    ModelState.AddModelError("", "Your account has been locked (" + cus.Email + ")");
                }
            }
            return View(cus);
        }

        // GET: Forgot
        public ActionResult Forgot()
        {
            return View();
        }

        // POST: Forgot
        [HttpPost]
        public ActionResult Forgot(LoginModel cus)
        {
            var cusDao = new CustomerDao();

            //Create new password
            var password = GetRandomText(10);

            //Update password
            Boolean check = cusDao.UpdateForgotPassword(cus.Email, Encryptor.MD5Hash(password));

            if (check)
            {
                //Send mail
                cusDao.Send(cus.Email, password);
            }

            ModelState.AddModelError("Email", "New password has been sent via your email. Please check email");
            return View(cus);
        }

        //Create a random text
        private string GetRandomText(int length)
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "0123456789ABCDEFGHJKHLMNOPRSWXYZabcdefghjkhmnopqrstuvwxyz";
            Random r = new Random();
            for (int j = 0; j < length; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }
            return randomText.ToString();
        }

        //Create a captcha imgae
        public FileResult GetCaptchaImage()
        {
            string text = Session["Captcha"].ToString();

            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font font = new Font("Arial", 15);

            SizeF textSize = drawing.MeasureString(text, font);

            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width + 40, (int)textSize.Height + 20);
            drawing = Graphics.FromImage(img);

            Color backColor = Color.White;
            Color textColor = Color.FromArgb(27, 156, 160);

            drawing.Clear(backColor);

            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 20, 10);

            drawing.Save();

            font.Dispose();
            textBrush.Dispose();
            drawing.Dispose();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            img.Dispose();

            return File(ms.ToArray(), "image/png");
        }
    }
}