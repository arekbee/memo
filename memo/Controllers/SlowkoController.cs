﻿using System.Linq;
using System.Web.Mvc;
using memo.Models;
using System.Web.Security;
using System;
using System.Web;
using System.Data.Entity;
using System.Collections.Generic;

namespace memo.Controllers
{
    public class SlowkoController : Controller
    {
        //private int angPol = 2; //narazie nie potrzebne
        private int polAng = 1;
        public enum Rola { NIE_ZALOGOWANY = 0, ADMIN = 1, ZWYKLY = 2 };

        public bazaEntities db = new bazaEntities();


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Rola = "nieZalogowny";
            //return Redirect("dictionary.cambridge.org/media/english-polish/us_pron/t/thr/threa/threat.mp3"); //tylko do przetestowania ;)
            string username = null;

            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                //pobranie nazwy zalogowanego uzytkownika
                username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                @ViewBag.Login = username;
            }

            ViewBag.Rola = "nieZalogowany";
            if (username != null)
            {
                if (sprawdzRole() == Rola.ADMIN)
                {
                    ViewBag.Rola = "admin";
                }
                else
                {
                    ViewBag.Rola = "zwykly";
                }
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
            if (ModelState.IsValid)
            {
                var czyIstnieje = db.uzytkownik.Where(x => x.nazwa == model.nazwa).Count();
                if (czyIstnieje > 0)
                {
                    ViewBag.Uzytkownik = "Użytkownik o podanym loginie już istnieje!";
                    return View(model);
                }

                uzytkownik nUzytkownik = new uzytkownik { nazwa = model.nazwa.Trim(), haslo = model.haslo.Trim(), rola = (int)Rola.ZWYKLY, ustawienia = polAng };
                db.uzytkownik.Add(nUzytkownik);


                statystykaUzytkownika nStatystyka = new statystykaUzytkownika { nazwa = model.nazwa.Trim(), dobreOdpowiedzi = 0, zleOdpowiedzi = 0 };
                db.statystykaUzytkownika.Add(nStatystyka);
                db.SaveChanges();
            }

            return RedirectToAction("Zaloguj");
        }


        [HttpGet]
        public ActionResult Zaloguj()
        {
            /*if (User.Identity.Name.Count() > 0)
            {
                FormsAuthentication.SignOut();
            }*/
            return View();
        }

        [HttpPost]
        public ActionResult Zaloguj(LogowanieModel model, string returnUrl)
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
                        FormsAuthentication.SetAuthCookie(login, true);
                        ViewBag.WiadLogowanie = "Zalogowano";
                        if (rolaUzytkownia(login).Equals("administrator"))
                        {
                            ViewBag.Rola = "admin";
                        }
                        else
                        {
                            ViewBag.Rola = "zwykly";
                        }

                        if ((Url.IsLocalUrl(returnUrl)) && (returnUrl.Length > 1) && (returnUrl.StartsWith("/")) && (!returnUrl.StartsWith("//")) && (!returnUrl.StartsWith("/\\")))
                        {
                            return Redirect(returnUrl);
                        }

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

        public string rolaUzytkownia(string user)
        {
            if (user != null)
            {
                var uzytkownik = db.uzytkownik.Where(x => x.nazwa == user);
                if (uzytkownik.Count() > 0)
                {
                    return uzytkownik.First().rola1.nazwa.Trim();
                }
            }

            return "";
        }

        //sprawdza czy jest login zalogowany
        private bool czyZalogowany()
        {
            if (User.Identity.Name.Count() > 0) //zalogowany
            {
                return true;
            }
            return false;
        }

        //sprawdz role uzytkownika
        public Rola sprawdzRole()
        {
            ViewBag.Rola = "nieZalogowany";
            Rola rola = Rola.NIE_ZALOGOWANY;
            if (czyZalogowany())
            {
                rola = Rola.ZWYKLY;
                ViewBag.Rola = "zwykly";
                if (rolaUzytkownia(User.Identity.Name).Equals("administrator"))
                {
                    ViewBag.Rola = "admin";
                    rola = Rola.ADMIN;
                }
            }
            return rola;
        }

