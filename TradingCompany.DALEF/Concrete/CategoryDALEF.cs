using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using CategoryDTO = TradingCompany.DTO.Category;
using CategoryModel = TradingCompany.DALEF.Models.Category;

namespace TradingCompany.DALEF.Concrete
{
    public class CategoryDALEF : GenericDAL<CategoryDTO>, ICategoryDAL
    {
        public CategoryDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override CategoryDTO Create(CategoryDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = _mapper.Map<CategoryModel>(entity);
                ctx.Categories.Add(model);
                ctx.SaveChanges();
                entity.CategoryId = model.CategoryId;
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Category: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Categories.Find(id);
                if (model == null) return false;
                ctx.Categories.Remove(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Category: {ex.Message}");
                return false;
            }
        }

        public override List<CategoryDTO> GetAll()
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var models = ctx.Categories.OrderBy(c => c.CategoryId).ToList();
                return _mapper.Map<List<CategoryDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Categories: {ex.Message}");
                return new List<CategoryDTO>();
            }
        }

        public override CategoryDTO GetById(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Categories.Find(id);
                return _mapper.Map<CategoryDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Category by Id: {ex.Message}");
                return null;
            }
        }

        public override CategoryDTO Update(CategoryDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var existing = ctx.Categories.Find(entity.CategoryId);
                if (existing == null) throw new Exception("Category not found");
                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                return _mapper.Map<CategoryDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Category: {ex.Message}");
                return null;
            }
        }
    }
}

