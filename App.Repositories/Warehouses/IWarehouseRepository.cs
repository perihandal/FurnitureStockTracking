using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Warehouses
{
    public interface IWarehouseRepository : IGenericRepository<Warehouse>
    {
        Task<List<Warehouse>> GetAllWithDetailsAsync();
        Task<Warehouse?> GetByCodeAsync(string code);
        Task<Warehouse?> GetByNameAsync(string name);


    }
}