        [HttpGet]
        public ActionResult Kokpit()
        {
            switch (sprawdzRole()) 
            {
                case Rola.ADMIN:
                    {
                        List<KokpitModel> uzytkownicy = new List<KokpitModel>();

                        var users = db.uzytkownik;
                        foreach (uzytkownik p in users)
                        {
                            var model = new KokpitModel();
                            model.login = p.nazwa.Trim();
                            model.rola = p.rola1.nazwa.Trim();
                            model.opcja = p.ustawieniaZagadki.opis.Trim();

                            uzytkownicy.Add(model);
                        }
                        return View(uzytkownicy);
                    }
                case Rola.ZWYKLY:
                    {
                        ViewBag.Komunikat = "Użytkownik '" + User.Identity.Name + "' nie posiada wystarczających uprawnień";
                        break;
                    }
                case Rola.NIE_ZALOGOWANY:
                    {
                        ViewBag.Komunikat = "Musisz się zalogować.";
                        break;
                    }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Kokpit(KokpitModel model)
        {
            db.uzytkownik.Where(x => x.nazwa == model.login).Single().rola = Convert.ToInt32(model.rola);
            db.uzytkownik.Where(x => x.nazwa == model.login).Single().ustawienia = Convert.ToInt32(model.opcja);
            db.SaveChanges();

            return RedirectToAction("Kokpit");
        }

        [HttpGet]
        public ActionResult Pytanie()
        {
            sprawdzRole();
            return View(generatePair());
        }

        public Pytanie generatePair()
        {
            int ustawienieUzytkownika = userSetting(User.Identity.Name);
            Pytanie nowy = new Pytanie();
            try
            {
                int max = db.slowko.Count();
                int losuj = new Random().Next(0, max - 1);
                memo.Models.slowko slowko = db.slowko.ToList().ElementAt(losuj);

                switch (ustawienieUzytkownika)
                {
                    case 1:
                        nowy.pytanie = slowko.pl;
                        nowy.poprawna_odpowiedz = slowko.eng;
                        break;
                    case 2:
                        nowy.pytanie = slowko.eng;
                        nowy.poprawna_odpowiedz = slowko.pl;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            return nowy;
        }

        public int userSetting(string user)
        {
            if (user != null)
            {
                var uzytkownik = db.uzytkownik.Where(x => x.nazwa == user);
                if (uzytkownik.Count() > 0)
                {
                    return uzytkownik.First().ustawienia;
                }
            }
            return 0; //błąd
        }

        [HttpPost]
        public ActionResult Pytanie(Pytanie model)
        {
            sprawdzRole();
            if (model.pytanie == null)
            {
                return View(generatePair());//Pytanie();
            }
            var uzytkownik = db.uzytkownik.Where(x => x.nazwa == User.Identity.Name);
            if (uzytkownik.Count() > 0)
            {
                if (model.odpowiedz_uzytkownika == null)
                {
                    return View(model);
                }
                string pytanie = model.pytanie;
                string odpowiedz_uzytkownika = model.odpowiedz_uzytkownika.ToString().Trim();
                string poprawna_odpowiedz = model.poprawna_odpowiedz.ToString().Trim();

                ViewBag.pytanie = pytanie;
                ViewBag.odpowiedz = poprawna_odpowiedz;

                if (odpowiedz_uzytkownika.Equals(poprawna_odpowiedz))
                {
                    ViewBag.czy_poprawna_odpowiedz = true;
                    uzytkownik.First().statystykaUzytkownika.dobreOdpowiedzi++;
                }
                else
                {
                    ViewBag.czy_poprawna_odpowiedz = false;
                    uzytkownik.First().statystykaUzytkownika.zleOdpowiedzi++;
                }
                ViewBag.dobre_odpowiedzi = uzytkownik.First().statystykaUzytkownika.dobreOdpowiedzi;
                ViewBag.zle_odpowiedzi = uzytkownik.First().statystykaUzytkownika.zleOdpowiedzi;
                db.SaveChanges();

            }

            return View();
        }


        [HttpGet]
        public ActionResult Panel()
        {
            if (sprawdzRole() != Rola.NIE_ZALOGOWANY)
            {
                PanelViewModel panel = new PanelViewModel();
                var uzytkownik = db.uzytkownik.Where(x => x.nazwa == User.Identity.Name).First();
                panel.login = uzytkownik.nazwa.Trim();
                panel.opcja = uzytkownik.ustawieniaZagadki.opis.Trim();
                return View(panel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Panel(PanelViewModel model)
        {
            db.uzytkownik.Where(x => x.nazwa == model.login).Single().ustawienia = Convert.ToInt32(model.opcja);
            db.SaveChanges();
            return RedirectToAction("Panel");
        }

        [HttpGet]
        public ActionResult Statystyki()
        {
            sprawdzRole();
            statystykaUzytkownika stat = db.uzytkownik.Where(x => x.nazwa == User.Identity.Name).First().statystykaUzytkownika;
            return View(stat);
        }

        [HttpGet]
        public ActionResult NoweSlowo()
        {
            sprawdzRole();
            return View();
        }

        [HttpPost]
        public ActionResult NoweSlowo(slowko nowe)
        {
            sprawdzRole();
            if(db.slowko.Where(x => x.eng.Trim() == nowe.eng.Trim()).Count() > 0)
            {
                ViewBag.Istnieje = "Wpisane słowo już istnieje w bazie.";
                return View(nowe);
            }
            if(ModelState.IsValid)
            {
                db.slowko.Add(nowe);
                db.SaveChanges();
                return RedirectToAction("NoweSlowo");
            }
            return View(nowe);
        }


        public bool Test(string napis)
        {
            if (napis != null && napis.Equals("admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult Test(IEnumerable<uzytkownik> model)
        {
            return View();
        }

        
    }
}