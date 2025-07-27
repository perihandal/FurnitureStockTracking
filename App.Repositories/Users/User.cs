using App.Repositories.Warehauses;
using App.Repositories.StockTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Repositories.Warehouses;
using App.Repositories.PriceDefinitions;

namespace App.Repositories.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = "Operator";

        public ICollection<Warehouse> ProductionLogs { get; set; } = new List<Warehouse>();
        public ICollection<PriceDefinition> StockTransactions { get; set; } = new List<PriceDefinition>();
    }
}
