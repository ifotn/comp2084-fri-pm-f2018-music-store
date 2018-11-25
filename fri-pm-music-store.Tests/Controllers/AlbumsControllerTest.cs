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
        Album album;

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

            album = new Album
            {
                AlbumId = 1010,
                Title = "OneZero - OneZero",
                Price = 12.99m,
                Artist = new Artist
                {
                    ArtistId = 1100,
                    Name = "Imagine Dragons"
                }
            };

            mock.Setup(m => m.Albums).Returns(albums.AsQueryable());
            controller = new AlbumsController(mock.Object);

        }

        // GET: Albums/Index
        #region
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

        #endregion

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

        [TestMethod]
        public void DetailsValidIdLoadsAlbum()
        {
            // act
            Album result = (Album)((ViewResult)controller.Details(300)).Model;

            // assert
            Assert.AreEqual(albums[1], result);
        }
        #endregion

        //GET: Albums/Edit
        #region

        [TestMethod]
        public void EditNoIdLoadsError()
        {
            // act 
            ViewResult result = (ViewResult)controller.Edit(null);

            // assert 
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void EditIdIsValidLoadsAlbum()
        {
            // act
            Album result = (Album)((ViewResult)controller.Edit(300)).Model;

            // assert 
            Assert.AreEqual(albums[1], result);
        }

        [TestMethod]
        public void EditInvalidIdLoadsError()
        {
            // act 
            ViewResult result = (ViewResult)controller.Edit(999);

            // assert 
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void EditViewBagArtist()
        {
            // act
            ViewResult result = (ViewResult)controller.Edit(100);

            // assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }

        [TestMethod]
        public void EditViewBagGenre()
        {
            // act
            ViewResult result = (ViewResult)controller.Edit(100);

            // assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }

        [TestMethod]
        public void EditValidIdLoadsView()
        {
            // act 
            ViewResult result = (ViewResult)controller.Edit(100);

            // assert
            Assert.AreEqual("Edit", result.ViewName);
        }

        #endregion

        // POST: Albums/Edit
        #region
        //If Model valid, update album
        [TestMethod]
        public void EditModelIsValidCreateAlbum()
        {
            //act 
            Album testAlbum = new Album { AlbumId = 1 };
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Edit(testAlbum); 
            //assert
            Assert.AreEqual("Index", result.RouteValues["action"]);

             
        }
		//If Model valid, return Index view
        [TestMethod]
        public void EditModelIsValidReturnView()
        {
            //act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Edit(albums[0]);

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
          

        }
		//If Model invalid, check viewbag values artist
        [TestMethod]
        public void EditCheckViewBagValuesArtist()
        {
            //act arrange 
            controller.ModelState.AddModelError("Error", "Error thing");
            Album testAlbum = new Album { AlbumId = 1 };
            ViewResult result = (ViewResult)controller.Edit(testAlbum);

            //assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }
        
		//If Model invalid, check viewbag values genres
        [TestMethod]
        public void EditCheckViewBagValuesGenre()
        {
            //act arrange 
            controller.ModelState.AddModelError("Error", "Error thing");
            Album testAlbum = new Album { AlbumId = 1 };
            ViewResult result = (ViewResult)controller.Edit(testAlbum);

            //assert
            Assert.IsNotNull(result.ViewBag.GenreId);

        }
		//If Model invalid, return the same view
        [TestMethod]
        public void EditModelInvalidReturnView()
        {
            controller.ModelState.AddModelError("Error", "Error thing");
            Album testAlbum = new Album { AlbumId = 1 };
            

            // act
            ViewResult result = (ViewResult)controller.Edit(testAlbum);

            // assert
            Assert.AreEqual("Edit", result.ViewName);

        }
		//If Model invalid, reload the same album
        [TestMethod]
        public void EditModelInvalidReloadAlbum()
        {
            // arrange
            controller.ModelState.AddModelError("Error", "Error thing");
            Album testAlbum = new Album{ AlbumId = 1 };

            // act
            Album result = (Album)((ViewResult)controller.Edit(testAlbum)).Model;

            // assert
            Assert.AreEqual(testAlbum, result);
        }
        #endregion

        //GET: Albums/Create
        #region

        [TestMethod]
        public void CreateLoadsView()
        {
            //act
            ViewResult result = (ViewResult)controller.Create();

            //assert
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void CreateArtistViewBagNotNull()
        {
            //act
            var result = ((ViewResult)controller.Create());

            //assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }

        [TestMethod]
        public void CreateGenreViewBagNotNull()
        {
            //act
            var result = ((ViewResult)controller.Create());

            //assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }

#endregion

        // POST: Albums/Create
        #region

        // model state is not null, save new record
        [TestMethod]
        public void ModelStateNotNullSavesNewRecord()
        {
            // act
            Album copiedAlbumFromGlobal = album;
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Create(copiedAlbumFromGlobal);

            // assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        // Check ViewBag values(2) - For Artist
        [TestMethod]
        public void CheckViewBagValueForArtist()
        {
            // arrange
            Album invalidAlbum = new Album();


            // act
            controller.ModelState.AddModelError("some error name", "fake error description");
            ViewResult result = (ViewResult)controller.Create(invalidAlbum);

            // assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }

        // Check ViewBag values(2) - For Genre
        [TestMethod]
        public void CheckViewBagValueForGenre()
        {
            // arrange
            Album invalidAlbum = new Album();

            // act
            controller.ModelState.AddModelError("some error name", "fake error description");
            ViewResult result = (ViewResult)controller.Create(invalidAlbum);

            // assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }

        #endregion

        // GET: Albums/Delete
        #region

        [TestMethod]
        public void DeleteNoIdLoadsError()
        {
            // act
            ViewResult result = (ViewResult)controller.Delete(null);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteInvalidIdLoadsError()
        {
            // act
            ViewResult result = (ViewResult)controller.Delete(543);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteValidIdLoadsView()
        {
            // act
            ViewResult result = (ViewResult)controller.Delete(100);

            // assert
            Assert.AreEqual("Delete", result.ViewName);
        }

        [TestMethod]
        public void DeleteValidIdLoadsAlbum()
        {
            // act
            // is different because not interested in viewresult, but in data that comes with view
            Album result = (Album)((ViewResult)controller.Delete(300)).Model;

            // assert
           Assert.AreEqual(albums[1], result);
        }

        #endregion

        // POST: Albums/Delete
        #region

        [TestMethod]
        public void DeleteConfirmedIdLoadsError()
        {
            //Act
            ViewResult result = (ViewResult)controller.DeleteConfirmed(-1);

            //Assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedNoIdLoadsError()
        {
            //Act
            ViewResult result = (ViewResult)controller.DeleteConfirmed(null);

            //Assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedDataSuccessful()
        {
            //Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.DeleteConfirmed(100);

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

    }
}
