using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProje.Models
{
    public class urunlerkat
    {
        public int urunid { get; set; }
        public string urunad { get; set; }
        public decimal urunfiyat { get; set; }
        public int urunstokadet { get; set; }
        public string urunresim { get; set; }
        public string urunozellik { get; set; }
        public int urunkategoriid { get; set; }
        public string urunkategoriad { get; set; }
       
    }
}