using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using fri_pm_music_store.Models;

namespace fri_pm_music_store.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AlbumsController : Controller
    {
        // disable the automatic db connection 
        //private MusicStoreModel db = new MusicStoreModel();

        private IAlbumsMock db;

        // default constructor, use the live db
        public AlbumsController()
        {
            this.db = new EFAlbums();
        }

        // mock constructor
        public AlbumsController(IAlbumsMock mock)
        {
            this.db = mock;
        }

        // GET: Albums
        public ActionResult Index()
        {
            var albums = db.Albums.Include(a => a.Artist).Include(a => a.Genre).OrderBy(a => a.Artist.Name).ThenBy(a => a.Title);
            //return View(albums.ToList());
        
            return View("Index", albums.ToList());
        }

        // GET: Albums/Details/5
        //[OverrideAuthorization]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                // scaffold code
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return View("Error");
            }

            // old scaffold code - doesn't work with our mock
            // Album album = db.Albums.Find(id);

            // new code that works with both ef & mock repositories
            Album album = db.Albums.SingleOrDefault(a => a.AlbumId == id);

            if (album == null)
            {
                // scaffold code
                // return HttpNotFound();
                return View("Error");
            }
            return View("Details", album);
        }

        //// GET: Albums/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ArtistId = new SelectList(db.Artists.OrderBy(a => a.Name), "ArtistId", "Name");
        //    ViewBag.GenreId = new SelectList(db.Genres.OrderBy(g => g.Name), "GenreId", "Name");
        //    return View();
        //}

        //// POST: Albums/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // upload album cover if there is one
        //        if (Request.Files.Count > 0)
        //        {
        //            var file = Request.Files[0];

        //            if (file.FileName != null && file.ContentLength > 0)
        //            {
        //                // get file path dynamically
        //                string path = Server.MapPath("~/Content/Images/") + file.FileName;
        //                file.SaveAs(path);

        //                album.AlbumArtUrl = "/Content/Images/" + file.FileName;
        //            }
        //        }

        //        db.Albums.Add(album);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
        //    ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
        //    return View(album);
        //}

        //// GET: Albums/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Album album = db.Albums.Find(id);
        //    if (album == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ArtistId = new SelectList(db.Artists.OrderBy(a => a.Name), "ArtistId", "Name", album.ArtistId);
        //    ViewBag.GenreId = new SelectList(db.Genres.OrderBy(g => g.Name), "GenreId", "Name", album.GenreId);
        //    return View(album);
        //}

        //// POST: Albums/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // upload album cover if there is one
        //        if (Request.Files.Count > 0)
        //        {
        //            var file = Request.Files[0];

        //            if (file.FileName != null && file.ContentLength > 0)
        //            {
        //                // get file path dynamically
        //                string path = Server.MapPath("~/Content/Images/") + file.FileName;
        //                file.SaveAs(path);

        //                album.AlbumArtUrl = "/Content/Images/" + file.FileName;
        //            }
        //        }

        //        db.Entry(album).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
        //    ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
        //    return View(album);
        //}

        //// GET: Albums/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Album album = db.Albums.Find(id);
        //    if (album == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(album);
        //}


        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            //Scaffolded code - no longer needed
            //Album album = db.Albums.Find(id);
            //db.Albums.Remove(album);
            //db.SaveChanges();

            Album album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
            if (album == null)
            {
                return View("Error");
            }
            else
            {
                db.Delete(album);
                return RedirectToAction("Index");
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
