using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.RecipeItems
{
    public class RecipeItemRepository(AppDbContext context) : GenericRepository<RecipeItem>(context),IRecipeItemRepository
    {
    }
}
