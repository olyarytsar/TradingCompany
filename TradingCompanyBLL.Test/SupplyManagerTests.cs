using System;
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
    public class SupplyManagerTests
    {
        private Mock<IOrderDAL> _orderDalMock;
        private Mock<IOrderItemDAL> _orderItemDalMock;
        private Mock<IProductDAL> _productDalMock;
        private Mock<ISupplierDAL> _supplierDalMock;
        private SupplyManager _sut;

        [SetUp]
        public void SetUp()
        {
            _orderDalMock = new Mock<IOrderDAL>(MockBehavior.Strict);
            _orderItemDalMock = new Mock<IOrderItemDAL>(MockBehavior.Strict);
            _productDalMock = new Mock<IProductDAL>(MockBehavior.Strict);
            _supplierDalMock = new Mock<ISupplierDAL>(MockBehavior.Strict);

            _sut = new SupplyManager(
                _orderDalMock.Object,
                _orderItemDalMock.Object,
                _productDalMock.Object,
                _supplierDalMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _orderDalMock.VerifyAll();
            _orderItemDalMock.VerifyAll();
            _productDalMock.VerifyAll();
            _supplierDalMock.VerifyAll();
        }

        [Test]
        public void GetAllSuppliers_ShouldReturnListFromDal()
        {
            // Arrange
            var suppliers = new List<Supplier> { new Supplier { SupplierId = 1, Brand = "Nail" } };
            _supplierDalMock.Setup(d => d.GetAll()).Returns(suppliers);

            // Act
            var result = _sut.GetAllSuppliers();

            // Assert
            Assert.That(result, Is.SameAs(suppliers));
        }

        [Test]
        public void GetActiveSupplyOrders_ShouldReturnOnlyActiveOrdersSortedByDate()
        {
            // Arrange
            var order1 = new Order { OrderId = 1, IsActive = true, OrderDate = new DateTime(2023, 1, 1) };
            var order2 = new Order { OrderId = 2, IsActive = false, OrderDate = new DateTime(2023, 1, 2) };
            var order3 = new Order { OrderId = 3, IsActive = true, OrderDate = new DateTime(2023, 1, 3) };

            _orderDalMock.Setup(d => d.GetAll()).Returns(new List<Order> { order1, order2, order3 });

            // Act
            var result = _sut.GetActiveSupplyOrders();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].OrderId, Is.EqualTo(3)); 
            Assert.That(result[1].OrderId, Is.EqualTo(1));
        }

        [Test]
        public void CreateSupplyOrder_ShouldThrow_WhenProductDoesNotBelongToSupplier()
        {
            // Arrange
            int supplierId = 10;
            var products = new List<Product>
            {
                new Product { ProductId = 100, SupplierId = 20, Price = 100 } 
            };
            var cart = new Dictionary<int, int> { { 100, 1 } };

            _productDalMock.Setup(p => p.GetAll()).Returns(products);

            // Act / Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _sut.CreateSupplyOrder(1, supplierId, cart));
            Assert.That(ex!.Message, Contains.Substring("items that do not belong"));
        }

        [Test]
        public void CreateSupplyOrder_ShouldCalculateTotalAndCreateOrderAndItems()
        {
            // Arrange
            int employeeId = 5;
            int supplierId = 10;

            var product1 = new Product { ProductId = 1, SupplierId = 10, Price = 50 };
            var product2 = new Product { ProductId = 2, SupplierId = 10, Price = 100 };

            var allProducts = new List<Product> { product1, product2 };
            var cart = new Dictionary<int, int>
            {
                { 1, 2 }, 
                { 2, 1 } 
            };

            var createdOrder = new Order { OrderId = 999 }; // Order returned by DB

            // Setup calls
            _productDalMock.Setup(p => p.GetAll()).Returns(allProducts);

            _orderDalMock.Setup(o => o.Create(It.Is<Order>(x =>
                x.EmployeeId == employeeId &&
                x.TotalAmount == 200 &&
                x.IsActive == true
            ))).Returns(createdOrder);

            _orderItemDalMock.Setup(oi => oi.Create(It.Is<OrderItem>(x => x.OrderId == 999))).Returns(new OrderItem());

            // Act
            _sut.CreateSupplyOrder(employeeId, supplierId, cart);

            // Assert
            
            _orderItemDalMock.Verify(oi => oi.Create(It.IsAny<OrderItem>()), Times.Exactly(2));
        }
        [Test]
        
        public void CreateSupplyOrder_ShouldCalculateCorrectTotalAmount()
        {
           
            int empId = 1, supId = 1;
            var prod = new Product { ProductId = 10, SupplierId = 1, Price = 50.5m };

            var cart = new Dictionary<int, int> { { 10, 4 } };

            _productDalMock.Setup(p => p.GetAll()).Returns(new List<Product> { prod });

            _orderDalMock.Setup(o => o.Create(It.Is<Order>(x => x.TotalAmount == 202.0m)))
                         .Returns(new Order { OrderId = 100 });

            _orderItemDalMock.Setup(oi => oi.Create(It.IsAny<OrderItem>()))
                             .Returns(new OrderItem());

            // Act
            _sut.CreateSupplyOrder(empId, supId, cart);

        }
        [Test]
        public void UpdateOrder_ShouldCallDal_WithUpdatedStatus()
        {
            // Arrange
            var order = new Order
            {
                OrderId = 15,
                IsActive = true,
                TotalAmount = 500
            };

            _orderDalMock
                .Setup(d => d.Update(It.IsAny<Order>()))
                .Returns((Order o) => o);

            order.IsActive = false;
            _sut.UpdateOrder(order);

            _orderDalMock.Verify(d => d.Update(It.Is<Order>(o => o.IsActive == false)), Times.Once);
        }
    }
}