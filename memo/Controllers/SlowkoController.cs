using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using memo.Models;

namespace memo.Controllers
{
    public class SlowkoController : Controller
    {
        private bazaEntities db = new bazaEntities();
        public ActionResult Index()
        {

            //string wynik = db.slowko.Where(x => x.pl == "pies").ToList().ElementAt(0).eng;
            string wynik = db.uzytkownik.Where(x => x.nazwa == "admin").ToList().ElementAt(0).ustawieniaZagadki.opis;

            ViewBag.Pytanie = wynik;

            return View();
        }
	}
}