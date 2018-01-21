using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductsRepository repository;

        public CartController(IProductsRepository repo)
        {
            repository = repo;
        }

        public RedirectToRouteResult AddToCart(int productid, string returnUrl)
        {
            Product product = repository.Products
                              .FirstOrDefault(p => p.ProductID == productid);
            if(product != null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        // GET: Cart
        public ActionResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if(cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}