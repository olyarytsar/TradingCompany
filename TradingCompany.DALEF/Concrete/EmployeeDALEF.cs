using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography; 
using System.Text;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces; 
using TradingCompany.DALEF.Concrete.ctx;
using EmpDTO = TradingCompany.DTO.Employee;
using EmpModel = TradingCompany.DALEF.Models.Employee;

namespace TradingCompany.DALEF.Concrete
{
 
    public class EmployeeDALEF : GenericDAL<EmpDTO>, IEmployeeDAL
    {
        public EmployeeDALEF(string connStr, IMapper mapper) : base(connStr, mapper) { }

       
        private string HashPassword(string password, string salt)
        {
            using (var alg = SHA512.Create())
            {
                
                var bytes = alg.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToBase64String(bytes);
            }
        }

        
        public override EmpDTO Create(EmpDTO entity)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
               
                if (ctx.Employees.Any(e => e.Login == entity.Login))
                {
                    throw new Exception("Користувач з таким логіном вже існує.");
                }

                
                Guid salt = Guid.NewGuid();
                string hashedPassword = HashPassword(entity.Password, salt.ToString());

                var model = _mapper.Map<EmpModel>(entity);

                
                model.Password = hashedPassword;
                model.Salt = salt; 

                ctx.Employees.Add(model);
                ctx.SaveChanges();

               
                entity.EmployeeId = model.EmployeeId;
                entity.Password = hashedPassword;
                entity.Salt = salt;

                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Employee: {ex.Message}");
                return null;
            }
        }


        public bool Login(string login, string password)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
               
                var user = ctx.Employees.SingleOrDefault(u => u.Login == login);

                if (user == null) return false;

                
                string inputHash = HashPassword(password, user.Salt.ToString());

               
                return user.Password == inputHash;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Login: {ex.Message}");
                return false;
            }
        }

        public EmpDTO GetByLogin(string login)
        {
            using var ctx = new TradingCompContext(_connStr);
            try
            {
                var model = ctx.Employees
                               .Include(e => e.Role)
                               .SingleOrDefault(u => u.Login == login);

                return _mapper.Map<EmpDTO>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Employee by Login: {ex.Message}");
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