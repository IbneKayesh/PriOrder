using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Utility
{
    public class AioAuthorization : ActionFilterAttribute
    {
        public int controller_id { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string next_url = string.Format("?next_url={0}", filterContext.HttpContext.Request.Url.PathAndQuery);
            var SessionData = HttpContext.Current.Session["userId"];

            if (SessionData == null)
            {
                CleanSession();
                filterContext.Result = new RedirectResult("~/Home/Login" + next_url, true);
                return;
            }

            //from session
            //List<int> controlers = new List<int>(); 
            //if (controlers.Any(x => x == controller_id))
            //{
            //    base.OnActionExecuting(filterContext);
            //}
            //else
            //{
            //    CleanSession();
            //    filterContext.HttpContext.Response.Redirect("~/Home/Login", true);
            //    return;
            //}


            //Open for all session
            base.OnActionExecuting(filterContext);
        }
        public void CleanSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}