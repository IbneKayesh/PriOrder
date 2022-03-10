using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class AccountsController : Controller
    {
        public ActionResult MyProfile()
        {
            string distId = Session["userId"].ToString();
            var obj = new T_DSMA();

            if (ApplData.CHACHE_ENABLED)
            {
                obj = HttpContext.Cache.Get(distId + "chProfile") as T_DSMA;
            }
            if (obj == null || obj.DSMA_DSID == null)
            {
                Tuple<List<T_DSMA>, EQResult> _tpl = AccountService.getDistProfile(distId);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    obj = _tpl.Item1.FirstOrDefault();
                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert(distId + "chProfile", obj, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Failed("No Prfile information found");
                }
            }
            return View(obj);
        }

        public ActionResult Banks()
        {
            Tuple<List<T_BANKCOL>, EQResult> _tpl = AccountService.getBankList();
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                ViewBag.ErrorMessages = "Account List is empty";
                return View(new List<T_BANKCOL>());
            }
        }

        public ActionResult MRStatus()
        {
            string distId = Session["userId"].ToString();
            Tuple<List<MR_STATUS>, EQResult> _tpl = AccountService.getMRStatusByDistId(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                ViewBag.ErrorMessages = "MR List is empty";
                return View(new List<MR_STATUS>());
            }
        }


        public ActionResult TestAction()
        {
            AccountService.GetDataSetSP();
            AccountService.ExecuteSP();
            return View();
        }
    }
}