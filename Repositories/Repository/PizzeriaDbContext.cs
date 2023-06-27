using System;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Core.Models.EntityModels;

namespace Repositories.Repository
{
    public class PizzeriaDbContext : DbContext 
    {
        private static bool bTestDataAdded = false;

        public PizzeriaDbContext()
        {
        }

        public DbSet<OutletInfo> Outlets { get; set; }
        public DbSet<PizzaInfo> Pizzas { get; set; }
        public DbSet<ToppingInfo> Toppings { get; set; }
        public DbSet<OutletPizza> OutletPizzas { get; set; }

        public PizzeriaDbContext(DbContextOptions<PizzeriaDbContext> options)
        : base(options)
        {
            if (!bTestDataAdded)
            {
                AddTestData();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutletInfo>()
                .HasKey(e => new { e.ID });

            modelBuilder.Entity<PizzaInfo>()
                .HasKey(e => new { e.ID });

            modelBuilder.Entity<ToppingInfo>()
                .HasKey(e => new { e.ID });

            modelBuilder.Entity<OutletPizza>()
                .HasKey(e => new { e.OutletID, e.PizzaID });

            modelBuilder.Entity<OutletPizza>()
                .HasOne(sp => sp.Outlet)
                .WithMany(s => s.OutletPizzas)
                .HasForeignKey(sp => sp.OutletID)
                .OnDelete(DeleteBehavior.Cascade); // Set cascading delete behavior when Outlet is deleted

            modelBuilder.Entity<OutletPizza>()
                .HasOne(sp => sp.Pizza)
                .WithMany(p => p.OutletPizzas)
                .HasForeignKey(sp => sp.PizzaID)
                .OnDelete(DeleteBehavior.Cascade); // Set cascading delete behavior when PizzaInfo is deleted
        }

        private void AddTestData()
        {
            try
            {
                Outlets.AddRange(
                    new OutletInfo() { Name = "Preston Pizzeria" },
                    new OutletInfo() { Name = "Southbank Pizzeria" }
                    );

                Pizzas.AddRange(
                    new PizzaInfo() { Name = "Capricciosa", Ingredients = "Cheese, Ham, Mushrooms, Olives" },
                    new PizzaInfo() { Name = "Mexicana", Ingredients = "Cheese, Salami, Capsicum, Chilli" },
                    new PizzaInfo() { Name = "Margherita", Ingredients = "Cheese, Spinach, Ricotta, Cherry Tomatoes" },
                    new PizzaInfo() { Name = "Vegetarian", Ingredients = "Cheese, Mushrooms, Capsicum, Onion, Olives" }
                    );

                Toppings.AddRange(
                    new ToppingInfo() { Name = "Cheese" },
                    new ToppingInfo() { Name = "Capsicum" },
                    new ToppingInfo() { Name = "Salami" },
                    new ToppingInfo() { Name = "Olives" }
                    );

                OutletPizzas.AddRange(
                    new OutletPizza() { OutletID = 1, PizzaID = 1, Price = 20 },
                    new OutletPizza() { OutletID = 1, PizzaID = 2, Price = 18 },
                    new OutletPizza() { OutletID = 1, PizzaID = 3, Price = 22 },
                    new OutletPizza() { OutletID = 2, PizzaID = 1, Price = 25 },
                    new OutletPizza() { OutletID = 2, PizzaID = 4, Price = 17 }
                    );

                SaveChanges();
                bTestDataAdded = true;

            }
            catch (Exception e)
            {
                throw new DbUpdateException("Error when seeding data.", e);
            }
        }
    }
}
