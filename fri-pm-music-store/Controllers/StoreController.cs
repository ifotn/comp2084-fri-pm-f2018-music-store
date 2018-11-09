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

        // GET: Store/ShoppingCart
        public ActionResult ShoppingCart()
        {
            // get current user's cart items
            GetCartId();
            string CurrentCartId = Session["CartId"].ToString();

            var CartItems = db.Carts.Where(c => c.CartId == CurrentCartId).ToList();
            return View(CartItems);
        }

        // GET: Store/Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            MigrateCart();
            return View();
        }

        // POST: Store/Checkout
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(FormCollection values)
        {
            // create new order from form values
            Order order = new Order();
            TryUpdateModel(order);

            // set the 4 auto properties
            order.Username = User.Identity.Name;
            order.Email = User.Identity.Name;
            order.OrderDate = DateTime.Now;

            // get the cart items & calc the order total
            decimal CartTotal;
            var CartItems = db.Carts.Where(c => c.CartId == User.Identity.Name).ToList();

            CartTotal = (from c in CartItems
                         select (int)c.Count * c.Album.Price).Sum();
            order.Total = CartTotal;

            // save the order
            db.Orders.Add(order);
            db.SaveChanges();

            // save the items
            foreach (Cart item in CartItems)
            {
                OrderDetail od = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    Quantity = item.Count,
                    UnitPrice = item.Album.Price,
                    OrderId = order.OrderId
                };
                db.OrderDetails.Add(od);
            }

            db.SaveChanges();

            // remove the items from the user's cart
            foreach (Cart item in CartItems)
            {
                db.Carts.Remove(item);
            }

            db.SaveChanges();

            // show confirmation page
            return RedirectToAction("Details", "Orders", new { id = order.OrderId });
        }

        private void MigrateCart()
        {
            if (!String.IsNullOrEmpty(Session["CartId"].ToString()) && User.Identity.IsAuthenticated) {
                if (Session["CartId"].ToString() != User.Identity.Name)
                    {
                        // user shopped anonymously but now has logged in to checkout
                        string CurrentCartId = Session["CartId"].ToString();
                        var CartItems = db.Carts.Where(c => c.CartId == CurrentCartId).ToList();

                        foreach (Cart item in CartItems)
                        {
                            item.CartId = User.Identity.Name;
                        }
                        db.SaveChanges();

                        // change the session variable
                        Session["CartId"] = User.Identity.Name;
                    }
            }
        }
    }
}