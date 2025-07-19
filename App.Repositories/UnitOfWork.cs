using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public Task<int> SaveChangesAsycn() => context.SaveChangesAsync(); // await koymama gerek yok zaten arka planda o şekilde çalışcak
    }
}
