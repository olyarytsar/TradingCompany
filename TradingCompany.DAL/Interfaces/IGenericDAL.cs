using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DAL.Interfaces
{
    public interface IGenericDAL<T>
    {
        T Create(T entity);
        List<T> GetAll();
        T GetById(int id);
        T Update(T entity);
        bool Delete(int id);
    }
}
