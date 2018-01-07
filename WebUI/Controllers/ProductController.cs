using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository repository;

        public ProductController(IProductsRepository productRepository)
        {
            this.repository = productRepository;
        }

        // GET: Product
        public ActionResult List()
        {
            int count = 0;
            List<Product> list = repository.Products.ToList();
            if (list != null)
                count = list.Count;
            return View(repository.Products);
        }
    }
}