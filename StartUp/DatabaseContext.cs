using Microsoft.EntityFrameworkCore;
using StartUp.Model;


namespace StartUp
{
    public class DatabaseContext : DbContext
    {
        private string _dbPath;
        public DatabaseContext()
        {
            this._dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.LocalApplicationData), "WhatWillWeEat.db");
        }
        public DatabaseContext(string dbPath)
        {
            this._dbPath = dbPath; 
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            OnDatabaseChanged();
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = base.SaveChangesAsync(cancellationToken);
            OnDatabaseChanged();
            return result;
        }

        public event EventHandler DatabaseChanged;

        protected virtual void OnDatabaseChanged()
        {
            DatabaseChanged?.Invoke(this, EventArgs.Empty);
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<IngredientAllergen> IngredientAllergens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={this._dbPath}");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>().HasKey(r => r.ID);
            modelBuilder.Entity<Ingredient>().HasKey(i => i.ID);
            modelBuilder.Entity<Allergen>().HasKey(a => a.ID);

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<IngredientAllergen>()
                .HasKey(ia => new { ia.IngredientId, ia.AllergenId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);

            modelBuilder.Entity<IngredientAllergen>()
                .HasOne(ia => ia.Ingredient)
                .WithMany(i => i.IngredientAllergens)
                .HasForeignKey(ia => ia.IngredientId);

            modelBuilder.Entity<IngredientAllergen>()
                .HasOne(ia => ia.Allergen)
                .WithMany(a => a.IngredientAllergens)
                .HasForeignKey(ia => ia.AllergenId);

            Recipe recipe1 = new Recipe
            {
                ID = 1,
                Name = "Боб",
                Description = "Свари боб",
            };

            Recipe recipe2 = new Recipe
            {
                ID = 2,
                Name = "Леща",
                Description = "Свари леща",
            };


            Allergen allergen = new Allergen
            {
                ID = 1,
                Name = "боб",
            };
            Allergen allergen2 = new Allergen
            {
                ID = 2,
                Name = "леща",
            };

            Ingredient ingredient = new Ingredient()
            {
                ID = 1,
                Name = "боб",
                Unit = "кг",
                Quantity = 1
            };

            Ingredient ingredient2 = new Ingredient()
            {
                ID = 2,
                Name = "леща",
                Unit = "",
                Quantity = 2
            };

            RecipeIngredient recipeIngredient1 = new RecipeIngredient
            {
                RecipeId = 1,
                IngredientId = 1,
            };

            RecipeIngredient recipeIngredient2 = new RecipeIngredient
            {
                RecipeId = 2,
                IngredientId = 2,
            };

            // IngredientAllergens
            IngredientAllergen ingredientAllergen1 = new IngredientAllergen
            {
                AllergenId = 1,
                IngredientId = 1,
            };

            IngredientAllergen ingredientAllergen2 = new IngredientAllergen
            {
                AllergenId = 2,
                IngredientId = 2,
            };

            modelBuilder.Entity<Recipe>().HasData(recipe1, recipe2);
            modelBuilder.Entity<Ingredient>().HasData(ingredient, ingredient2);
            modelBuilder.Entity<Allergen>().HasData(allergen, allergen2);
            modelBuilder.Entity<RecipeIngredient>().HasData(recipeIngredient1, recipeIngredient2);
            modelBuilder.Entity<IngredientAllergen>().HasData(ingredientAllergen1, ingredientAllergen2);
         }
    }
}
