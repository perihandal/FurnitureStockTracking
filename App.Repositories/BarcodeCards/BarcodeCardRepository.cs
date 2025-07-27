using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.BarcodeCards
{
    public class BarcodeCardRepository(AppDbContext context) : GenericRepository<BarcodeCard>(context), IBarcodeCardRepository
    {
    }
}
