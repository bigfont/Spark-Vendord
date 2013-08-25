using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vendord.Models;
using Vendord.DAL;
using System.Data.Entity.Infrastructure;
using Vendord.ViewModels;

namespace Vendord.Controllers
{
    public class VendorController : Controller
    {
        private VendordContext db = new VendordContext();

        //
        // GET: /Vendor/

        public ActionResult Index()
        {
            return View(db.Vendors.ToList().OrderBy(v => v.Name));
        }

        //
        // GET: /Vendor/Details/5

        public ActionResult Details(int id = 0)
        {
            Vendor vendor = db.Vendors
                .Include(v => v.VendorProducts.Select(vp => vp.Product))
                .Where(v => v.ID == id)
                .FirstOrDefault();

            vendor.VendorProducts = vendor.VendorProducts.OrderBy(vp => vp.Product.Name).ToList();

            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        //
        // GET: /Vendor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Vendor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorCreateViewModel vendorCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                Vendor v = new Vendor
                {
                    Name = vendorCreateViewModel.Name
                };

                db.Vendors.Add(v);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendorCreateViewModel);
        }

        //
        // GET: /Vendor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        //
        // POST: /Vendor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }

        //
        // GET: /Vendor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        //
        // POST: /Vendor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendors.Find(id);
            db.Vendors.Remove(vendor);
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