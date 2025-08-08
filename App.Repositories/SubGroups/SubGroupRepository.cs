using App.Repositories.SubGroups;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.SubGroups
{
    public class SubGroupRepository(AppDbContext context) : GenericRepository<SubGroup>(context),ISubGroupRepository
    {
        public async Task<List<SubGroup>> GetAllWithDetailsAsync()
        {
            return await context.SubGroups
                .Include(sg => sg.MainGroup)       // MainGroup ilişkisini dahil et
                .Include(sg => sg.User)            // User ilişkisini dahil et
                .Include(sg => sg.StockCards)      // StockCards ilişkisini dahil et (eğer gerekiyorsa)
                .ToListAsync();                    // Verileri liste olarak döndür
        }
    }
}
