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

        // GET: Store/AddToCart
        public ActionResult AddToCart(int AlbumId)
        {
            // identify the user's cart
            GetCartId();
            //string CurrentCartId = Session["CartId"].ToString();

            // save new item
            Cart CartItem = new Cart
            {
                CartId = Session["CartId"].ToString(),
                AlbumId = AlbumId,
                Count = 1,
                DateCreated = DateTime.Now
            };

            db.Carts.Add(CartItem);
            db.SaveChanges();

            // show the cart page
            return RedirectToAction("ShoppingCart");
        }

        private void GetCartId()
        {
            // if we don't already have a cart id in the session
            if (Session["CartId"] == null)
            {
                if (User.Identity.Name == "")
                {
                    // user is anonymous, generate random unique string
                    Session["CartId"] = Guid.NewGuid().ToString();                
                }
                else
                {
                    // is the user logged in?
                    Session["CartId"] = User.Identity.Name;
                }
            } 
        }
    }
}