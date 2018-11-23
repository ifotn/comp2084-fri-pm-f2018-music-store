using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// add ref to web project controllers
using fri_pm_music_store.Controllers;
using System.Web.Mvc;
using Moq;
using fri_pm_music_store.Models;
using System.Collections.Generic;
using System.Linq;

namespace fri_pm_music_store.Tests.Controllers
{
    [TestClass]
    public class AlbumsControllerTest
    {
        // global variables needed for multiple tests in this class
        AlbumsController controller;
        Mock<IAlbumsMock> mock;
        List<Album> albums;
 
        [TestInitialize]
        public void TestInitalize()
        {
            // this method runs automatically before each individual test

            // create a new mock data object to hold a fake list of albums
            mock = new Mock<IAlbumsMock>();

            // populate mock list
            albums = new List<Album>
            {
                new Album { AlbumId = 100, Title = "One Hundred", Price = 6.99m, Artist = new Artist {
                        ArtistId = 1000, Name = "Singer One"
                    }
                },
                new Album { AlbumId = 300, Title = "Three Hundred", Price = 7.99m, Artist = new Artist {
                        ArtistId = 1001, Name = "Singer Two"
                    }
                },
                new Album { AlbumId = 200, Title = "Two Hundred", Price = 8.99m, Artist = new Artist {
                        ArtistId = 1001, Name = "Singer Two"
                    }
                }
            };

            // put list into mock object and pass it to the albums controller
            //albums.OrderBy(a => a.Artist.Name).ThenBy(a => a.Title);

            mock.Setup(m => m.Albums).Returns(albums.AsQueryable());
            controller = new AlbumsController(mock.Object);

        }

        [TestMethod]
        public void IndexLoadsView()
        {
            // arrange - now moved to TestInitialize for code re-use
            //AlbumsController controller = new AlbumsController();

            // act
            ViewResult result = controller.Index() as ViewResult;

            // assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void IndexReturnsAlbums()
        {
            // act
            var result = (List<Album>)((ViewResult)controller.Index()).Model;

            // assert
            CollectionAssert.AreEqual(albums, result);
        }

        // GET: Albums/Details
        #region
        
        [TestMethod]
        public void DetailsNoIdLoadsError()
        {
            // act
            ViewResult result = (ViewResult)controller.Details(null);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DetailsInvalidIdLoadsError()
        {
            // act
            ViewResult result = (ViewResult)controller.Details(534);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }
        #endregion

    }
}
