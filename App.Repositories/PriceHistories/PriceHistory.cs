using App.Repositories.PriceDefinitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.PriceHistories
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public PriceType PriceType { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;

        public bool IsActive = true;

        public int PriceDefinitionId { get; set; }
        public PriceDefinition PriceDefinition { get; set; }
    }
}
