using App.Repositories.Companies;
using App.Repositories.Branches;

namespace App.Repositories.Warehouses
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public int BranchId { get; set; }
        public Branch Branch { get; set; } = default!;
    }
}
