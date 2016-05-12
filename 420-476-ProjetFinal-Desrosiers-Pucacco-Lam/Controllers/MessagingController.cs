using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _420_476_ProjetFinal_Desrosiers_Pucacco_Lam.Controllers
{
    public class MessagingController : Controller
    {

        private BDProjetEntities db = new BDProjetEntities();

        [HttpPost]
        public ActionResult Message(string creatorId, string title)
        {
            if (Session["ConnectedUserID"] != null)
            {
                ViewBag.CreatorId = creatorId;
                ViewBag.Title = title;
                return View();
            }
            else {
                return RedirectToAction("Login", "Account");
            }
        }

        
        [HttpPost]
        public ActionResult SendMessage([Bind(Include = "sourceUserId,targetUserId,message1")] Message msg)
        {
            if (Session["ConnectedUserID"] != null)
            {
                if (ModelState.IsValid)
                {
                    msg.targetUserId = 
                    msg.sourceUserId = (int)Session["ConnectedUserID"];
                    db.Messages.Add(msg);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }

                return View();
            }
            else {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}