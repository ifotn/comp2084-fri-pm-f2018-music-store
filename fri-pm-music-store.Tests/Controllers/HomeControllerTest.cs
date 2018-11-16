using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using fri_pm_music_store;
using fri_pm_music_store.Controllers;

namespace fri_pm_music_store.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Some content about us", result.ViewBag.Message);
        }

        [TestMethod]
        public void AboutLoadsView()
        {
            // arrange
            HomeController controller = new HomeController();

            // act
            ViewResult result = controller.About() as ViewResult;

            // assert
            Assert.AreEqual("About-Us", result.ViewName);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ContactViewBagValid()
        {
            // arrange
            HomeController controller = new HomeController();

            // act
            ViewResult viewResult = controller.Contact() as ViewResult;

            // assert
            Assert.AreEqual("Your contact page.", viewResult.ViewBag.Message);
        }

        [TestMethod]
        public void Gallery()
        {
            // arrange
            HomeController controller = new HomeController();

            // act
            ViewResult result = controller.Gallery() as ViewResult;

            // assert
            Assert.IsNotNull(result);
        }
    }
}
