using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.DAL.Concrete;
using TradingCompany.DALEF.Concrete.ctx;
using EmpDTO = TradingCompany.DTO.Employee;
using EmpModel = TradingCompany.DALEF.Models.Employee;

namespace TradingCompany.DALEF.Concrete
{
    public class EmployeeDALEF : GenericDAL<EmpDTO>
    {
        public EmployeeDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override EmpDTO Create(EmpDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = _mapper.Map<EmpModel>(entity);
                ctx.Employees.Add(model);
                ctx.SaveChanges();
                entity.EmployeeId = model.EmployeeId;
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Employee: {ex.Message}");
                return null;
            }
        }

        public override bool Delete(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Employees.Find(id);
                if (model == null) return false;
                ctx.Employees.Remove(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Employee: {ex.Message}");
                return false;
            }
        }

        public override List<EmpDTO> GetAll()
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var models = ctx.Employees
                                .Include(e => e.Role) 
                                .OrderBy(e => e.EmployeeId)
                                .ToList();
                return _mapper.Map<List<EmpDTO>>(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Employees: {ex.Message}");
                return new List<EmpDTO>();
            }
        }

        public override EmpDTO GetById(int id)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Employees
                               .Include(e => e.Role) 
                               .FirstOrDefault(e => e.EmployeeId == id);
                return _mapper.Map<EmpDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Employee by Id: {ex.Message}");
                return null;
            }
        }


        public override EmpDTO Update(EmpDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var existing = ctx.Employees.Find(entity.EmployeeId);
                if (existing == null) throw new Exception("Employee not found");
                _mapper.Map(entity, existing);
                ctx.SaveChanges();
                return _mapper.Map<EmpDTO>(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Employee: {ex.Message}");
                return null;
            }
        }
    }
}
