using OnlineBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBanking.Areas.Admin.Dao {
    public class LoginDao {
        OnlineBankingDbContext db = null;

        //Constructor
        public LoginDao() {
            db = new OnlineBankingDbContext();
        }

        // getUserByID
        public Admintb GetUserByName(String name) {
            return db.Admintbs.SingleOrDefault(x => x.LoginName == name);
        }

        // login
        public bool Login(string userName, string passWord) {

            var result = db.Admintbs.Count(x => x.LoginName == userName && x.LoginPassword == passWord);
            if (result > 0) return true;
            else return false;
        }

    }
}