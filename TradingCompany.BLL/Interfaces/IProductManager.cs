using System.Collections.Generic;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IProductManager
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        List<Product> GetProducts(string searchTerm, string sortOrder);
        List<Product> GetProductsBySupplier(int supplierId);
        Product CreateProduct(Product product);
        Product UpdateProduct(Product product);
        bool DeleteProduct(int id);
        List<Category> GetAllCategories();
        List<Supplier> GetAllSuppliers();
    }
}
