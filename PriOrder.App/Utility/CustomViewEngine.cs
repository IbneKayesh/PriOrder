using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Utility
{
    public class CustomViewEngine 
    {
        //public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        //{
        //    var viewPath = controllerContext.HttpContext.Request.Browser.IsMobileDevice ? "_Layout.New" : "_Layout";

        //    return base.FindView(controllerContext, viewPath, "_Layout.New", useCache);
        //}
    }

    //public class CustomViewEngine : RazorViewEngine
    //{
    //    public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
    //    {
    //        var viewPath = controllerContext.HttpContext.Request.Browser.IsMobileDevice ? "_Layout.New" : "_Layout";

    //        return base.FindView(controllerContext, viewPath, "_Layout.New", useCache);
    //    }
    //}
}