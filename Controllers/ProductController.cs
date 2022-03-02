using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Rocky.Data;
using Rocky.Models;

using Rocky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly App_DbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(App_DbContext db,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
            #region webbag
            // using View bag so the view is not strongly typed view so should use model

            //Product product = new Product();
            //IEnumerable<SelectListItem> listItems = _db.Category.Select(a => new SelectListItem()
            //{
            //    Text = a.Name,
            //    Value = a.ID.ToString()
            //});
            //ViewBag.listitems = listItems;
            #endregion 

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
        public IActionResult Upsert(ProductVM productvm)
        {
           
            if (!ModelState.IsValid )
            {
               return BadRequest();
            }

            //Model is valid
            else
            {
                #region upsertoldfashion
                ////create
                //if ()
                //{
                //    _db.Product.Update(product);
                //    _db.SaveChanges();
                //    //return RedirectToAction(nameof(Index));
                //}

                ////now we have 2 scenarios
                ////1.no product was sent but id exists.
                ////..this mean that it's edit and user delete data fields and click save btn
                //if (!ModelState.IsValid && id!=null)
                //{
                //    Product product1 = _db.Product.Find(id);
                //  return View(product1 );
                //}

                ////2.there is a product but with no id value
                ////..this is a create 

                //if (id == null && product != null)
                //{
                //    _db.Product.Add(product);
                //    _db.SaveChanges();

                //   // return RedirectToAction(nameof(Index));
                //}
                #endregion

                //to add img first:add it to wwwroot file to save in server then save its name to db
                //  to add it to wwwroot :it's a static file & to get path use webhostenvironment interface 
                //---to deal with webhostenvironment  interface should inject it and register in DI---
                //first: catch file(img)from request form cus files route on form request
                //second: get path of www
                //third: copy this file to a file streaam which save it in wwwroot

                var files=HttpContext.Request.Form.Files;
                string webRootPath =_webHostEnvironment.WebRootPath;   

                //creating
                if (productvm.product.ID==0) 
                {
                    string upload = webRootPath + WebConstant.ImgPath;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(upload, filename + extension),FileMode.Create))
                    {
                        //add img to wwwroot
                        files[0].CopyTo(filestream);    
                    }
                         //add product to db
                    productvm.product.Image = filename + extension;
                    _db.Product.Add(productvm.product);
                }

                //edit
                else    
                {
                    //first make sure if img was updated the old img was deleted firstly & set the new img
                    //second if no files come(no img update) so update the rest of product props only
                     Product objfromDb= _db.Product.AsNoTracking().FirstOrDefault(a=> a.ID==productvm.product.ID);
                    if (objfromDb.Image==null)
                    {
                        objfromDb.Image = "";
                    }
                  
                    //---first
                    if (files.Count()>0)
                    {
                        string upload = webRootPath + WebConstant.ImgPath;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                       string  oldimgpath = Path.Combine(upload, objfromDb.Image);
                        if (System.IO.File.Exists(oldimgpath))
                        {
                            System.IO.File.Delete(oldimgpath);
                        }
                         //add img to wwwroot use filestream
                        using (var filestream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {                     
                            files[0].CopyTo(filestream);
                        }
                        //set img prop
                        productvm.product.Image = filename + extension;
                    }
                    else
                    //there is no file (no img)
                    {
                        productvm.product.Image = objfromDb.Image;
                    }

                    _db.Product.Update(productvm.product);
                }


                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
