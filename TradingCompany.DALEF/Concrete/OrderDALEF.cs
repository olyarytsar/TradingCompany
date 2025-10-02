﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.DAL.Concrete;
using TradingCompany.DALEF.Concrete.ctx;
using OrderDTO = TradingCompany.DTO.Order;
using OrderModel = TradingCompany.DALEF.Models.Order;

namespace TradingCompany.DALEF.Concrete
{
    public class OrderDALEF : GenericDAL<OrderDTO>
    {
        public OrderDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override OrderDTO Create(OrderDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = _mapper.Map<OrderModel>(entity);
                ctx.Orders.Add(model);
                ctx.SaveChanges();
                entity.OrderId = model.OrderId;
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Order: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Orders.Find(id);
                if (model == null) return false;
                ctx.Orders.Remove(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Order: {ex.Message}");
                return false;
            }
        }

        public override List<OrderDTO> GetAll()
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var models = ctx.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.OrderItems)
                    .OrderBy(o => o.OrderId)
                    .ToList();
                return _mapper.Map<List<OrderDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Orders: {ex.Message}");
                return new List<OrderDTO>();
            }
        }

        public override OrderDTO GetById(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.OrderItems)
                    .FirstOrDefault(o => o.OrderId == id);
                return _mapper.Map<OrderDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Order by Id: {ex.Message}");
                return null;
            }
        }

        public override OrderDTO Update(OrderDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var existing = ctx.Orders.Find(entity.OrderId);
                if (existing == null) throw new Exception("Order not found");
                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                return _mapper.Map<OrderDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Order: {ex.Message}");
                return null;
            }
        }
    }
}
