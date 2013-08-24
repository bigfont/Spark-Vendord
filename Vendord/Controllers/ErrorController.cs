using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vendord.Controllers
{
    public class ErrorController : Controller
    {                
        // error/index
        public ActionResult Index()
        {
            return View();
        }

        // shared/error
        public ActionResult Error()
        {
            return View();
        }

        // error/notfound
        public ActionResult NotFound()
        {
            return View();
        }
    }
}
