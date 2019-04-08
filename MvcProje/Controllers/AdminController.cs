using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProje.Models;

namespace MvcProje.Controllers
{
    public class AdminController : Controller
    {
        public kodlama1_mvcEntities veri = new kodlama1_mvcEntities();

        public ActionResult girisKontrol()
        {
            if (Session["userad"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return girisKontrol();
        }
        public ActionResult Kisiler()
        {
            //girisKontrol();
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                var kisilerim = (from n1 in veri.userlar
                                 join n2 in veri.roller on n1.userrol equals n2.rolid
                                 select new userverol
                                 {
                                     userid = n1.userid,
                                     userad = n1.userad,
                                     usersifre = n1.usersifre,
                                     userrolid = n2.rolid,
                                     userrolad = n2.rolad
                                 });
                ViewBag.sayi = kisilerim.Count() - 1;
                var roller = veri.roller.ToList();
                ViewBag.rol = roller;
                return View(kisilerim.ToList());
                
            }
        }
        [HttpPost]
        public ActionResult KisiGuncelle(List<userverol> kisilerim)
        {
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                foreach (var item in kisilerim)
                {
                    var varolankisi = (from nesne in veri.userlar where nesne.userid == item.userid select nesne).FirstOrDefault();
                    varolankisi.userad = item.userad;
                    varolankisi.userrol = item.userrolid;
                    varolankisi.usersifre = item.usersifre;
                    veri.SaveChanges();

                }
                return RedirectToAction("Kisiler");
            }
        }
        public ActionResult KisiSil(int? id)
        {
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                if (id != null)
                {
                    var kisi = veri.userlar.Find(id);
                    veri.userlar.Remove(kisi);
                    veri.SaveChanges();
                }
                else
                {
                    ViewBag.silmehatasi = "Kişi silmek için seçilmesi gerekir";
                }
                return RedirectToAction("Kisiler");
            }
        }
        public ActionResult KisiEkle()
        {
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                ViewBag.roller = veri.roller.ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult KisiEkle(userlar kisi)
        {
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                ViewBag.roller = veri.roller.ToList();
                if (kisi.userad == "" || kisi.usersifre == "" || kisi.userrol == 0)
                {
                    ViewBag.hata = "Kullanıcı Adı, Şifre veya Rol Alanları Boş Bırakılamaz";
                    return View();
                }
                else
                {
                    var kisiVarMi = from nesne in veri.userlar where nesne.userad == kisi.userad select nesne;
                    if (kisiVarMi.Any())
                    {
                        ViewBag.hata = "Kayıtlı Kullanıcıdır.";
                        return View();
                    }
                    else
                    {
                        veri.userlar.Add(kisi);
                        veri.SaveChanges();
                        return RedirectToAction("Kisiler");
                    }
                }
            }
        }
        public ActionResult KisiProfil(int? id)
        {
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                if (id != null)
                {

                    var kisi = from nesne in veri.userlar
                               join nesne2 in veri.roller on nesne.userrol equals nesne2.rolid
                               where nesne.userid == id
                               select new userverol
                               {
                                   userad = nesne.userad,
                                   usersifre = nesne.usersifre,
                                   userid = nesne.userid,
                                   userrolad = nesne2.rolad,
                                   userrolid = nesne2.rolid
                               };
                    if (kisi.Any())
                    {
                        return View(kisi.FirstOrDefault());
                    }
                    else
                    {
                        return Redirect("~/Home/Index");
                    }
                }
                else
                {
                    return Redirect("~/Home/Index");
                }
            }
        }
        public ActionResult KendiSil(int id)
        {
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                var kisi = veri.userlar.Find(id);
                veri.userlar.Remove(kisi);
                veri.SaveChanges();
                return Redirect("~/Home/Index");
            }
        }
        public ActionResult KendiGuncelle(int? id)
        {
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                if (id == null)
                {
                    return RedirectToAction("KisiProfil");
                }
                else
                {
                    var kisi = from n1 in veri.userlar
                               join n2 in veri.roller on n1.userrol equals n2.rolid
                               where n1.userid == id
                               select new userverol
                               {
                                   userad = n1.userad,
                                   usersifre = n1.usersifre,
                                   userid = n1.userid,
                                   userrolad = n2.rolad,
                                   userrolid = n2.rolid
                               };
                    ViewBag.roller = veri.roller.ToList();
                    return View(kisi.FirstOrDefault());
                }
            }
        }
        [HttpPost]
        public ActionResult KendiGuncelle(userlar kisi)
        {
            if (Session["userad"] == null)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                var varolanKisi = veri.userlar.Find(kisi.userid);
                varolanKisi.userad = kisi.userad;

                varolanKisi.usersifre = kisi.usersifre;
                varolanKisi.userrol = kisi.userrol;
                veri.SaveChanges();
                return RedirectToAction("KisiProfil/" + kisi.userid);
            }
        }
        public ActionResult Cikis()
        {
            if (Session["userad"] != null)
            {
                Session["userad"] = null;
                Session["userid"] = null;
                Session.Clear();
            }
                return Redirect("~/Home/Index");
            
        }
        public ActionResult KatSil(int? id)
        {
            if (Session["userad"] != null)
            {
                if (id != null)
                {
                    var kategori = veri.Kategori.Find(id);
                    veri.Kategori.Remove(kategori);
                    veri.SaveChanges();
                }
                return RedirectToAction("Kategoriler");
            }
            else
            {
                return Redirect("~/Home/Index");
            }

        }

        public ActionResult KategoriEkle()
        {
            return girisKontrol();
        }
        [HttpPost]
        public ActionResult KategoriEkle(Kategori kate)
        {
            if (Session["userad"] != null)
            {
                var varmi = from nesne in veri.Kategori where nesne.katad == kate.katad select nesne;
                if (varmi.Any())
                {
                    ViewBag.hata = "Kategori Mevcut";
                    return View();
                }
                else
                {
                    veri.Kategori.Add(kate);
                    veri.SaveChanges();
                    return RedirectToAction("Kategoriler");
                }
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }
        public ActionResult Kategoriler()
        {
            if (Session["userad"] != null)
            {
                ViewBag.adet = veri.Kategori.Count();
                return View(veri.Kategori.ToList());
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }
        public ActionResult Urunler(int? id)
        {
            if (Session["userad"] != null)
            {
                ViewBag.kategoriliste = veri.Kategori.ToList();
                if (id != null)
                {
                    ViewBag.kate = (from n1 in veri.urunler join n2 in veri.Kategori on n1.urunkategori equals n2.katid where n1.urunid==id select n2.katad).FirstOrDefault();
                    var urun = veri.urunler.Find(id);
                    ViewBag.guncelle = "Ürün Güncelle";
                    Session["guncelle"] = 1;
                    return View(urun);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return Redirect("~/Home/Index");
            }
            
           
        }
        [HttpPost]
        public ActionResult Urunler(urunler urun)
        {
            if (Session["guncelle"] != null)
            {
                var varOlanUrun = (from nesne in veri.urunler where nesne.urunid == urun.urunid select nesne).FirstOrDefault();
                varOlanUrun.urunad = urun.urunad;
                varOlanUrun.urunfiyat = urun.urunfiyat;
                varOlanUrun.urunkategori = urun.urunkategori;
                varOlanUrun.urunozellik = urun.urunozellik;
                if (urun.urunresim != null)
                {
                    varOlanUrun.urunresim = urun.urunresim;
                }
                varOlanUrun.urunstokadet = urun.urunstokadet;
                veri.SaveChanges();
                Session["guncelle"] = null;
            }
            else
            {
                veri.urunler.Add(urun);
                veri.SaveChanges();
            }
            return RedirectToAction("UrunListele");
        }
        public ActionResult UrunListele()
        {
            var urunlerim = from n1 in veri.urunler join n2 in veri.Kategori on n1.urunkategori equals n2.katid select new urunlerkat {
                urunid=n1.urunid,
                urunad=n1.urunad,
                urunfiyat=n1.urunfiyat,
                urunozellik=n1.urunozellik,
                urunkategoriad=n2.katad,
                urunkategoriid=n2.katid,
                urunresim=n1.urunresim,
                urunstokadet=n1.urunstokadet

            };
            return View(urunlerim.ToList());
        }
      
    }
}