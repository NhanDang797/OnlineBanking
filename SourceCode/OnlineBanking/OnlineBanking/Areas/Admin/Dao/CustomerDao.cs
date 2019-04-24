using OnlineBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBanking.Areas.Admin.Dao {
    public class CustomerDao {

        OnlineBankingDbContext db = null;
        
        public CustomerDao() {
            db = new OnlineBankingDbContext();
        }

        // create new customer
        public void CreateCustomer( Customer customer) {
            db.Customers.Add(customer);
            db.SaveChanges();
        }
      


    }
}