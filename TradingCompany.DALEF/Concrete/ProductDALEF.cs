using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using ProductDTO = TradingCompany.DTO.Product;
using ProductModel = TradingCompany.DALEF.Models.Product;

namespace TradingCompany.DALEF.Concrete
{
    public class ProductDALEF : GenericDAL<ProductDTO>, IProductDAL
    {
        public ProductDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override ProductDTO Create(ProductDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = _mapper.Map<ProductModel>(entity); 
                ctx.Products.Add(model);
                ctx.SaveChanges();
                entity.ProductId = model.ProductId;
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Product: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Products.Find(id);
                if (model == null) return false;
                ctx.Products.Remove(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Product: {ex.Message}");
                return false;
            }
        }

        public override List<ProductDTO> GetAll()
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var models = ctx.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .OrderBy(p => p.ProductId)
                    .ToList();
                return _mapper.Map<List<ProductDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Products: {ex.Message}");
                return new List<ProductDTO>();
            }
        }

        public override ProductDTO GetById(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .FirstOrDefault(p => p.ProductId == id);
                return _mapper.Map<ProductDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Product by Id: {ex.Message}");
                return null;
            }
        }

        public override ProductDTO Update(ProductDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var existing = ctx.Products.Find(entity.ProductId);
                if (existing == null) throw new Exception("Product not found");
                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                return _mapper.Map<ProductDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Product: {ex.Message}");
                return null;
            }
        }
    }
}

