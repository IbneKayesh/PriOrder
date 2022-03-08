using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Messages
        public ActionResult Sms()
        {
            var objList = MessageService.getSms("");
            return View(objList);
        }

        public ActionResult Support()
        {
            string distId = "1234";
            var objList = MessageService.getSupport(distId);
            return View(objList);
        }
        public ActionResult Help()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Help(WO_SUP_MSG obj)
        {
            return View();
        }


        public ActionResult SupportReply(string replyId)
        {
            if (Request.UrlReferrer == null)
            {
                TempData["mesg"] = SweetMessages.Failed("This Support number is closed!");
                return RedirectToAction(nameof(Support));
            }
            var objList = MessageService.getSupportBody(replyId);
            return View(objList);
        }
    }
}