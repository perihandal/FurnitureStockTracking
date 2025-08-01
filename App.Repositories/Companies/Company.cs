﻿using App.Repositories.Branches;
using App.Repositories.StockCards;
using App.Repositories.Warehouses;

namespace App.Repositories.Companies
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string TaxNumber { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public ICollection<StockCard> StockCards { get; set; } = new List<StockCard>();
        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    }
}
