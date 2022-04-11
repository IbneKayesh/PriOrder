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
    public class NFController : Controller
    {
        public ActionResult Index()
        {
            string distId = Session["userId"].ToString();
            Tuple<List<NIF_DIST>, EQResult> _tpl = NFService.getNIF_DIST(distId);
            if (_tpl.Item2.ROWS > 0)
            {

            }
            else
            {
                return RedirectToAction(nameof(Apply));
            }
            return View();
        }

        public ActionResult Apply()
        {
            //string distId = Session["userId"].ToString();
            NIF_APPL obj = new NIF_APPL();
            return View(obj);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Apply(NIF_APPL obj)
        {
            return View(obj);
        }
        
    }
}