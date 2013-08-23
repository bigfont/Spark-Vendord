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
            var vendorproducts = db.VendorProducts
                .Include(vp => vp.Vendor)
                .Include(vp => vp.Product)
                .OrderBy(vp => vp.Vendor.Name);
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
            ViewBag.VendorID = new SelectList(db.Vendors.OrderBy(v => v.Name), "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products.OrderBy(p => p.Name), "ID", "Name");
            return View(new VendorProductCreateViewModel());
        }

        //
        // POST: /VendorProduct/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorProductCreateViewModel vendorProductCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                VendorProduct vp = new VendorProduct { 
                    VendorID = vendorProductCreateViewModel.VendorID,
                    ProductID = vendorProductCreateViewModel.ProductID,
                    UnitPrice = vendorProductCreateViewModel.UnitPrice
                };

                db.VendorProducts.Add(vp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateViewBagWithVendorIDandProductIDSelectLists(vendorProductCreateViewModel);

            return View(vendorProductCreateViewModel);
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

            PopulateViewBagWithVendorIDandProductIDSelectLists(vendorproduct);

            // return view
            return View(vendorproduct);
        }

        #region RefractorMe
        
        private void PopulateViewBagWithVendorIDandProductIDSelectLists(VendorProductCreateViewModel vendorProductCreateViewModel)
        {
            VendorProduct vendorProduct = new VendorProduct { 
                VendorID = vendorProductCreateViewModel.VendorID,
                ProductID = vendorProductCreateViewModel.ProductID
            };
            PopulateViewBagWithVendorIDandProductIDSelectLists(vendorProduct);
        }
        private void PopulateViewBagWithVendorIDandProductIDSelectLists(VendorProduct vendorproduct)
        {
            // vendors
            IList<Vendor> vendors = db.Vendors.ToList();
            IEnumerable<SelectListItem> selectList_Vendors =
                from v in vendors
                select new SelectListItem
                {
                    Selected = (v.ID == vendorproduct.VendorID),
                    Text = v.Name,
                    Value = v.ID.ToString()
                };
            ViewBag.VendorID = selectList_Vendors;

            // products
            IList<Product> products = db.Products.ToList();
            IEnumerable<SelectListItem> selectList_Products =
                from p in products
                select new SelectListItem
                {
                    Selected = (p.ID == vendorproduct.ProductID),
                    Text = p.Name,
                    Value = p.ID.ToString()
                };
            ViewBag.ProductID = selectList_Products;
        }

        #endregion

        //
        // POST: /VendorProduct/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorProduct vendorproduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorproduct).State = EntityState.Modified;
                // {"Violation of UNIQUE KEY constraint 'UQ_VendorProducts_VendorID_ProductID'. 
                // Cannot insert duplicate key in object 'dbo.VendorProducts'. 
                // The duplicate key value is (2, 2).\r\nThe statement has been terminated."}
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