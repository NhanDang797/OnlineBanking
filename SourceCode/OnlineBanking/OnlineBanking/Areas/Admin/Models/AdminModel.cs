using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineBanking.Areas.Admin.Models {
    [Serializable]

    public class AdminModel {
        [Required(ErrorMessage = "Please Enter UserName")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public String Password { get; set; }

    }
}