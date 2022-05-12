using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
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
            //obj.USER_ID = "837075";
            //obj.USER_PASSWORD = "25802";
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USER_LOGIN obj, string next_url)
        {
            Tuple<List<USER_LOGIN_INFO>, EQResult> _tpl = LoginService.getDistInfo(obj.USER_ID, obj.USER_PASSWORD, false);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                Session["userLoginInf"] = _tpl.Item1.FirstOrDefault();
                Session["userId"] = _tpl.Item1.FirstOrDefault().DIST_ID;
                Session["userName"] = _tpl.Item1.FirstOrDefault().DIST_NAME;
                Session["userGroup"] = _tpl.Item1.FirstOrDefault().DIST_GROUP;
                Session["userMobile"] = _tpl.Item1.FirstOrDefault().DIST_MOBILE;

                //Load Menu
                Tuple<List<WO_APP_MENU>, EQResult> _tplMenu = LoginService.getMenusByUserId("", IsHo: false);
                Session["menuLeft"] = _tplMenu.Item1.Where(x => x.MODULE_ID == 10).ToList();
                Session["menuBottom"] = _tplMenu.Item1.Where(x => x.MODULE_ID == 20).ToList();


                //Clear Cache
                foreach (DictionaryEntry entry in HttpContext.Cache)
                {
                    HttpContext.Cache.Remove((string)entry.Key);
                }

                if (Url.IsLocalUrl(next_url) && next_url.Length > 1 && next_url.StartsWith("/") && !next_url.StartsWith("//") && !next_url.StartsWith("/\\"))
                {
                    return Redirect(next_url);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                ViewBag.ErrorMessages = "User Id/Password is incorrect";
                return View(obj);
            }
        }




        public ActionResult Signin()
        {
            USER_LOGIN obj = new USER_LOGIN();
            obj.IS_XO = true;
            return View(viewName: "Login", model: obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin(USER_LOGIN obj)
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


        public ActionResult BottomMenu()
        {
            string distId = Session["userId"].ToString();
            //var objList = HttpContext.Cache.Get("chBottomMenu") as List<WO_APP_MENU>;
            List<WO_APP_MENU> objList = (List<WO_APP_MENU>)Session["menuBottom"];
            //Cart Count
            if (objList.Any(x => x.MENU_ID == 20010003))
            {
                int CartCount = 0;
                try
                {
                    CartCount = (int)HttpContext.Cache?.Get(distId + ApplData.CART_COUNT_CACHE);
                }
                catch
                {
                    CartCount = OrderService.getCartItemsCount(distId);
                    HttpContext.Cache.Insert(distId + ApplData.CART_COUNT_CACHE, CartCount, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                }
                objList.Where(x => x.MENU_ID == 20010003).First().MODULE_ID = CartCount;
            }
            return PartialView("_BottomMenu", objList);
        }

        public ActionResult LeftMenu()
        {
            List<WO_APP_MENU> objList = (List<WO_APP_MENU>)Session["menuLeft"];
            return PartialView("_LeftMenu", objList);
        }




        public ActionResult UserGuide()
        {
            return View();
        }

    }
}
