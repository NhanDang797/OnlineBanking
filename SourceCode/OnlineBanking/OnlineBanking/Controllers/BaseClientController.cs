using OnlineBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBanking.Controllers
{
    public class BaseClientController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            int session = Convert.ToInt32(Session["Cus_Session"]);
            if (session == 0) {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Login", action = "Login" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}