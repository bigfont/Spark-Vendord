using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prototype.ViewModels;
using System.Reflection;

namespace Prototype.Controllers
{
    public class ExcelController : Controller
    {
        private string ExcelDirectory
        {
            get
            {
                return Server.MapPath("~/ExcelWorkbooks");
            }
        }

        //GET
        public ActionResult Index()
        {
            return View();
        }

        //GET
        public ActionResult Vendors()
        {
            //get a list of all vendors
            List<SelectVendor> selectVendors = new List<SelectVendor>();
            using (Models.VendordContext db = new Prototype.Models.VendordContext())
            {
                selectVendors = db.Vendors
                    .Where<Models.Vendor>(v => v.VendorName != null)
                    .Select<Models.Vendor, SelectVendor>(v => new SelectVendor { VendorID = v.VendorID, VendorName = v.VendorName }).ToList();
            }

            return View(selectVendors);
        }

        public ActionResult CreateVendor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Workbooks(string vendorNameID = null, string vendorName = null)
        {
            //get all the workbook names
            IEnumerable<System.String> workbookNames = from file in Directory.EnumerateFiles(ExcelDirectory, "*.xlsx", SearchOption.AllDirectories)
                                                       select System.IO.Path.GetFileName(file);

            //create and/or store the vendor name and ID                        
            if (vendorName != null)
            {
                Models.Vendor newVendor;
                using (Prototype.Models.VendordContext db = new Models.VendordContext())
                {
                    newVendor = db.Vendors.Create();
                    newVendor.VendorName = vendorName;
                    db.Vendors.Add(newVendor);
                    db.SaveChanges();
                }
                //viewbag
                ViewBag.VendorID = newVendor.VendorID;
                ViewBag.VendorName = newVendor.VendorName;
            }
            else
            {
                //viewbag
                ViewBag.VendorID = vendorNameID.Split('_')[0];
                ViewBag.VendorName = vendorNameID.Split('_')[1];
            }

            return View(workbookNames);
        }

        [HttpPost]
        public ActionResult Worksheets(string vendorID, string vendorName, HttpPostedFileBase workbookFile)
        {
            string workbookName;
            ActionResult actionResult;
            if (UploadPostedFile(workbookFile, out workbookName))
            {
                //get a list of all worksheets
                var excel = new ExcelQueryFactory(Path.Combine(ExcelDirectory, workbookName));
                IEnumerable<System.String> worksheetNames = excel.GetWorksheetNames();
                actionResult = View("Worksheets", worksheetNames);
            }
            else
            {
                actionResult = RedirectToAction("Workbooks");
            }

            //viewbag
            ViewBag.VendorID = vendorID;
            ViewBag.VendorName = vendorName;
            ViewBag.WorkbookName = workbookName;

            return actionResult;
        }

        [HttpPost]
        public ActionResult Columns(string workbookName, string worksheetName, int vendorID, string vendorName)
        {
            //viewbag
            ViewBag.WorkbookName = workbookName;
            ViewBag.WorksheetName = worksheetName;
            ViewBag.VendorID = vendorID;
            ViewBag.VendorName = vendorName;

            //get all the column names in the excel worksheet
            var excel = new ExcelQueryFactory(Path.Combine(ExcelDirectory, workbookName));
            IEnumerable<System.String> excelColumnNames = excel.GetColumnNames(worksheetName);

            //get all the properties of the simpleProduct
            PropertyInfo[] simpleProductPropInfo = typeof(SimpleProduct).GetProperties();

            //set up the excel column to product propery mapping choices
            ExcelColumnToProductPropertyMappingChoices mappingChoices = new ExcelColumnToProductPropertyMappingChoices();
            mappingChoices.ExcelColumns = excelColumnNames.ToList<String>();
            mappingChoices.ProductProperties = simpleProductPropInfo.Select<PropertyInfo, String>(p => p.Name.ToString()).ToList<String>();

            //return the view
            return View(mappingChoices);
        }

        [HttpPost]
        public ActionResult BeforeImport(string workbookName, string worksheetName, int vendorID, string vendorName, SimpleProduct mappings)
        {
            //viewbag
            ViewBag.WorkbookName = workbookName;
            ViewBag.WorksheetName = worksheetName;
            ViewBag.VendorID = vendorID;
            ViewBag.VendorName = vendorName;

            string[] stringsToAvoid = { "$ DECREASE", "$ INCREASE", "NEW", "DISCONTINUED" };

            //get the excel rows
            var excel = new ExcelQueryFactory(Path.Combine(ExcelDirectory, workbookName));
            var rows = excel.Worksheet(worksheetName);

            //create vendor product list
            SimpleVendor simpleVendor = new SimpleVendor();
            simpleVendor.VendorName = vendorName;
            simpleVendor.VendorID = vendorID;

            //add the appropriate excel columns to the vendor product list            
            foreach (LinqToExcel.Row r in rows)
            {
                SimpleProduct product = new SimpleProduct();

                product.ProductName = r[mappings.ProductName];
                product.ProductDescription = r[mappings.ProductDescription];

                PropertyInfo[] properties = typeof(SimpleProduct).GetProperties();
                bool hasData = properties.Any<PropertyInfo>(pi =>
                    pi.GetValue(product) != null &&
                    pi.GetValue(product).ToString().Length > 0 &&
                    !stringsToAvoid.Any<String>(pi.GetValue(product).ToString().Contains));

                if (hasData)
                {
                    simpleVendor.Products.Add(product);
                }
            }

            return View(simpleVendor);
        }

        [HttpPost]
        public ActionResult AfterImport(string workbookName, string worksheetName, int vendorID, string vendorName, SimpleVendor simpleVendor)
        {
            //viewbag
            ViewBag.WorkbookName = workbookName;
            ViewBag.WorksheetName = worksheetName;
            ViewBag.VendorID = vendorID;
            ViewBag.VendorName = vendorName;

            //create the product list
            List<Models.Product> products = new List<Models.Product>();
            AutoMapper.Mapper.CreateMap<ViewModels.SimpleProduct, Models.Product>();
            foreach (SimpleProduct p in simpleVendor.Products)
            {
                Models.Product product = AutoMapper.Mapper.Map<ViewModels.SimpleProduct, Models.Product>(p);
                products.Add(product);
            }

            //update the database
            using (Models.VendordContext db = new Prototype.Models.VendordContext())
            {
                //get the vendor by id from the db
                //it should exist because we selected or created one earlier
                Models.Vendor vendor = db.Vendors.Where<Models.Vendor>(v => v.VendorID == simpleVendor.VendorID).FirstOrDefault<Models.Vendor>();

                //update the vendor
                vendor.VendorName = simpleVendor.VendorName;
                vendor.Products = products;
                //save                
                db.SaveChanges();
            }
            return View();
        }

        private bool UploadPostedFile(HttpPostedFileBase file, out string baseFileName)
        {
            bool success = false;
            baseFileName = null;
            if (file != null && file.ContentLength > 0)
            {
                string savePath;

                //save the excel file
                baseFileName = Path.GetFileName(file.FileName);
                savePath = Path.Combine(ExcelDirectory, baseFileName);
                file.SaveAs(savePath);
                success = true;
            }
            return success;
        }

    }
}
