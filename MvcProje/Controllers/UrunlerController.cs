using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProje.Models;
namespace MvcProje.Controllers
{
    public class UrunlerController : Controller
    {
        // GET: Urunle
        public kodlama1_mvcEntities veri = new kodlama1_mvcEntities();
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["userid"]);
            var sepetteki = veri.sepet.Where(a => a.sepetuserid == id);
            var sepettekiadet = (from nesne in sepetteki select nesne.sepeturunid).Distinct().Count();
            Session["adet"] = sepettekiadet;
            var urunlerim = from n1 in veri.urunler join n2 in veri.Kategori on n1.urunkategori equals n2.katid select new urunlerkat {
                urunad=n1.urunad,
                urunfiyat=n1.urunfiyat,
                urunid=n1.urunid,
                urunkategoriad=n2.katad,
                urunkategoriid=n2.katid,
                urunozellik=n1.urunozellik,
                urunresim=n1.urunresim,
                urunstokadet=n1.urunstokadet
            };
            return View(urunlerim.ToList());
        }
        [HttpPost]
        public ActionResult Index(FormCollection degerler)
        {
            int adet = Convert.ToInt32(degerler["adet"]);
            int id = Convert.ToInt32(degerler["id"]);
            int userid = Convert.ToInt32(Session["userid"]);
            sepet yeniUrun = new sepet();
            yeniUrun.sepeturunid = id;
            yeniUrun.sepetuserid = userid;
            yeniUrun.sepeturunadet = adet;
            veri.sepet.Add(yeniUrun);
            veri.SaveChanges();
            var sepetteki = veri.sepet.Where(a => a.sepetuserid == userid);
            var sepettekiadet = (from nesne in sepetteki select nesne.sepeturunid).Distinct().Count();
            Session["adet"] = sepettekiadet;
          
            return RedirectToAction("Index");
        }
        public ActionResult SepetGoruntule()
        {
            int userid = Convert.ToInt32(Session["userid"]);
            var sepettekiler = (from nesne in veri.urunler
                                join nesne2 in veri.sepet on nesne.urunid equals nesne2.sepeturunid
                                where nesne2.sepetuserid == userid
                                select new sepette
                                {
                                    urunad = nesne.urunad,
                                    urunfiyat = nesne.urunfiyat,
                                    urunresim = nesne.urunresim,
                                    urunid = nesne.urunid,
                                    urunadet = (from n1 in veri.sepet where n1.sepeturunid == nesne2.sepeturunid select n1.sepeturunadet).Sum()

                                }).Distinct();
            return View(sepettekiler.ToList());
        }

        public ActionResult Sil(int? id)
        {
            if (Session["userad"] != null)
            {
                if (id != null)
                {
                    int userid = Convert.ToInt32(Session["userid"]);
                    var urun = (from nesne in veri.sepet where nesne.sepeturunid == id && nesne.sepetuserid==userid select nesne).ToList();
                    //veri.sepet.RemoveRange(urun);
                    //veri.SaveChanges();
                    foreach (var item in urun)
                    {
                        veri.sepet.Remove(item);
                        veri.SaveChanges();
                    }
                   
                }

                return RedirectToAction("SepetGoruntule");
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }
    }
}