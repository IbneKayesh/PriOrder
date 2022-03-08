using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Undelivered()
        {
            string distId = Session["userId"].ToString();
            Tuple<List<T_UNDELIVERED>, EQResult> _tpl = ReportService.getUndelivered(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("Undelivered is empty");
                return View(new List<T_UNDELIVERED>());
            }
        }
    }
}