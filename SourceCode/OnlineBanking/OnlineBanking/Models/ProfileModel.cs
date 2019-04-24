using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineBanking.Models
{
    public class ProfileModel
    {
        public int CustomerId { get; set; }

        [StringLength(18, MinimumLength = 9, ErrorMessage = "Password has 9 - 18 characters")]
        [Display(Name = "Password")]
        public string LoginPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("LoginPassword", ErrorMessage = "Confirm password must be same a password")]
        public string ConfirmPassword { get; set; }

        [StringLength(18, MinimumLength = 9, ErrorMessage = "Transaction Password has 9 - 18 characters")]
        [Display(Name = "Transaction Password")]
        public string TransactionPassword { get; set; }

        [Display(Name = "Confirm transaction Password")]
        [Compare("TransactionPassword", ErrorMessage = "Confirm transaction password must be same a transaction password")]
        public string ConfirmTransactionPassword { get; set; }

        public string Email { get; set; }

        [StringLength(300, MinimumLength = 5, ErrorMessage = "Address must be at least 5 characters")]
        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}