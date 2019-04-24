using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineBanking.Common;
using OnlineBanking.Models;

namespace OnlineBanking.Controllers {

    public class TransactionsController : BaseClientController {
        private OnlineBankingDbContext db = new OnlineBankingDbContext();

        public int getCustomerID() {
            return Convert.ToInt32(Session["Cus_Session"]);
        }

        public string getTransactionPass() {
            int CustomerId = getCustomerID();
            Customer customer = db.Customers.SingleOrDefault(x => x.CustomerId == CustomerId);

            return customer.TransactionPassword;
        }

        //-------------- tu viet
        string BenificialAccountName = "";
        public ActionResult FundTransfer() {
            int CustomerId = getCustomerID();
            IEnumerable<SelectListItem> accountList = db.BankAccounts
        .Where(item => item.CustomerId == CustomerId)
        .Select(item => new SelectListItem { Value = item.AccountNumber, Text = item.AccountNumber });
            ViewBag.SourceAccountNumber = accountList;
            return View();
        }

        public ActionResult TransferConfirm(string OriginatorAccount, string BenificialAccount, decimal Amount, string Content) {

            int CustomerId = getCustomerID();
            bool validated = false;
            string message = "";

            if (OriginatorAccount.Equals(BenificialAccount))
            {
                validated = false;
                message = "You cannot send to the same account.";
                return Json(new
                {
                    message = message
                });
            }
            //check originator account
            var OriginatorAccountInDatabase = db.BankAccounts
                .Where(item => item.CustomerId == CustomerId)
                .Where(item => item.AccountNumber.Equals(OriginatorAccount)).First();
            if (OriginatorAccountInDatabase != null) {
                validated = true;
            }
            else {
                validated = false;
                message = "This account is invalid.";
                return Json(new
                {
                    message = message
                });
            }
            //check amount of originator account
            //check greater than zero
            if (Amount > 0) {
                validated = true;
            }
            else {
                validated = false;
                message = "Amount must greater than 0.";
                return Json(new
                {
                    message = message
                });
            }

            //check less than originator account balance - 10 USD
            decimal OriginatorAccountBalance = OriginatorAccountInDatabase.Balance;
            if (Amount >= OriginatorAccountBalance - 10) {
                validated = false;
                message = "You do not have enough money to transfer.";
                return Json(new
                {
                    message = message
                });
            }

            //check benificial account
            var BenificialAccountInDatabase = db.BankAccounts
                .Where(item => item.AccountNumber.Equals(BenificialAccount)).FirstOrDefault();
            if (BenificialAccountInDatabase != null) {
                validated = true;
                var benAcc = db.Customers
                    .Where(cus => cus.CustomerId == BenificialAccountInDatabase.CustomerId).First();
                BenificialAccountName = benAcc.FirstName + " " + benAcc.LastName;
            }
            else
            {
                validated = false;
                message = "Your destination account does not exist.";
                return Json(new
                {
                    message = message
                });
            }
            if (validated == true)
            {
                return Json(new
                {
                    oA = OriginatorAccount,
                    bA = BenificialAccount,
                    bAName = BenificialAccountName,
                    am = Amount,
                    content = Content
                });
            }
            else
            {
                return Json(new
                {
                    message = message
                });
            }
        }

