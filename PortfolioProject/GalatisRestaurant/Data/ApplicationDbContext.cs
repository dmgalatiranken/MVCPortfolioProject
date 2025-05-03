using GalatisRestaurant.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GalatisRestaurant.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DB Tables
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<ProductIngredient> ProductIngredients { get; set; } // Join table
    public DbSet<Review> Reviews { get; set; }

    // Fluent API Configuration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Composite Keys and Relationships for ProductIngredient
        modelBuilder.Entity<ProductIngredient>()
            .HasKey(pi => new { pi.ProductId, pi.IngredientId });

        modelBuilder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductIngredients)
            .HasForeignKey(pi => pi.ProductId);

        modelBuilder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Ingredient)
            .WithMany(i => i.ProductIngredients)
            .HasForeignKey(pi => pi.IngredientId);

        // Seed Data
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Appetizer" },
            new Category { CategoryId = 2, Name = "Entree" },
            new Category { CategoryId = 3, Name = "Side Dish" },
            new Category { CategoryId = 4, Name = "Dessert" },
            new Category { CategoryId = 5, Name = "Beverage" }
            );

        modelBuilder.Entity<Ingredient>().HasData(

            // Pizza Ingredients for Italian Pizza Restaurant

            new Ingredient { IngredientId = 1, Name = "Pizza Dough" },
            new Ingredient { IngredientId = 2, Name = "Pizza Sauce" },
            new Ingredient { IngredientId = 3, Name = "Provel Cheese" },
            new Ingredient { IngredientId = 4, Name = "Sausage" },
            new Ingredient { IngredientId = 5, Name = "Hamburger" },
            new Ingredient { IngredientId = 6, Name = "Pepperoni" },
            new Ingredient { IngredientId = 7, Name = "Bacon" },
            new Ingredient { IngredientId = 8, Name = "Mushrooms" },
            new Ingredient { IngredientId = 9, Name = "Onions" }
            );

        modelBuilder.Entity<Product>().HasData(

            // Restaurant Products

            new Product
            {
                ProductId = 1,
                Name = "Cheese Pizza",
                Description = "A classic cheese pizza with our homemade sauce and provel cheese.",
                Price = 12.99m,
                Stock = 98,
                CategoryId = 2
            },
            new Product
            {
                ProductId = 2,
                Name = "Sausage Pizza",
                Description = "A classic sausage pizza with our homemade sauce and provel cheese.",
                Price = 13.99m,
                Stock = 87,
                CategoryId = 2
            },
            new Product
            {
                ProductId = 3,
                Name = "Pepperoni Pizza",
                Description = "A classic pepperoni pizza with our homemade sauce and provel cheese.",
                Price = 14.99m,
                Stock = 91,
                CategoryId = 2
            }
            );

        // Wire up products to their ingredients (each product has many ingredients)
        modelBuilder.Entity<ProductIngredient>().HasData(

            // Cheese Pizza Product Ingredients
            new ProductIngredient { ProductId = 1, IngredientId = 1 },
            new ProductIngredient { ProductId = 1, IngredientId = 2 },
            new ProductIngredient { ProductId = 1, IngredientId = 3 },

            // Sausage Pizza Product Ingredients
            new ProductIngredient { ProductId = 2, IngredientId = 1 },
            new ProductIngredient { ProductId = 2, IngredientId = 2 },
            new ProductIngredient { ProductId = 2, IngredientId = 3 },
            new ProductIngredient { ProductId = 2, IngredientId = 4 },

            // Pepperoni Pizza Product Ingredients
            new ProductIngredient { ProductId = 3, IngredientId = 1 },
            new ProductIngredient { ProductId = 3, IngredientId = 2 },
            new ProductIngredient { ProductId = 3, IngredientId = 3 },
            new ProductIngredient { ProductId = 3, IngredientId = 6 }

            );
    }
}
