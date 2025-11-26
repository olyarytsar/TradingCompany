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
    }
}
