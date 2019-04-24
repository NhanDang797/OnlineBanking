using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OnlineBanking.Models;


namespace OnlineBanking.Controllers {
    public class AccountController : BaseClientController {

        public int getCustomerID() {
            return Convert.ToInt32(Session["Cus_Session"]);
        }

        private OnlineBankingDbContext db = new OnlineBankingDbContext();

        public ActionResult RequestCheque() {
            int customerID = getCustomerID();         
            //int customerID = 1;
            db.Configuration.ProxyCreationEnabled = false;

            Customer customer = db.Customers.SingleOrDefault(x => x.CustomerId == customerID);
            db.Entry(customer).Collection(a => a.BankAccounts).Load();

            IEnumerable<Cheque> list = db.Cheques;
            ViewBag.listCheque = list;

            return View(customer);
        }

        [HttpPost]
        public ActionResult CreateCheque(string accBank, string numberCheque) {
            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            Cheque cheque = new Cheque();
            cheque.AccountNumber = accBank;
            cheque.IssuedDate = DateTime.Now;
            cheque.EndDate = null;
            cheque.NumberOfChequeBook = Int16.Parse(numberCheque);
            cheque.Status = true;

            db.Cheques.Add(cheque);
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public ActionResult BlockBankAccount(string id) {
            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            Cheque cheque = db.Cheques.SingleOrDefault(x => x.AccountNumber.Equals(id));
            cheque.Status = false;
            cheque.EndDate = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult AccountList() {
            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            int idCustomer = getCustomerID();
            //int customerID = 1;

            Customer customer = db.Customers.SingleOrDefault(x => x.CustomerId == idCustomer);
            db.Entry(customer).Collection(a => a.BankAccounts).Load();

            return View(customer);
        }


        public ActionResult Transaction(string accNumber) {
            OnlineBankingDbContext db = new OnlineBankingDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            IEnumerable<Transaction> transaction = db.Transactions.OrderByDescending(x=>x.Date).Where(x => x.SourceAccountNumber.Equals(accNumber));
            return View(transaction);
        }

         public ActionResult Logout() {
            Session["Cus_Session"] = null;
            return RedirectToAction("Index","Home");
        }


    }



}