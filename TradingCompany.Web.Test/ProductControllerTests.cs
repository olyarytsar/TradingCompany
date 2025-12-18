using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.MVC.Controllers;
using TradingCompany.MVC.Models;

namespace TradingCompany.MVC.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductManager> _mockManager;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<ProductController>> _mockLogger;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            _mockManager = new Mock<IProductManager>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(_mockManager.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Test]
        public void Index_ReturnsViewWithProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Name = "P1" }, new Product { Name = "P2" } };
            _mockManager.Setup(m => m.GetAllProducts()).Returns(products);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(products, result.Model);
        }

        [Test]
        public void Details_ProductExists_ReturnsViewWithProduct()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "P1" };
            _mockManager.Setup(m => m.GetProductById(1)).Returns(product);

            // Act
            var result = _controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product, result.Model);
        }

        [Test]
        public void Details_ProductNull_ReturnsNotFound()
        {
            // Arrange
            _mockManager.Setup(m => m.GetProductById(1)).Returns((Product)null);

            // Act
            var result = _controller.Details(1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

       
        [Test]
        public void Create_Get_ReturnsViewWithModel()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EditProductModel>(result.Model);
        }

      
        [Test]
        public void Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new EditProductModel { Name = "New Product", Price = 100 };
            var productDto = new Product { Name = "New Product" };

            _mockMapper.Setup(m => m.Map<Product>(model)).Returns(productDto);
            _mockManager.Setup(m => m.CreateProduct(productDto)).Returns(productDto);

            // Act
            var result = _controller.Create(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var model = new EditProductModel { Name = "" }; 
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Create(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }

        [Test]
        public void Create_Post_ManagerReturnsNull_ReturnsViewWithError()
        {
            // Arrange
            var model = new EditProductModel { Name = "Bad Product" };
            var productDto = new Product();

            _mockMapper.Setup(m => m.Map<Product>(model)).Returns(productDto);
            
            _mockManager.Setup(m => m.CreateProduct(productDto)).Returns((Product)null);

            // Act
            var result = _controller.Create(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
            var error = _controller.ModelState[string.Empty].Errors[0].ErrorMessage;
            Assert.AreEqual("Database failed to save product.", error);
        }

        [Test]
        public void Edit_Get_ProductExists_ReturnsViewWithModel()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "P1" };
            var model = new EditProductModel { ProductId = 1, Name = "P1" };

            _mockManager.Setup(m => m.GetProductById(1)).Returns(product);
            _mockMapper.Setup(m => m.Map<EditProductModel>(product)).Returns(model);

            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }

  
        [Test]
        public void Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new EditProductModel { ProductId = 1, Name = "Updated" };
            var productDto = new Product { ProductId = 1, Name = "Updated" };

            _mockMapper.Setup(m => m.Map<Product>(model)).Returns(productDto);
            _mockManager.Setup(m => m.UpdateProduct(productDto)).Returns(productDto);

            // Act
            var result = _controller.Edit(1, model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void Edit_Post_ExceptionThrown_ReturnsView()
        {
            // Arrange
            var model = new EditProductModel { ProductId = 1 };
            _mockManager.Setup(m => m.UpdateProduct(It.IsAny<Product>())).Throws(new Exception());

            // Act
            var result = _controller.Edit(1, model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }

        [Test]
        public void Delete_Get_ProductExists_ReturnsView()
        {
            // Arrange
            var product = new Product { ProductId = 1 };
            _mockManager.Setup(m => m.GetProductById(1)).Returns(product);

            // Act
            var result = _controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product, result.Model);
        }

        [Test]
        public void DeleteConfirmed_Success_RedirectsToIndex()
        {
            // Arrange
            _mockManager.Setup(m => m.DeleteProduct(1)).Returns(true);

            // Act
            var result = _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void DeleteConfirmed_ProductLinkedToOrders_ReturnsViewWithError()
        {
            // Arrange
            int id = 1;
            var product = new Product { ProductId = id, Name = "Linked Product" };

            
            _mockManager.Setup(m => m.DeleteProduct(id)).Returns(false);
            _mockManager.Setup(m => m.GetProductById(id)).Returns(product);

            // Act
            var result = _controller.DeleteConfirmed(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product, result.Model);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual("Cannot delete: Product is linked to orders.", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }
    }
}