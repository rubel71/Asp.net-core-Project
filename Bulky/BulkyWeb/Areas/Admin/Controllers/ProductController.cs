using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using BulkyBookWeb.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _IUnitofWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _IUnitofWork = unitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> listProduct = _IUnitofWork.Product.GetAll(includeproperties:"Category").ToList();
           
            return View(listProduct);

        }
        public IActionResult Upsert(int? id)
        {
           
            ProductVM productVM = new ProductVM()
            {
                Categorylist= _IUnitofWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()             
            };
            if(id==null || id==0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _IUnitofWork.Product.Get(u => u.Id==id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM objproduct,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string productpath = Path.Combine(wwwRootPath, @"images\product");
                    if(!string.IsNullOrEmpty(objproduct.Product.ImageUrl))
                    {
                        //delete old image
                        var oldimagepath = Path.Combine(wwwRootPath, objproduct.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldimagepath))
                        {
                            System.IO.File.Delete(oldimagepath);
                        }
                    }
                    using(var filestream=new FileStream(Path.Combine(productpath,fileName),FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    objproduct.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if(objproduct.Product.Id==0)
                {
                    _IUnitofWork.Product.Add(objproduct.Product);
                }
                else
                {
                    _IUnitofWork.Product.Update(objproduct.Product);
                }
                
                _IUnitofWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                objproduct.Categorylist = _IUnitofWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }
            return View(objproduct);
        }
      
       
        #region API Calles
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> listProduct = _IUnitofWork.Product.GetAll(includeproperties: "Category").ToList();
            return Json(new { data = listProduct});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productTobeDeleted = _IUnitofWork.Product.Get(u => u.Id == id);
            if (productTobeDeleted == null)
            {
                return Json(new { success = false, message="Error while deleting"});
            }
            var oldimagepath = Path.Combine(_WebHostEnvironment.WebRootPath, productTobeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldimagepath))
            {
                System.IO.File.Delete(oldimagepath);
            }
            _IUnitofWork.Product.Remove(productTobeDeleted);
            _IUnitofWork.Save();
            return Json(new {success=true,message="Delete successful"});
        }
        #endregion
    }
}
