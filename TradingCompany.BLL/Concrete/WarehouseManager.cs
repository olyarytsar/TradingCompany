using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concrete
{
    public class WarehouseManager : IWarehouseManager
    {
        private readonly ProductDALEF _productDal;
        private readonly OrderDALEF _orderDal;
        private readonly OrderItemDALEF _orderItemDal;
        private readonly SupplierDALEF _supplierDal;

        public WarehouseManager(ProductDALEF productDal, OrderDALEF orderDal, OrderItemDALEF orderItemDal, SupplierDALEF supplierDal)
        {
            _productDal = productDal;
            _orderDal = orderDal;
            _orderItemDal = orderItemDal;
            _supplierDal = supplierDal;
        }

        public List<Product> ViewProducts(string search = null, string sortBy = null)
        {
            var products = _productDal.GetAll();

            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(sortBy))
            {
                products = sortBy.ToLower() switch
                {
                    "name" => products.OrderBy(p => p.Name).ToList(),
                    "price" => products.OrderBy(p => p.Price).ToList(),
                    "quantity" => products.OrderBy(p => p.QuantityInStock).ToList(),
                    _ => products
                };
            }

            Console.WriteLine("Products retrieved successfully.");
            return products;
        }

        public Order CreateOrder(Employee employee, int supplierId, Dictionary<int, int> productQuantities)
        {
            var supplier = _supplierDal.GetById(supplierId);
            if (supplier == null)
            {
                Console.WriteLine("Supplier not found.");
                return null;
            }

            var order = new Order
            {
                EmployeeId = employee.EmployeeId,
                OrderDate = DateTime.Now,
                IsActive = true,
                TotalAmount = 0m
            };

            order = _orderDal.Create(order);

            foreach (var pq in productQuantities)
            {
                var product = _productDal.GetById(pq.Key);
                if (product == null)
                {
                    Console.WriteLine($"Product with ID {pq.Key} not found.");
                    continue;
                }

                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = pq.Key,
                    Quantity = pq.Value,
                    Product = product
                };

                _orderItemDal.Create(orderItem);
                order.TotalAmount += product.Price * pq.Value;
            }

            order = _orderDal.Update(order);
            Console.WriteLine($"Order {order.OrderId} created successfully for supplier {supplier.Brand}.");
            return order;
        }

        public List<Order> ViewActiveOrders()
        {
            var orders = _orderDal.GetAll().Where(o => o.IsActive).ToList();
            Console.WriteLine("Active orders retrieved successfully.");
            return orders;
        }

        public Order UpdateOrder(Order order)
        {
            var existingOrder = _orderDal.GetById(order.OrderId);
            if (existingOrder == null)
            {
                Console.WriteLine($"Order {order.OrderId} not found.");
                return null;
            }

            existingOrder.IsActive = order.IsActive;
            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.OrderDate = order.OrderDate;

            var updatedOrder = _orderDal.Update(existingOrder);
            Console.WriteLine($"Order {order.OrderId} updated successfully.");
            return updatedOrder;
        }
    }
}
