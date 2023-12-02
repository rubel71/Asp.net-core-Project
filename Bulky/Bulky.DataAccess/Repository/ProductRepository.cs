using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Models;
using BulkyBookWeb.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBookWeb.DataAccess.Repository
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Product objProduct)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == objProduct.Id);
            if(objFromDb!=null)
            {
                objFromDb.Title = objProduct.Title;
                objFromDb.Description = objProduct.Description;
                objFromDb.ISBN = objProduct.ISBN;
                objFromDb.Price = objProduct.Price;
                objFromDb.Price50 = objProduct.Price50;
                objFromDb.Price100 = objProduct.Price100;
                objFromDb.ListPrice = objProduct.ListPrice;
                objFromDb.Author = objProduct.Author;
                objFromDb.CategoryId = objProduct.CategoryId;
                if(objProduct.ImageUrl!=null)
                {
                    objFromDb.ImageUrl = objProduct.ImageUrl;
                }

            }
        }
    }
}
