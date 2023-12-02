using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using BulkyBookWeb.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _IUnitOfWork;
        public CategoryController(IUnitOfWork IUnitOfWork)
        {
            _IUnitOfWork = IUnitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> categorylist = _IUnitOfWork.Category.GetAll().ToList();
            return View(categorylist);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category objCategory)
        {
            if (objCategory.Name == objCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The display order can not exactly mathe name");
            }
            if (ModelState.IsValid)
            {
                _IUnitOfWork.Category.Add(objCategory);
                _IUnitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            return View(objCategory);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFormDb = _IUnitOfWork.Category.Get(u => u.Id == id);
            //Category? categoryFormDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFormDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }
        [HttpPost]
        public IActionResult Edit(Category objCategory)
        {
            if (ModelState.IsValid)
            {
                _IUnitOfWork.Category.update(objCategory);
                _IUnitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");

            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFormDb = _IUnitOfWork.Category.Get(u => u.Id == id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Deletepost(int? id)
        {
            Category? objcategory = _IUnitOfWork.Category.Get(u => u.Id == id);
            if (objcategory == null)
            {
                return NotFound();
            }
            _IUnitOfWork.Category.Remove(objcategory);
            _IUnitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
