using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vendord.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        #region Custom Errors

        public ViewResult Trouble()
        {
            return View("Error");
        }

        public ViewResult Confused()
        {
            return View("Error");
        }

        #endregion
    }
}
