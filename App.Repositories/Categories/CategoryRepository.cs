using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Categories
{
    internal class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
    }
}