        public ActionResult Transfer(string OriginatorAccount, string BenificialAccount, decimal Amount, string Content, string TransactionPassword) {
            int CustomerId = getCustomerID();
            string transactionpass = getTransactionPass();
            //Check transaction password
            if (Encryptor.MD5Hash(TransactionPassword).Equals(transactionpass)) {
                using (db) {
                    db.Database.Log = Console.Write;
                    using (DbContextTransaction transaction = db.Database.BeginTransaction()) {
                        try {
                            var source = db.BankAccounts.Where(acc => acc.AccountNumber.Equals(OriginatorAccount)).First();
                            var destination = db.BankAccounts.Where(acc => acc.AccountNumber.Equals(BenificialAccount)).First();
                            source.Balance = source.Balance - Amount;
                            destination.Balance = destination.Balance + Amount;
                            if (ModelState.IsValid) {
                                db.Entry(source).State = EntityState.Modified;
                                db.Entry(destination).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                            //create 2 transaction
                            Transaction t1 = new Transaction();
                            t1.CustomerId = source.CustomerId;
                            t1.SourceAccountNumber = OriginatorAccount;
                            t1.TargetAccountNumber = BenificialAccount;
                            t1.Date = DateTime.Now;
                            //t1.Date = String.Format("{0:yyyy-MM-dd}", DateTime.Today);
                            t1.Description = Content;
                            t1.SendReceiveStatus = true;
                            t1.Amount = Amount;
                            t1.Balance = source.Balance;

                            //add transaction to Transaction table
                            if (ModelState.IsValid) {
                                db.Transactions.Add(t1);
                                db.Entry(t1).State = EntityState.Added;
                                db.SaveChanges();
                            }
                            Transaction t2 = new Transaction();
                            t2.CustomerId = source.CustomerId;
                            t2.SourceAccountNumber = BenificialAccount;
                            t2.TargetAccountNumber = OriginatorAccount;
                            t2.Date = DateTime.Now;
                            //t2.Date = String.Format("{0:yyyy-MM-dd}", new DateTime(2008, 3, 9));
                            t2.Description = Content;
                            t2.SendReceiveStatus = false;
                            t2.Amount = Amount;
                            t2.Balance = destination.Balance;


                            //add transaction to Transaction table
                            if (ModelState.IsValid) {
                                db.Transactions.Add(t2);
                                db.Entry(t2).State = EntityState.Added;
                                db.SaveChanges();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex) {
                            transaction.Rollback();
                            string message = "Error occurred. Please try again in few minutes.";
                            return Json(new
                            {
                                message2 = message
                            });
                        }
                        return Json(new
                        {
                            bA = BenificialAccount,
                            bAName = BenificialAccountName,
                            am = Amount
                        });
                    }
                }
            }
            else
            {
                string message = "Your password is incorrect.";
                return Json(new
                {
                    message2 = message
                });
            }
        }

        // ***view
        public ActionResult ChoseStatementType() {

            int CustomerId = getCustomerID();
            IEnumerable<SelectListItem> accountList = db.BankAccounts
        .Where(item => item.CustomerId == CustomerId)
        .Select(item => new SelectListItem { Value = item.AccountNumber, Text = item.AccountNumber });
            ViewBag.SourceAccountNumber = accountList;
            return View();
        }


        //****go to statement type
        [HttpPost]
        public ActionResult GetStatement(FormCollection fc) {
            string AccountNumber = fc["originatoraccount"];

            string txtStatementType = fc["dropdownstatementtype"].ToString();
            int statementType = Convert.ToInt16(txtStatementType);
            string txtYear = fc["yeardropdown"].ToString();
            int Year = Convert.ToInt16(txtYear);
            string txtMonth = fc["monthdropdown"].ToString();
            int Month = Convert.ToInt16(txtMonth);
            ViewBag.AccountNumber = AccountNumber;
            ViewBag.Year = Year;
            ViewBag.Month = Month;

            if (statementType == 2) {
                return RedirectToAction("MonthlyStatement", new { Year = ViewBag.Year, Month = ViewBag.Month, AccountNumber = ViewBag.AccountNumber });
            }
            else {
                return RedirectToAction("AnnualStatement", new { Year = ViewBag.Year, AccountNumber = ViewBag.AccountNumber });
            }
        }

        //****get Monthly Statement
        public ActionResult MonthlyStatement(int Year, int Month, string AccountNumber) {
            int DisplayYear = Year;
            string DisplayMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month); ;
            string DisplayAccNumber = AccountNumber;
            int CustId = db.BankAccounts.Where(item => item.AccountNumber == AccountNumber).First().CustomerId;
            var cus = db.Customers.
                Where(item => item.CustomerId == CustId).First();
            string DisplayAccName = cus.FirstName + " " + cus.LastName;
            string DisplayDate = String.Format("{0:d/M/yyyy}", DateTime.Now);

            //lay mot collection theo Account Number, date & year
            string DisplayOpenBalance = "0.00";
            decimal? PaidIn = 0;
            decimal? PaidOut = 0;
            string DisplayPaymentIn;
            string DisplayPaymentOut;
            string DisplayClosingBalance = "0.00";

            IQueryable<Transaction> collection = db.Transactions
                .Where(item => item.SourceAccountNumber == AccountNumber)
                .Where(item => item.Date.Year == Year)
                .Where(item => item.Date.Month == Month);
            var closestDay = db.Transactions
                .Where(item => item.SourceAccountNumber == AccountNumber)
                .Where(item => item.Date.Year == Year)
                .Where(item => item.Date.Month < Month)
                .OrderByDescending(item => item.Date)
                .FirstOrDefault();
            decimal? openBalance = closestDay != null ? closestDay.Balance : 0;
            if (collection.Any()) {
                DisplayOpenBalance = String.Format("{0:.00}", openBalance);
                PaidIn = collection.Where(item => item.SendReceiveStatus == false).Sum(item => (decimal?)item.Amount);
                PaidOut = collection.Where(item => item.SendReceiveStatus == true).Sum(item => (decimal?)item.Amount);
                DisplayClosingBalance = String.Format("{0:.00}", collection.OrderByDescending(item => item.Date).First().Balance);
            }
            else {
                DisplayOpenBalance = String.Format("{0:.00}", openBalance);
                PaidIn = 0;
                PaidOut = 0;
                DisplayClosingBalance = String.Format("{0:.00}", openBalance);
            }

            DisplayPaymentIn = String.Format("{0:0.00}", PaidIn != null ? PaidIn : 0);
            DisplayPaymentOut = String.Format("{0:0.00}", PaidOut != null ? PaidOut : 0);

            ViewBag.DisplayYear = DisplayYear;
            ViewBag.DisplayMonth = DisplayMonth;
            ViewBag.DisplayAccNumber = DisplayAccNumber;
            ViewBag.DisplayAccName = DisplayAccName;
            ViewBag.DisplayDate = DisplayDate;
            ViewBag.DisplayOpenBalance = DisplayOpenBalance;
            ViewBag.DisplayPaymentIn = DisplayPaymentIn;
            ViewBag.DisplayPaymentOut = DisplayPaymentOut;
            ViewBag.DisplayClosingBalance = DisplayClosingBalance;

            List<Transaction> list = db.Transactions
                .Where(item => item.SourceAccountNumber == AccountNumber)
                .Where(item => item.Date.Year == Year)
                .Where(item => item.Date.Month == Month).OrderBy(item => item.Date).ToList();
            List<StatementDTO> outputList = new List<StatementDTO>();
            foreach (var item in list) {
                StatementDTO stm = new StatementDTO();
                stm.Date = String.Format("{0:d/M/yyyy}", item.Date);
                stm.Description = item.Description;
                stm.PaidIn = (item.SendReceiveStatus == false) ? String.Format("{0:.##}", item.Amount) : " ";
                stm.PaidOut = (item.SendReceiveStatus == true) ? String.Format("{0:.##}", item.Amount) : " ";
                stm.Balance = String.Format("{0:.##}", item.Balance);
                outputList.Add(stm);
            }
            return View(outputList);
        }

        //*** get annual statement
        public ActionResult AnnualStatement(int Year = 2019, string AccountNumber = "OB-6525235312") {
            int DisplayYear = Year;
            string DisplayAccNumber = AccountNumber;
            int CustId = db.BankAccounts.Where(item => item.AccountNumber == AccountNumber).First().CustomerId;
            var cus = db.Customers.
                Where(item => item.CustomerId == CustId).First();
            string DisplayAccName = cus.FirstName + " " + cus.LastName;
            string DisplayDate = String.Format("{0:d/M/yyyy}", DateTime.Now);

            //lay mot collection theo Account Number, date & year
            string DisplayOpenBalance = "0.00";
            decimal? PaidIn = 0;
            decimal? PaidOut = 0;
            string DisplayPaymentIn;
            string DisplayPaymentOut;
            string DisplayClosingBalance = "0.00";

            IQueryable<Transaction> collection = db.Transactions
                .Where(item => item.SourceAccountNumber == AccountNumber)
                .Where(item => item.Date.Year == Year);
            var closestDay = db.Transactions
                .Where(item => item.SourceAccountNumber == AccountNumber)
                .Where(item => item.Date.Year < Year)
                .OrderByDescending(item => item.Date)
                .FirstOrDefault();
            decimal? openBalance = closestDay != null ? closestDay.Balance : 0;
            if (collection.Any()) {
                DisplayOpenBalance = String.Format("{0:.00}", openBalance);
                PaidIn = collection.Where(item => item.SendReceiveStatus == false).Sum(item => (decimal?)item.Amount);
                PaidOut = collection.Where(item => item.SendReceiveStatus == true).Sum(item => (decimal?)item.Amount);
                DisplayClosingBalance = String.Format("{0:.00}", collection.OrderByDescending(item => item.Date).First().Balance);
            }
            else {
                DisplayOpenBalance = String.Format("{0:.00}", openBalance);
                PaidIn = 0;
                PaidOut = 0;
                DisplayClosingBalance = String.Format("{0:.00}", openBalance);
            }

            DisplayPaymentIn = String.Format("{0:0.00}", PaidIn != null ? PaidIn : 0);
            DisplayPaymentOut = String.Format("{0:0.00}", PaidOut != null ? PaidOut : 0);

            ViewBag.DisplayYear = DisplayYear;
            ViewBag.DisplayAccNumber = DisplayAccNumber;
            ViewBag.DisplayAccName = DisplayAccName;
            ViewBag.DisplayDate = DisplayDate;
            ViewBag.DisplayOpenBalance = DisplayOpenBalance;
            ViewBag.DisplayPaymentIn = DisplayPaymentIn;
            ViewBag.DisplayPaymentOut = DisplayPaymentOut;
            ViewBag.DisplayClosingBalance = DisplayClosingBalance;

            List<StatementDTO> outputList = new List<StatementDTO>();

            var transact = db.Transactions
               .Where(item => item.SourceAccountNumber == AccountNumber)
               .Where(item => item.Date.Year == Year);

            var result = transact
                            .GroupBy(c => new {
                                c.Date.Month,
                                c.SendReceiveStatus
                            })
                            .Select(gcs => new {
                                Month = gcs.Key.Month,
                                PaidIn = gcs.Where(c => c.SendReceiveStatus == false).Sum(c => (decimal?)c.Amount) ?? 0,
                                PaidOut = gcs.Where(c => c.SendReceiveStatus == true).Sum(c => (decimal?)c.Amount) ?? 0,
                                Sum = gcs.Where(c => c.SendReceiveStatus == false).Sum(c => (decimal?)c.Amount) ?? 0 - gcs.Where(c => c.SendReceiveStatus == true).Sum(c => (decimal?)c.Amount) ?? 0
                            });

            foreach (var item in result) {
                StatementDTO stm = new StatementDTO();
                stm.Date = item.Month.ToString() + "/" + Year.ToString();
                stm.PaidIn = String.Format("{0:0.00}", item.PaidIn);
                stm.PaidOut = String.Format("{0:0.00}", item.PaidOut);
                stm.Sum = String.Format("{0:0.00}", item.Sum);
                outputList.Add(stm);
            }

            return View(outputList);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
