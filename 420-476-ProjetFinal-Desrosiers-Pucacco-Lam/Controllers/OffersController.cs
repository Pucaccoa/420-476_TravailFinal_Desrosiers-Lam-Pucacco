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
    public class OffersController : Controller
    {
        private BDProjetEntities db = new BDProjetEntities();

        // GET: Offers
        public ActionResult Index()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "id", "categoryName");
            var offers = db.Offers.Include(o => o.Category).Include(o => o.User).Include(o => o.User1);
            return View(offers.ToList());
        }

        [HttpPost]
        public ActionResult Index(int categoryId, string offerTitle)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "id", "categoryName");

            if (offerTitle != "")
            {
                ViewBag.OfferTitle = offerTitle;
                var offers = db.Offers.Where(o => o.title.Contains(offerTitle) && o.categoryId == categoryId).Include(o => o.Category).Include(o => o.User).Include(o => o.User1);
                return View(offers.ToList());
            }
            else
            {
                var offers = db.Offers.Where(o => o.categoryId == categoryId).Include(o => o.Category).Include(o => o.User).Include(o => o.User1);
                return View(offers.ToList());
            }
        }

        // GET: Offers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // GET: Offers/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.Categories, "id", "categoryName");
            ViewBag.creatorID = new SelectList(db.Users, "id", "firstName");
            ViewBag.matchedUserId = new SelectList(db.Users, "id", "firstName");
            return View();
        }

        // POST: Offers/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,text,title,dateCreated,image,creatorID,matchedUserId,categoryId")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoryId = new SelectList(db.Categories, "id", "categoryName", offer.categoryId);
            ViewBag.creatorID = new SelectList(db.Users, "id", "firstName", offer.creatorID);
            ViewBag.matchedUserId = new SelectList(db.Users, "id", "firstName", offer.matchedUserId);
            return View(offer);
        }

        // GET: Offers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryId = new SelectList(db.Categories, "id", "categoryName", offer.categoryId);
            ViewBag.creatorID = new SelectList(db.Users, "id", "firstName", offer.creatorID);
            ViewBag.matchedUserId = new SelectList(db.Users, "id", "firstName", offer.matchedUserId);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,text,title,dateCreated,image,creatorID,matchedUserId,categoryId")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.Categories, "id", "categoryName", offer.categoryId);
            ViewBag.creatorID = new SelectList(db.Users, "id", "firstName", offer.creatorID);
            ViewBag.matchedUserId = new SelectList(db.Users, "id", "firstName", offer.matchedUserId);
            return View(offer);
        }

        // GET: Offers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offer offer = db.Offers.Find(id);
            db.Offers.Remove(offer);
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
