﻿using App.Repositories.StockCards;

namespace App.Repositories.Categories
{
    public class Category
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public ICollection<StockCard> StockCards { get; set; } = new List<StockCard>();
    }
}
