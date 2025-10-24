
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<Basket> Baskets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Product)
                .WithMany(p => p.Items)
                .HasForeignKey(bi => bi.ProductId);

            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Basket)
                .WithMany(b => b.Items)
                .HasForeignKey(bi => bi.BasketId);
        }
    }
}
