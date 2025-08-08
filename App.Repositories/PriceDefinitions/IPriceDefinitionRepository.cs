using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.PriceDefinitions
{
   public interface IPriceDefinitionRepository: IGenericRepository<PriceDefinition>
    {
        Task<List<PriceDefinition>> GetAllWithDetailsAsync();
        Task<PriceDefinition> GetAllWithDetailsAsync(int id);
    }
}
