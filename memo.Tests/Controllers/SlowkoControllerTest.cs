using memo.Controllers;
using memo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web.Security;

namespace memo.Tests.Controllers
{
    [TestClass]
    public class SlowkoControllerTest
    {
        [TestMethod]
        public void zalogujViewTest()
        {
            //arange
            SlowkoController panel = new SlowkoController();

            //act
            var result = panel.Zaloguj() as ViewResult;

            // assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void rolaUzytkowniaTest()
        {
            SlowkoController panel = new SlowkoController();
            string wynik1 = panel.rolaUzytkownia("nemo");
            string wynik2 = panel.rolaUzytkownia("pawel");
            string wynik3 = panel.rolaUzytkownia("kowalski");
            string wynik4 = panel.rolaUzytkownia("kowalski1");


            Assert.AreEqual(wynik1, "administrator");
            Assert.AreEqual(wynik2, "zwykly");
            Assert.AreEqual(wynik3, "zwykly");
            Assert.AreEqual(wynik4, "administrator");
        }

        [TestMethod]
        public void generatePairTest()
        {
            SlowkoController panel = new SlowkoController();
            Pytanie wynik = panel.generatePair();

            Assert.IsNotNull(wynik);

        }


    }



}

