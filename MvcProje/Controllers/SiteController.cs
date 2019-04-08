using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProje.Controllers
{
    public class SiteController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            Session["adet"] = null;
            if (Session["userad"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }
    }
}