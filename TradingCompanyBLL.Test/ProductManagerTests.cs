using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TradingCompany.BLL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Tests
{
    [TestFixture]
    public class ProductManagerTests
    {
        private Mock<IProductDAL> _productDalMock;
        private ProductManager _sut;

        [SetUp]
        public void SetUp()
        {
            _productDalMock = new Mock<IProductDAL>(MockBehavior.Strict);
            _sut = new ProductManager(_productDalMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _productDalMock.VerifyAll();
        }

        [Test]
        public void GetAllProducts_ShouldReturnListFromDal()
        {
            // Arrange
            var list = new List<Product>();
            _productDalMock.Setup(d => d.GetAll()).Returns(list);

            // Act
            var result = _sut.GetAllProducts();

            // Assert
            Assert.That(result, Is.SameAs(list));
        }

        [Test]
        public void GetProductById_ShouldReturnProductFromDal()
        {
            // Arrange
            var product = new Product { ProductId = 1 };
            _productDalMock.Setup(d => d.GetById(1)).Returns(product);

            // Act
            var result = _sut.GetProductById(1);

            // Assert
            Assert.That(result, Is.SameAs(product));
        }

        [Test]
        public void GetProducts_ShouldFilterBySearchTerm()
        {
            // Arrange
            var p1 = new Product { Name = "Apple iPhone", Category = new Category { CategoryName = "Phone" } };
            var p2 = new Product { Name = "Samsung Galaxy", Category = new Category { CategoryName = "Phone" } };
            var p3 = new Product { Name = "Laptop", Category = new Category { CategoryName = "PC" } };

            _productDalMock.Setup(d => d.GetAll()).Returns(new List<Product> { p1, p2, p3 });

            // Act
            var result = _sut.GetProducts("Samsung", "Name_Asc");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Samsung Galaxy"));
        }

        [Test]
        public void GetProducts_ShouldSortByPriceDescending()
        {
            // Arrange
            var p1 = new Product { Name = "Cheap", Price = 100 };
            var p2 = new Product { Name = "Expensive", Price = 900 };
            var p3 = new Product { Name = "Medium", Price = 500 };

            _productDalMock.Setup(d => d.GetAll()).Returns(new List<Product> { p1, p2, p3 });

            // Act
            var result = _sut.GetProducts("", "Price_Desc");

            // Assert
            Assert.That(result[0].Price, Is.EqualTo(900));
            Assert.That(result[1].Price, Is.EqualTo(500));
            Assert.That(result[2].Price, Is.EqualTo(100));
        }

        [Test]
        public void GetProductsBySupplier_ShouldFilterAndSortByName()
        {
            // Arrange
            var p1 = new Product { SupplierId = 1, Name = "B Product" };
            var p2 = new Product { SupplierId = 2, Name = "A Product" }; 
            var p3 = new Product { SupplierId = 1, Name = "A Product" };

            _productDalMock.Setup(d => d.GetAll()).Returns(new List<Product> { p1, p2, p3 });

            // Act
            var result = _sut.GetProductsBySupplier(1);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("A Product")); 
            Assert.That(result[1].Name, Is.EqualTo("B Product"));
        }
    }
}