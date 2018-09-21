using fri_pm_music_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fri_pm_music_store.Controllers
{
    public class AlbumsController : Controller
    {
        // GET: Albums
        public ActionResult Index()
        {
            // create mock list of type Album
            var albums = new List<Album>();

            for (int i = 1; i <= 10; i++)
            {
                albums.Add(new Album { Title = "Album " + i.ToString() });
            }

            // add albums to ViewBag for display
            //ViewBag.albums = albums;

            // pass the strongly-typed list directly to the view
            return View(albums);
        }
    }
}