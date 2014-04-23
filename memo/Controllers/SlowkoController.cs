using System.Linq;
using System.Web.Mvc;
using memo.Models;

namespace memo.Controllers
{
    public class SlowkoController : Controller
    {
        private int zwykly = 2;
        private int administrator = 1;

        private int angPol = 2;
        private int polAng = 1;

        private bazaEntities db = new bazaEntities();

        [HttpGet]
        public ActionResult Index()
        {

            //string wynik = db.slowko.Where(x => x.pl == "pies").ToList().ElementAt(0).eng;
            //string wynik = db.uzytkownik.Where(x => x.nazwa == "admin").ToList().ElementAt(0).ustawieniaZagadki.opis;
            /*
            uzytkownik nowy = new uzytkownik { nazwa = "u1", haslo = "1234", rola = 2, ustawienia = 1 };
            db.uzytkownik.Add(nowy);
            db.SaveChanges();

            ViewBag.Pytanie = wynik;*/

            //RejestracjaModel nowy = new RejestracjaModel { nazwa = "u2", haslo = "1234", hasloPowtorka = "1234", regulamin = true };
            //Rejestracja(nowy);


            return RedirectToAction("Zaloguj");
        }


        [HttpGet]
        public ActionResult Rejestracja()
        {
            return View();
        }


        
        [HttpPost]
        public ActionResult Rejestracja(RejestracjaModel model)
        {
            if(ModelState.IsValid)
            {
                var czyIstnieje = db.uzytkownik.Where(x => x.nazwa == model.nazwa).Count();
                if(czyIstnieje > 0)
                {
                    ViewBag.Uzytkownik = "Użytkownik o podanym loginie już istnieje!";
                    return View(model);
                }

                uzytkownik nUzytkownik = new uzytkownik { nazwa = model.nazwa.Trim(), haslo = model.haslo.Trim(), rola = zwykly, ustawienia = polAng };
                db.uzytkownik.Add(nUzytkownik);


                statystykaUzytkownika nStatystyka = new statystykaUzytkownika { nazwa = model.nazwa.Trim(), dobreOdpowiedzi = 0, zleOdpowiedzi = 0 };
                db.statystykaUzytkownika.Add(nStatystyka);
                db.SaveChanges();
            }

            return View("Zaloguj");
        }


        [HttpGet]
        public ActionResult Zaloguj()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Zaloguj(LogowanieModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = db.uzytkownik.Where(x => x.nazwa == model.nazwa.Trim());
                int czyIstnieje = user.Count();
                if (czyIstnieje > 0) //istnieje użytkownik
                {
                    string haslo = user.First().haslo.ToString().Trim();
                    string hasloUzytkownika = model.haslo.ToString().Trim();

                    if (haslo.Equals(hasloUzytkownika)) //poprawnie zalogowany użytkownik
                    {
                        ViewBag.Login = user.First().nazwa.Trim();
                        ViewBag.WiadLogowanie = "Zalogowano";
                        return View("Index");
                    }
                }
                ViewBag.WiadLogowanie = "Podałeś zły login lub hasło.";
            }
            else
            {
                ViewBag.WiadLogowanie = "Nie wpisałeś loginu lub hasła.";
            }

            return View(model);
        }







	}
}