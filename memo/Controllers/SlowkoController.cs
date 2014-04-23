using System.Linq;
using System.Web.Mvc;
using memo.Models;
using System.Web.Security;
using System;
using System.Web;

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
            string username = null;
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                //let us take out the username now                
                username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                @ViewBag.Login = username;
            }

            if(username != null)
            {
                return View();
            }

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
                string login = model.nazwa.Trim();
                var user = db.uzytkownik.Where(x => x.nazwa == login);
                int czyIstnieje = user.Count();
                if (czyIstnieje > 0) //istnieje użytkownik
                {
                    string haslo = user.First().haslo.ToString().Trim();
                    string hasloUzytkownika = model.haslo.ToString().Trim(); //haslo podane przez użytkownika

                    if (haslo.Equals(hasloUzytkownika)) //poprawnie zalogowany użytkownik
                    {
                        ViewBag.Login = login;
                        ViewBag.WiadLogowanie = "Zalogowano";
                        FormsAuthentication.SetAuthCookie(login, true);
                        return RedirectToAction("Index");
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

        
        public ActionResult Wyloguj()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
	}
}