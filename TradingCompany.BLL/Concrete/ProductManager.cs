using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concrete
{
    public class ProductManager : IProductManager
    {
        private readonly IProductDAL _productDal;
        private readonly ICategoryDAL _categoryDal;
        private readonly ISupplierDAL _supplierDal;

   
        public ProductManager(IProductDAL productDal, ICategoryDAL categoryDal, ISupplierDAL supplierDal)
        {
            _productDal = productDal;
            _categoryDal = categoryDal;
            _supplierDal = supplierDal;
        }

        public List<Product> GetAllProducts()
        {
            return _productDal.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _productDal.GetById(id);
        }

        public List<Product> GetProducts(string searchTerm, string sortOrder)
        {
            var products = _productDal.GetAll();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                products = products.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    (p.Category != null && p.Category.CategoryName.ToLower().Contains(searchTerm)) ||
                    (p.Supplier != null && p.Supplier.Brand.ToLower().Contains(searchTerm))
                ).ToList();
            }

            switch (sortOrder)
            {
                case "Name_Asc": products = products.OrderBy(p => p.Name).ToList(); break;
                case "Name_Desc": products = products.OrderByDescending(p => p.Name).ToList(); break;
                case "Price_Asc": products = products.OrderBy(p => p.Price).ToList(); break;
                case "Price_Desc": products = products.OrderByDescending(p => p.Price).ToList(); break;
                case "Quantity_Asc": products = products.OrderBy(p => p.QuantityInStock).ToList(); break;
                case "Quantity_Desc": products = products.OrderByDescending(p => p.QuantityInStock).ToList(); break;
                default: products = products.OrderBy(p => p.ProductId).ToList(); break;
            }

            return products;
        }

        public List<Product> GetProductsBySupplier(int supplierId)
        {
            return _productDal.GetAll()
                              .Where(p => p.SupplierId == supplierId)
                              .OrderBy(p => p.Name)
                              .ToList();
        }

        public Product CreateProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
           
            return _productDal.Create(product);
        }

        public Product UpdateProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            return _productDal.Update(product);
        }

        public bool DeleteProduct(int id)
        {
            return _productDal.Delete(id);
        }

        public List<Category> GetAllCategories()
        {
            return _categoryDal.GetAll();
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _supplierDal.GetAll();
        }

       
    }
}
