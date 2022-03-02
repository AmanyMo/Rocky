using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocky.Data;
using Rocky.Models;

using Rocky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            IEnumerable<Product> products= _db.Product;
            return View(products);
           
        }
        public IActionResult Details()
        {
            IEnumerable<Product> products = _db.Product;

            return View(products);
        }

        //get create & update
        public IActionResult Upsert(int? id)
        {
            // using View bag so the view is not strongly typed view so should use model

            //Product product = new Product();
            //IEnumerable<SelectListItem> listItems = _db.Category.Select(a => new SelectListItem()
            //{
            //    Text = a.Name,
            //    Value = a.ID.ToString()
            //});
            //ViewBag.listitems = listItems;

            //use model view to be strongly typed view
            ProductVM productvm = new ProductVM()
            {
                product = new Product(),
                ListItems = _db.Category.Select(a => new SelectListItem()
                {
                    Text=a.Name,
                    Value=a.ID.ToString()
                })
            };
            //create so there is no id
            if (id==null)
            {
                return View(productvm);
            }
            else
            {
                //it's an edit and there is an id
                 productvm.product = _db.Product.Find(id);
                 return View(productvm);

            }

            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int? id,Product product)
        {
            //no id no product (empty form)
            if (!ModelState.IsValid && id == null)
            {
               return BadRequest();
            }

            //edit
            else
            {
                if (ModelState.IsValid && id != null)
                {
                    _db.Product.Update(product);
                    _db.SaveChanges();
                    //return RedirectToAction(nameof(Index));
                }

                //now we have 2 scenarios
                //1.no product was sent but id exists.
                //..this mean that it's edit and user delete data fields and click save btn
                if (!ModelState.IsValid && id!=null)
                {
                    Product product1 = _db.Product.Find(id);
                  return View(product1 );
                }

                //2.there is a product but with no id value
                //..this is a create 

                if (id == null && product != null)
                {
                    _db.Product.Add(product);
                    _db.SaveChanges();

                   // return RedirectToAction(nameof(Index));
                }
            }


            return RedirectToAction(nameof(Index));
        }

    }
}
