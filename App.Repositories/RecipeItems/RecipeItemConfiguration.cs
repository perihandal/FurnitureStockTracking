using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.RecipeItems
{
    public class RecipeItemConfiguration : IEntityTypeConfiguration<RecipeItem>
    {
        public void Configure(EntityTypeBuilder<RecipeItem> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Product)
                   .WithMany(p => p.RecipeItems)
                   .HasForeignKey(r => r.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.RequiredProduct)
                   .WithMany(p => p.UsedInRecipes)
                   .HasForeignKey(r => r.RequiredProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.Quantity).IsRequired();
        }
    }

}
