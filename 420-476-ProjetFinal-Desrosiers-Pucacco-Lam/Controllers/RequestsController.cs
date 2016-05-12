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
    public class RequestsController : Controller
    {
        private BDProjetEntities db = new BDProjetEntities();

        // GET: Requests
        public ActionResult Index()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "id", "categoryName");
            var requests = db.Requests.Include(r => r.Category).Include(r => r.Notification).Include(r => r.User).Include(r => r.User1);
            return View(requests.ToList());
          
        }

        [HttpPost]
        public ActionResult Index(int categoryId, string offerTitle)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "id", "categoryName");

            if (offerTitle != "")
            {
                ViewBag.OfferTitle = offerTitle;
                var offers = db.Requests.Where(r => r.title.Contains(offerTitle) && r.categoryId == categoryId).Include(r => r.Category).Include(r => r.User).Include(r => r.User1);
                return View(offers.ToList());
            }
            else
            {
                var offers = db.Requests.Where(r => r.categoryId == categoryId).Include(r => r.Category).Include(r => r.User).Include(r => r.User1);
                return View(offers.ToList());
            }
        }


        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.Categories, "id", "categoryName");
            ViewBag.id = new SelectList(db.Notifications, "id", "type");
            ViewBag.creatorId = new SelectList(db.Users, "id", "firstName");
            ViewBag.matchedUserID = new SelectList(db.Users, "id", "firstName");
            return View();
        }

        // POST: Requests/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "text,title,image,categoryId")] Request request)
        {
            if (ModelState.IsValid)
            {
                var creatorid = (int)Session["ConnectedUserID"];
                var requestid = db.Requests.Count() + 1;
                request.id = requestid;
                request.dateCreated = DateTime.Now;
                request.creatorId = creatorid;
                request.matchedUserID = null;
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Account", "MyOffersAndRequests");
            }

            ViewBag.categoryId = new SelectList(db.Categories, "id", "categoryName", request.categoryId);
            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryId = new SelectList(db.Categories, "id", "categoryName", request.categoryId);
            ViewBag.id = new SelectList(db.Notifications, "id", "type", request.id);
            ViewBag.creatorId = new SelectList(db.Users, "id", "firstName", request.creatorId);
            ViewBag.matchedUserID = new SelectList(db.Users, "id", "firstName", request.matchedUserID);
            return View(request);
        }

        // POST: Requests/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "text,title,image,categoryId")] Request request)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Account", "MyOffersAndRequests");

            }
            ViewBag.categoryId = new SelectList(db.Categories, "id", "categoryName", request.categoryId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
