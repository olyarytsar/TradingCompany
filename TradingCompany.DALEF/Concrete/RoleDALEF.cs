using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using RoleDTO = TradingCompany.DTO.Role;
using RoleModel = TradingCompany.DALEF.Models.Role;

namespace TradingCompany.DALEF.Concrete
{
    public class RoleDALEF : GenericDAL<RoleDTO>, IRoleDAL
    {
        public RoleDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override RoleDTO Create(RoleDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = _mapper.Map<RoleModel>(entity);
                ctx.Roles.Add(model);
                ctx.SaveChanges();
                entity.RoleId = model.RoleId;
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Role: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Roles.Find(id);
                if (model == null) return false;
                ctx.Roles.Remove(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Role: {ex.Message}");
                return false;
            }
        }

        public override List<RoleDTO> GetAll()
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var models = ctx.Roles.OrderBy(r => r.RoleId).ToList();
                return _mapper.Map<List<RoleDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Roles: {ex.Message}");
                return new List<RoleDTO>();
            }
        }

        public override RoleDTO GetById(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Roles.Find(id);
                return _mapper.Map<RoleDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Role by Id: {ex.Message}");
                return null;
            }
        }

        public override RoleDTO Update(RoleDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var existing = ctx.Roles.Find(entity.RoleId);
                if (existing == null) throw new Exception("Role not found");
                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                return _mapper.Map<RoleDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Role: {ex.Message}");
                return null;
            }
        }
    }
}
