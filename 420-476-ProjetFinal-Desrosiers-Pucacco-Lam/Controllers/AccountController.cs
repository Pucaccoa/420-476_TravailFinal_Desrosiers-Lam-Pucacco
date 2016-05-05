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
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                foreach (User u in db.Users)
                {
                    if (u.login == login && u.password == password)
                    {
                        Session["ConnectedUser"] = u;
                        return RedirectToAction("Index", "Offers");
                    }
                }
                ViewBag.LoginFail = "Login/Mot de passe invalid";
                return View();
            }
            else {
                ViewBag.LoginFail = "Login/Mot de passe invalid";
                return View();
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp([Bind(Include = "firstName,lastName,login,password,description")] User user)
        {
            ViewBag.Login = user.login;
            ViewBag.FirstName = user.firstName;
            ViewBag.LastName = user.lastName;
            ViewBag.Description = user.description;

            if (!string.IsNullOrEmpty(user.login) && !string.IsNullOrEmpty(user.password) && !string.IsNullOrEmpty(user.firstName) && !string.IsNullOrEmpty(user.lastName) && !string.IsNullOrEmpty(user.description))
            {
                //ViewBag.Login = user.login;
                //ViewBag.FirstName = user.firstName;
                //ViewBag.LastName = user.lastName;
                //ViewBag.Description = user.description;

                if (checkAvailability(user.login))
                {
                    if (ModelState.IsValid)
                    {
                        user.id = getAutoUserId();
                        user.image = null;
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Login","Account");
                    }

                }
                else {
                    ViewBag.ErrorMessage = "Identifiant choisi est non disponible";
                }
            } else {
                ViewBag.ErrorMessage = "Un des champs n'est pas rempli, veuillez le remplir";
            }
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
                return RedirectToAction("Index", "Offers");
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
                    return RedirectToAction("Index","Offers");
                }
                return View(user);
            }
            else {
                return View(user);
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        public int getAutoUserId()
        {
            int id = db.Users.Count();
            id++;
            return id;
        }

        public bool checkAvailability(string login) {
            foreach (User u in db.Users)
            {
                if (u.login == login)
                {
                    return false;
                }
            }
            return true;
        }
    }
}