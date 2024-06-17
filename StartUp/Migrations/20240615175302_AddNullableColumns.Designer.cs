﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StartUp;

#nullable disable

namespace StartUp.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240615175302_AddNullableColumns")]
    partial class AddNullableColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("StartUp.Model.Allergen", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Allergens");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "боб"
                        },
                        new
                        {
                            ID = 2,
                            Name = "леща"
                        });
                });

            modelBuilder.Entity("StartUp.Model.Ingredient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<string>("Unit")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Ingredients");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "боб",
                            Quantity = 3.0,
                            Unit = "кг"
                        },
                        new
                        {
                            ID = 2,
                            Name = "леща",
                            Quantity = 0.0,
                            Unit = ""
                        });
                });

            modelBuilder.Entity("StartUp.Model.IngredientAllergen", b =>
                {
                    b.Property<int>("IngredientId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AllergenId")
                        .HasColumnType("INTEGER");

                    b.HasKey("IngredientId", "AllergenId");

                    b.HasIndex("AllergenId");

                    b.ToTable("IngredientAllergens");

                    b.HasData(
                        new
                        {
                            IngredientId = 1,
                            AllergenId = 1
                        },
                        new
                        {
                            IngredientId = 2,
                            AllergenId = 2
                        });
                });

            modelBuilder.Entity("StartUp.Model.Recipe", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Recipes");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Description = "Свари картофи",
                            Name = "Мусака"
                        },
                        new
                        {
                            ID = 2,
                            Description = "Свари мусака",
                            Name = "Картофи"
                        },
                        new
                        {
                            ID = 3,
                            Description = "Свари боб",
                            Name = "Боб"
                        },
                        new
                        {
                            ID = 4,
                            Description = "Свари леща",
                            Name = "Леща"
                        });
                });

            modelBuilder.Entity("StartUp.Model.RecipeIngredient", b =>
                {
                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IngredientId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RecipeId", "IngredientId");

                    b.HasIndex("IngredientId");

                    b.ToTable("RecipeIngredients");

                    b.HasData(
                        new
                        {
                            RecipeId = 3,
                            IngredientId = 1
                        },
                        new
                        {
                            RecipeId = 4,
                            IngredientId = 2
                        });
                });

            modelBuilder.Entity("StartUp.Model.IngredientAllergen", b =>
                {
                    b.HasOne("StartUp.Model.Allergen", "Allergen")
                        .WithMany("IngredientAllergens")
                        .HasForeignKey("AllergenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StartUp.Model.Ingredient", "Ingredient")
                        .WithMany("IngredientAllergens")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Allergen");

                    b.Navigation("Ingredient");
                });

            modelBuilder.Entity("StartUp.Model.RecipeIngredient", b =>
                {
                    b.HasOne("StartUp.Model.Ingredient", "Ingredient")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StartUp.Model.Recipe", "Recipe")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("StartUp.Model.Allergen", b =>
                {
                    b.Navigation("IngredientAllergens");
                });

            modelBuilder.Entity("StartUp.Model.Ingredient", b =>
                {
                    b.Navigation("IngredientAllergens");

                    b.Navigation("RecipeIngredients");
                });

            modelBuilder.Entity("StartUp.Model.Recipe", b =>
                {
                    b.Navigation("RecipeIngredients");
                });
#pragma warning restore 612, 618
        }
    }
}
