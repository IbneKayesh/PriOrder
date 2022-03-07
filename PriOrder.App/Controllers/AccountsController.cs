using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult MyProfile()
        {
            return View();
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
    }
}