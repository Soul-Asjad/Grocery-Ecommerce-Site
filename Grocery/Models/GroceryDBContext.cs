using Microsoft.EntityFrameworkCore;

namespace Grocery.Models
{
    public class GroceryDBContext : DbContext
    {
        public GroceryDBContext(DbContextOptions<GroceryDBContext> options) : base(options)
        {

        }

        public DbSet<Users> User { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Addresses> Addresss { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Orders → Users
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.users)
                .WithMany()
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            // Orders → Addresses
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.address)
                .WithMany()
                .HasForeignKey(o => o.AddressID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}