using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProje.Models;

namespace MvcProje.Controllers
{
    public class HomeController : Controller
    {
        public kodlama1_mvcEntities veri = new kodlama1_mvcEntities();
        // GET: Home
        public ActionResult Index()
        {
            int userid = Convert.ToInt32(Session["userid"]);
            var sepettekiadet = veri.sepet.Where(a => a.sepetuserid == userid).Count();
            Session["adet"] = sepettekiadet;
            return View();
        }
        [HttpPost]
        public ActionResult Index(userlar kisi)
        {
            var varolankisi = (from nesne in veri.userlar where nesne.userad == kisi.userad && nesne.usersifre == kisi.usersifre select nesne);
            if (varolankisi.Any())
            {
                Session["userad"] = varolankisi.FirstOrDefault().userad;
                Session["userid"] = varolankisi.FirstOrDefault().userid;
                if (varolankisi.FirstOrDefault().userrol == 1)
                {                 
                    return Redirect("~/Site/Index");
                }
                else
                {                   
                    return Redirect("~/Admin/Index");
                }
            }
            else
            {
                ViewBag.hata = "Kullanıcı Adı ve Şifre Hatalı";
                return View();
            }
           
        }
        public ActionResult Kayit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Kayit(userlar kisi)
        {
            kisi.userrol = 1;
            veri.userlar.Add(kisi);
            veri.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}