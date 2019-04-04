using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRestaurant.Api.Modules.Common.Entities {
    public class AutoRestaurantContext : DbContext {
        public AutoRestaurantContext(DbContextOptions<AutoRestaurantContext> options) : base(options) { }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<MenuItem>()
                .HasOne(mi => mi.Menu)
                .WithMany(menu => menu.MenuItems)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
