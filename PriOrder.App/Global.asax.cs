using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace PriOrder.App
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Request.RequestContext.HttpContext.SetOverriddenBrowser(BrowserOverride.Mobile);
            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("iPad")
            //{
            //    ContextCondition = (context => context.GetOverriddenUserAgent().IndexOf
            //    ("iPad", StringComparison.OrdinalIgnoreCase) >= 0)
            //});
            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Android")
            //{
            //    ContextCondition = (context => context.GetOverriddenUserAgent().IndexOf
            //    ("Android", StringComparison.OrdinalIgnoreCase) >= 0)
            //});

            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new CustomViewEngine());
        }
    }
}
