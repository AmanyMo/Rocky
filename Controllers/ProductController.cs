using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly App_DbContext _db;
        public ProductController(App_DbContext db)
        {
            _db = db;   
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
           IEnumerable<Product> products= _db.Product;
            return View(products);
        }
    }
}
