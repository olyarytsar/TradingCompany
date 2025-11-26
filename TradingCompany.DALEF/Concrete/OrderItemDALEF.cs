using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using OrderItemDTO = TradingCompany.DTO.OrderItem;
using OrderItemModel = TradingCompany.DALEF.Models.OrderItem;


namespace TradingCompany.DALEF.Concrete
{
    public class OrderItemDALEF : GenericDAL<OrderItemDTO>, IOrderItemDAL
    {
        public OrderItemDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override OrderItemDTO Create(OrderItemDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = _mapper.Map<OrderItemModel>(entity);
                ctx.OrderItems.Add(model);
                ctx.SaveChanges();
                entity.OrderItemId = model.OrderItemId;
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating OrderItem: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.OrderItems.Find(id);
                if (model == null) return false;
                ctx.OrderItems.Remove(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting OrderItem: {ex.Message}");
                return false;
            }
        }

        public override List<OrderItemDTO> GetAll()
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var models = ctx.OrderItems
                    .Include(oi => oi.Product)
                    .Include(oi => oi.Order)
                    .OrderBy(oi => oi.OrderItemId)
                    .ToList();
                return _mapper.Map<List<OrderItemDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving OrderItems: {ex.Message}");
                return new List<OrderItemDTO>();
            }
        }

        public override OrderItemDTO GetById(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.OrderItems
                    .Include(oi => oi.Product)
                    .Include(oi => oi.Order)
                    .FirstOrDefault(oi => oi.OrderItemId == id);
                return _mapper.Map<OrderItemDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving OrderItem by Id: {ex.Message}");
                return null;
            }
        }

        public override OrderItemDTO Update(OrderItemDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var existing = ctx.OrderItems.Find(entity.OrderItemId);
                if (existing == null) throw new Exception("OrderItem not found");
                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                return _mapper.Map<OrderItemDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating OrderItem: {ex.Message}");
                return null;
            }
        }
    }
}
