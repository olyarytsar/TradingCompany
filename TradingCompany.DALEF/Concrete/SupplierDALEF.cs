using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using SupplierDTO = TradingCompany.DTO.Supplier;
using SupplierModel = TradingCompany.DALEF.Models.Supplier;

namespace TradingCompany.DALEF.Concrete
{
    public class SupplierDALEF : GenericDAL<SupplierDTO>, ISupplierDAL
    {
        public SupplierDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override SupplierDTO Create(SupplierDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = _mapper.Map<SupplierModel>(entity);
                ctx.Suppliers.Add(model);
                ctx.SaveChanges();
                entity.SupplierId = model.SupplierId;
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Supplier: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Suppliers.Find(id);
                if (model == null) return false;
                ctx.Suppliers.Remove(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Supplier: {ex.Message}");
                return false;
            }
        }

        public override List<SupplierDTO> GetAll()
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var models = ctx.Suppliers.OrderBy(s => s.SupplierId).ToList();
                return _mapper.Map<List<SupplierDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Suppliers: {ex.Message}");
                return new List<SupplierDTO>();
            }
        }

        public override SupplierDTO GetById(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Suppliers.Find(id);
                return _mapper.Map<SupplierDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Supplier by Id: {ex.Message}");
                return null;
            }
        }

        public override SupplierDTO Update(SupplierDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var existing = ctx.Suppliers.Find(entity.SupplierId);
                if (existing == null) throw new Exception("Supplier not found");
                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                return _mapper.Map<SupplierDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Supplier: {ex.Message}");
                return null;
            }
        }
    }
}
