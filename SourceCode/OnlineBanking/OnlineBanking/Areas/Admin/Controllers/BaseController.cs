using OnlineBanking.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBanking.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            var session = (AdminModel)Session["ADMIN_SESSION"];
            if (session == null) {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Login", action = "Login", Area = "Admin" }));
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}