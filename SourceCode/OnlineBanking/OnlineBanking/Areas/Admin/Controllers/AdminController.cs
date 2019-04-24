using OnlineBanking.Areas.Admin.Dao;
using OnlineBanking.Areas.Admin.Models;
using OnlineBanking.Common;
using OnlineBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBanking.Areas.Admin.Controllers {
    public class AdminController : BaseController {

        // GET: Admin/Home
        public ActionResult Index() {
            return View();
        }

        // create new customer
        [HttpPost]
        public ActionResult CreateCustomer(string fname, string lname, string email, string loginpass, string tranpass, string phone, string address, string gender) {

            DateTime now = DateTime.Now;
            bool genderFormat = true;
            if (gender == "1") { // male
                genderFormat = true;
            }
            else {
                genderFormat = true;
            }
            
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Admin/mail/mailtemplate.html"));
            content = content.Replace("{{CustomerName}}", fname +lname);
            content = content.Replace("{{passlogin}}", loginpass);
            content = content.Replace("{{passtran}}", tranpass);
            content = content.Replace("{{email}}", email);
            content = content.Replace("{{date_}}", now.ToString());

            new MailHelper().SendMail(email, "New Message From OnlineBanking", content);


            Customer customer = new Customer();
            customer.FirstName = fname;
            customer.LastName = lname;
            customer.Email = email;
            customer.LoginPassword = Encryptor.MD5Hash(loginpass);
            customer.TransactionPassword = Encryptor.MD5Hash(tranpass);
            customer.PhoneNumber = phone; 
            customer.Address = address;
            customer.Gender = genderFormat;
            customer.LockedStatus = true;  // 1 active
            customer.CreateDate = now;

            var dao = new CustomerDao();
            dao.CreateCustomer(customer);

            return RedirectToAction("Index", "Admin");
        }


        // load data for searching

        [HttpGet]
        public JsonResult LoadData(int page, int size, string name, string status) {

            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            var dao = new CustomerDao();

            IEnumerable<Customer> model = db.Customers;

            if (!string.IsNullOrEmpty(name)) {
                model = model.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(name.ToLower()));
            }

            if (!string.IsNullOrEmpty(status)) {
                var statusBool = bool.Parse(status);
                model = model.Where(x => x.LockedStatus == statusBool);
            }

            var countPage = model.Count();

            model = model.OrderBy(x => x.CustomerId).Skip((page - 1) * size).Take(size);


            return Json(new {
                data = model,
                totalRow = countPage,
                status = true

            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadFAQs() {

            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            
            IEnumerable<FAQ> model = db.FAQs;
            
            return Json(new {
                data = model,
                status = true
            });
        }

         [HttpPost]
        public JsonResult GetFaqsById(int id) {

            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            
            FAQ model = db.FAQs.SingleOrDefault(x => x.Id == id);
            
            return Json(new {
                data = model,
                status = true
            });
        }


         [HttpPost]
        public ActionResult EditFaqs(int id ,string question, string answer) {

            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            var faqs = db.FAQs.SingleOrDefault(x => x.Id == id);
            faqs.Question = question;
            faqs.Answer = answer;
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult DeleteFaqs(int id) {

            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            var faqs = db.FAQs.SingleOrDefault(x => x.Id == id);
            db.FAQs.Remove(faqs);
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult CreateFaqs(string question, string answer) {

            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            FAQ faq = new FAQ();
            faq.Question = question;
            faq.Answer = answer;

            db.FAQs.Add(faq);
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }




        public ActionResult Detail(int id) {
            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            Customer customer = db.Customers.FirstOrDefault(a => a.CustomerId == id);
            db.Entry(customer).Collection(a => a.BankAccounts).Load();


            return View(customer);
        }

        [HttpPost]
        public ActionResult CreateAccount(int id,string accountNumber) {
            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            DateTime now = DateTime.Now;
            BankAccount bank = new BankAccount();
            bank.AccountNumber = accountNumber;
            bank.CustomerId = id;
            bank.Balance = 0;
            bank.CreateDate = now;

            db.BankAccounts.Add(bank);
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult TransferMoney(string accNumber,string numberMoney) {
            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            BankAccount bankAccount = db.BankAccounts.SingleOrDefault(x=> x.AccountNumber == accNumber);
            decimal moneyCurrent = bankAccount.Balance;
            decimal newMoney = moneyCurrent + decimal.Parse(numberMoney);

            bankAccount.Balance = newMoney;
            db.SaveChanges();


            return RedirectToAction("Index", "Admin");
        }


        //create account customer


        //update password and send email 
        [HttpPost]
        public ActionResult UpdateCustomer(int id, string loginpass, string tranpass) {

            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            Customer cus = db.Customers.SingleOrDefault(x => x.CustomerId == id);
            //send mail

            string name = cus.FirstName + " " + cus.LastName;
            string email = cus.Email;
            string date_ = DateTime.Now.ToString();

            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Admin/mail/mailtemplate.html"));
            content = content.Replace("{{CustomerName}}",name);
            content = content.Replace("{{passlogin}}", loginpass);
            content = content.Replace("{{passtran}}", tranpass);
            content = content.Replace("{{email}}",email );
            content = content.Replace("{{date_}}", date_);
            
            new MailHelper().SendMail(email, "New Message From OnlineBanking", content);

            //end sending mail
            //md5
            cus.LoginPassword = Encryptor.MD5Hash( loginpass);
            cus.TransactionPassword = Encryptor.MD5Hash(tranpass);
            cus.LockedStatus = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }


        //update password and send email 
        [HttpPost]
        public ActionResult BlockCustomer(int id) {

            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            Customer cus = db.Customers.SingleOrDefault(x => x.CustomerId == id);
            //send mail
            
            cus.LockedStatus = false;

            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }
        

    }
}