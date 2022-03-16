using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class PromotionController : Controller
    {
        public ActionResult Slab()
        {
            string distId = Session["userId"].ToString();
            var objList = new List<V_CHOITALY>();
            Tuple<List<V_CHOITALY>, EQResult> _tpl = PromotionService.getSlabPromo();
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                objList = _tpl.Item1;
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Slab promotions found");
            }
            return View(objList);
        }
    }
}