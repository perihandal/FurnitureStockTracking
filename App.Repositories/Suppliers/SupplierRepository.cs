﻿using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Suppliers
{
    public class SupplierRepository(AppDbContext context) : GenericRepository<Supplier>(context), ISupplierRepository
    {
    }
}
