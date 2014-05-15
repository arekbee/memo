using memo.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web.Security;

namespace memo.Tests.Controllers
{
    [TestClass]
    public class SlowkoControllerTest
    {
        [TestMethod]
        public void ZalogujTest()
        {
            //arange
            SlowkoController panel = new SlowkoController();

            //act
            var result = panel.Zaloguj() as ViewResult;

            // assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void TestTest()
        {
            //arange
            SlowkoController panel = new SlowkoController();
            string login = "admin";

            //act
            bool result1 = panel.Test(login);

            // assert
            Assert.AreEqual<bool>(result1, true);
        }
    }



}

