using OnlineBanking.Common;
using OnlineBanking.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBanking.Controllers {
    public class HomeController : Controller {
        OnlineBankingDbContext db = new OnlineBankingDbContext();

        public ActionResult Index() {
            ViewBag.date = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult Faqs() {
            IEnumerable<FAQ> faqs = db.FAQs;

            return View(faqs);
        }
        

        //trangnguyen
        public ActionResult Term()
        {
            return View();
        }
        public ActionResult Privacy()
        {
            return View();
        }
        //end trang-nguyen

            
        
        //aphuong
        public ActionResult Contact() {
            return View();
        }

        //thai

        public ActionResult About() {
            return View();
        }
    }
}