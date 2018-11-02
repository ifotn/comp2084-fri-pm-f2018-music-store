using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fri_pm_music_store.Models;

namespace fri_pm_music_store.Controllers
{
    public class StoreController : Controller
    {
        // db connection - this is done automatically in scaffolded controllers
        private MusicStoreModel db = new MusicStoreModel();

        // GET: Store
        public ActionResult Index()
        {
            var genres = db.Genres.OrderBy(g => g.Name).ToList();
            return View(genres);
        }

        // GET: Store/Albums?genre=Name
        public ActionResult Albums(string genre)
        {
            // get albums for selected genre
            var albums = db.Albums.Where(a => a.Genre.Name == genre).ToList();

            // load view and pass the album list for display
            return View(albums);
        }

        // GET: Store/Product
        public ActionResult Product(string ProductName)
        {
            ViewBag.ProductName = ProductName;
            return View();
        }
    }
}