using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476_ProjetFinal_Desrosiers_Pucacco_Lam;

namespace _420_476_ProjetFinal_Desrosiers_Pucacco_Lam.Controllers
{
    public class AccountController : Controller
    {
        private BDProjetEntities db = new BDProjetEntities();

        // GET: Account
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            if (login != null && password != null)
            {
                foreach (User u in db.Users)
                {
                    if (u.login == login && u.password == password)
                    {
                        Session["ConnectedUser"] = u;
                        return RedirectToAction("EditProfile", "Account");
                    }
                }
                ViewBag.LoginFail = "";
                return View();
            }
            else {
                ViewBag.LoginFail = "";
                return View();
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult EditProfile()
        {
            if (Session["ConnectedUser"] != null)
            {
                User user = (User)Session["ConnectedUser"];
                return View(user);
            }
            else {
                return RedirectToAction("Home", "Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "id,firstName,lastName,login,password,image,description")] User user)
        {
            if (Session["ConnectedUser"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(user);
            }
            else {
                return RedirectToAction("Home", "Index");
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Home", "Index");
        }
    }
}