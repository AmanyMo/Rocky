using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class CategoryController : Controller
    {
        private readonly App_DbContext _db;

        public CategoryController(App_DbContext app_Db)
        {
            _db = app_Db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.Category;
            return View(categories);
        }

        public IActionResult Details()
        {
            IEnumerable<Category> categories = _db.Category;

            return View(categories);
        }
        //Get
      [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
      [HttpPost]
        public IActionResult Create(Category categoryObj)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
         IQueryable<string> nameexist = _db.Category.Select(n=>n.Name);
            if (nameexist.Contains(categoryObj.Name))
            {
                return Content("this Category Exists already");
            }
            _db.Category.Add(categoryObj);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Category category = _db.Category.Find(id);
            if (category==null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            else
            {
               //Category category1= _db.Category.Find(id);
               // if (category1 == null)
               // {
               //     return Content("this element does not exist");  
               // }
               // else { 
                    _db.Category.Update(category);
                    _db.SaveChanges();
                //}
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           Category category= _db.Category.Find(id);

            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
       // [Route("https://localhost:44314/Category/Delete/{id}")]
        public IActionResult Deletepost(int id)
        {
            Category category = _db.Category.Find(id);
            _db.Category.Remove(category);
            return RedirectToAction(nameof(Index));

        }
    }
}
