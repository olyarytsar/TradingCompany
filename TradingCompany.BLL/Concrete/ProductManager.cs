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

        public ProductManager(IProductDAL productDal)
        {
            _productDal = productDal;
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
                case "Name_Asc":
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "Name_Desc":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                case "Price_Asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "Price_Desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "Quantity_Asc":
                    products = products.OrderBy(p => p.QuantityInStock).ToList();
                    break;
                case "Quantity_Desc": 
                    products = products.OrderByDescending(p => p.QuantityInStock).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.ProductId).ToList();
                    break;
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
    }
}