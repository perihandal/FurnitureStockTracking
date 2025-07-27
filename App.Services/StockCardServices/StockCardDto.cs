using App.Repositories.StockCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.StockCardServices
{

    public record class StockCardDto()
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public StockCardType Type { get; set; }
        public string Unit { get; set; } = default!;
        public decimal Tax { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = default!;

        public int BranchId { get; set; }
        public string BranchName { get; set; } = default!;

        public int MainGroupId { get; set; }
        public string MainGroupName { get; set; } = default!;

        public int? SubGroupId { get; set; }
        public string? SubGroupName { get; set; }

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }


}
