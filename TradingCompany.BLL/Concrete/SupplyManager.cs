using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concrete
{
    public class SupplyManager : ISupplyManager
    {
        private readonly IOrderDAL _orderDal;
        private readonly IOrderItemDAL _orderItemDal;
        private readonly IProductDAL _productDal;
        private readonly ISupplierDAL _supplierDal;

        public SupplyManager(IOrderDAL orderDal, IOrderItemDAL orderItemDal, IProductDAL productDal, ISupplierDAL supplierDal)
        {
            _orderDal = orderDal;
            _orderItemDal = orderItemDal;
            _productDal = productDal;
            _supplierDal = supplierDal;
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _supplierDal.GetAll();
        }

        public List<Order> GetActiveSupplyOrders()
        {
            return _orderDal.GetAll()
                            .Where(o => o.IsActive)
                            .OrderByDescending(o => o.OrderDate)
                            .ToList();
        }

        public void CreateSupplyOrder(int employeeId, int supplierId, Dictionary<int, int> productQuantities)
        {
          
            var productIds = productQuantities.Keys.ToList();

           
            var productsToCheck = _productDal.GetAll()
                                             .Where(p => productIds.Contains(p.ProductId))
                                             .ToList();

           
            if (productsToCheck.Any(p => p.SupplierId != supplierId))
            {
                throw new InvalidOperationException("Attempt to order items that do not belong to the selected supplier.");
            }

            
            decimal totalAmount = 0;

           
            foreach (var product in productsToCheck)
            {
              
                if (productQuantities.TryGetValue(product.ProductId, out int qty))
                {
                    totalAmount += product.Price * qty;
                }
            }

            
            var newOrder = new Order
            {
                EmployeeId = employeeId,
                OrderDate = DateTime.Now,
                IsActive = true,
                TotalAmount = totalAmount
            };

            var createdOrder = _orderDal.Create(newOrder);

        
            if (createdOrder != null && createdOrder.OrderId > 0)
            {
                foreach (var item in productQuantities)
                {
                    var newOrderItem = new OrderItem
                    {
                        OrderId = createdOrder.OrderId,
                        ProductId = item.Key,
                        Quantity = item.Value
                    };

                    _orderItemDal.Create(newOrderItem);
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            if (order != null && order.OrderId > 0)
            {
                _orderDal.Update(order);
            }
        }
    }
}