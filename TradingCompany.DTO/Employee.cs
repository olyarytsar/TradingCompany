using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid Salt { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public override string ToString()
        {
            return $"{EmployeeId}: {FirstName} ({Login}) - {Phone}, Role: {Role}";
        }
    }
}

