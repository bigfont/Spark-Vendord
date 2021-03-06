﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vendord.Models;
using Vendord.ViewModels;
using Vendord.DAL;
using AutoMapper;

namespace Vendord.Controllers
{
    public class ProductController : Controller
    {
        private VendordContext db = new VendordContext();

        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View(db.Products.ToList().OrderBy(p => p.Name));
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products
                .Include(p => p.VendorProducts.Select(vp => vp.Vendor))
                .Where(p => p.ID == id)
                .FirstOrDefault();

            product.VendorProducts = product.VendorProducts.OrderBy(vp => vp.Vendor.Name).ToList();

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCreateViewModel productCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                Product p = Mapper.Map<Product>(productCreateViewModel);

                db.Products.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCreateViewModel);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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