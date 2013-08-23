using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vendord.Models;
using Vendord.DAL;

namespace Vendord.Controllers
{
    public class VendorProductController : Controller
    {
        private VendordContext db = new VendordContext();

        //
        // GET: /VendorProduct/

        public ActionResult Index()
        {
            var vendorproducts = db.VendorProducts.Include(v => v.Vendor).Include(v => v.Product);
            return View(vendorproducts.ToList());
        }

        //
        // GET: /VendorProduct/Details/5

        public ActionResult Details(int id = 0)
        {
            VendorProduct vendorproduct = db.VendorProducts.Find(id);
            if (vendorproduct == null)
            {
                return HttpNotFound();
            }
            return View(vendorproduct);
        }

        //
        // GET: /VendorProduct/Create

        public ActionResult Create()
        {
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            return View();
        }

        //
        // POST: /VendorProduct/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorProduct vendorproduct)
        {
            if (ModelState.IsValid)
            {
                db.VendorProducts.Add(vendorproduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "Name", vendorproduct.VendorID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", vendorproduct.ProductID);
            return View(vendorproduct);
        }

        //
        // GET: /VendorProduct/Edit/5

        public ActionResult Edit(int id = 0)
        {
            VendorProduct vendorproduct = db.VendorProducts.Find(id);
            if (vendorproduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "Name", vendorproduct.VendorID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", vendorproduct.ProductID);
            return View(vendorproduct);
        }

        //
        // POST: /VendorProduct/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorProduct vendorproduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorproduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "Name", vendorproduct.VendorID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", vendorproduct.ProductID);
            return View(vendorproduct);
        }

        //
        // GET: /VendorProduct/Delete/5

        public ActionResult Delete(int id = 0)
        {
            VendorProduct vendorproduct = db.VendorProducts.Find(id);
            if (vendorproduct == null)
            {
                return HttpNotFound();
            }
            return View(vendorproduct);
        }

        //
        // POST: /VendorProduct/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorProduct vendorproduct = db.VendorProducts.Find(id);
            db.VendorProducts.Remove(vendorproduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}