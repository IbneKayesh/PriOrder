using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //
            //return Json(new { success = true, messages = SweetMessages._DeleteSuccess });
            //return Json(new { success = false, messages = SweetMessages._DeleteError });
            return View();
        }

        public ActionResult TestJson()
        {
            return Json(new { success = false, messages = "Successfully added" });
        }


        public ActionResult Login()
        {
            ViewBag.Title = "Login";
            USER_LOGIN obj = new USER_LOGIN();
            obj.USER_ID = "837075";
            //obj.USER_PASSWORD = "25802";
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USER_LOGIN obj)
        {
            Tuple<List<USER_LOGIN_INFO>, EQResult> _tpl = LoginService.getDistInfo(obj.USER_ID, obj.USER_PASSWORD, false);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                Session["userLoginInf"] = _tpl.Item1.FirstOrDefault();
                Session["userId"] = _tpl.Item1.FirstOrDefault().DIST_ID;
                Session["userName"] = _tpl.Item1.FirstOrDefault().DIST_NAME;
                Session["userGroup"] = _tpl.Item1.FirstOrDefault().DIST_GROUP;
                Session["userMobile"] = _tpl.Item1.FirstOrDefault().DIST_MOBILE;
                Session["userBalnace"] = _tpl.Item1.FirstOrDefault().DIST_MOBILE;

                //Load Menu
                Tuple<List<WO_APP_MENU>, EQResult> _tplMenu = LoginService.getMenusByUserId("", IsHo: false);
                Session["menuLeft"] = _tplMenu.Item1.Where(x => x.MODULE_ID == 10).ToList();
                Session["menuBottom"] = _tplMenu.Item1.Where(x => x.MODULE_ID == 20).ToList();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessages = "User Id/Password is incorrect";
                return View(obj);
            }
        }




        public ActionResult BoLogin()
        {
            return View(viewName: "Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BoLogin(USER_LOGIN obj)
        {
            Tuple<List<USER_LOGIN_INFO>, EQResult> _tpl = LoginService.getDistInfo(obj.USER_ID, obj.USER_PASSWORD);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                Session["userLoginInf"] = _tpl.Item1.FirstOrDefault();
                Session["userId"] = _tpl.Item1.FirstOrDefault().DIST_ID;
                Session["userName"] = _tpl.Item1.FirstOrDefault().DIST_NAME;
                Session["userGroup"] = _tpl.Item1.FirstOrDefault().DIST_GROUP;
                Session["userMobile"] = _tpl.Item1.FirstOrDefault().DIST_MOBILE;
                Session["userBalnace"] = _tpl.Item1.FirstOrDefault().DIST_MOBILE;

                //Load Menu
                Tuple<List<WO_APP_MENU>, EQResult> _tplMenu = LoginService.getMenusByUserId("");
                Session["menuLeft"] = _tplMenu.Item1.Where(x => x.MODULE_ID == 50).ToList();
                Session["menuBottom"] = _tplMenu.Item1.Where(x => x.MODULE_ID == 60).ToList();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessages = "User Id/Password is incorrect";
                return View(viewName: "Login", model: obj);
            }
        }







        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult BottomMenu()
        {
            //var objList = HttpContext.Cache.Get("chBottomMenu") as List<WO_APP_MENU>;

            var objList = (List<WO_APP_MENU>)Session["menuBottom"];
            return View("_NotificationsPartial", objList);
        }

    }
}
