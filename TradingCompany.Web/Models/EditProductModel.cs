using TradingCompany.MVC.App.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TradingCompany.MVC.Models
{
    public class EditProductModel
    {
        public EditProductModel()
        {
            Name = string.Empty;
            Categories = new List<SelectListItem>();
            Suppliers = new List<SelectListItem>();
        }

        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length should be between 3 and 50 characters!")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1,000,000!")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required!")]
        [Range(0, 100000, ErrorMessage = "Quantity cannot be negative!")]
        [DisplayName("Stock Quantity")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "Category is required!")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Supplier is required!")]
        [DisplayName("Supplier")]
        public int SupplierId { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
    }
}