using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using StartUp;
using StartUp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WhatWillWeEat2._0.Services;

namespace WhatWillWeEat2._0.ViewModel
{
    public class AddRecipePageViewModel : ViewModelBase
    {
        private string name;
        private string description;
        private string ingredients;

        private DatabaseContext _dbContext;
        private RelayCommand saveCommand;
        private ICommand returnEntryCommand;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                NotifyPropertyChanged(nameof(Description));
            }
        }

        public string Ingredients
        {
            get
            {
                return ingredients;
            }
            set
            {
                ingredients = value;
                NotifyPropertyChanged(nameof(Ingredients));
            }
        }

        public DatabaseContext DbContext
        {
            get
            {
                if(_dbContext == null)
                {
                    _dbContext = new DatabaseContext();
                    _dbContext.Database.EnsureCreatedAsync();
                }
                return _dbContext;
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                if(saveCommand == null)
                {
                    saveCommand = new RelayCommand(SaveRecipe, () => CanBeSaved);
                }
                return saveCommand;
            }
        }

        public ICommand ReturnEntryCommand
        {
            get
            {
                if(returnEntryCommand == null)
                {
                    returnEntryCommand = new RelayCommand(RefreshSave);
                }
                return returnEntryCommand;
            }
        }

        public bool CanBeSaved
        {
            get
            {
                if(!String.IsNullOrWhiteSpace(Name) &&
                    !String.IsNullOrWhiteSpace(Description) &&
                    !String.IsNullOrWhiteSpace(Ingredients))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private async void SaveRecipe()
        {
            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
            List<Ingredient> ingredients = new List<Ingredient>();

            string[] splittedIngredients = Ingredients
                .Trim()
                .Split(", ")
                .ToArray();

            Recipe recipe = new Recipe()
            {
                Name = Name.Trim(),
                Description = Description.Trim()
            };


            foreach (string ingredientString in splittedIngredients)
            {
                Ingredient ingredient;

                try
                {
                    ingredient = IngredientUtils.ConvertStringToIngredient(ingredientString);
                }
                catch(InvalidIngredientException)
                {
                    await AppShell.Current.DisplayAlert("Incorrect format", "Specified ingredients are in incorrect format!", "OK");
                    return;
                }

                RecipeIngredient recipeIngredient = new RecipeIngredient()
                { 
                    Recipe = recipe,
                    Ingredient = ingredient
                };

                ingredients.Add(ingredient);
                recipeIngredients.Add(recipeIngredient);

                List<Allergen> allergens = await DbContext.Allergens
                    .Where(a => a.Name.ToLower() == ingredient.Name.ToLower())
                    .ToListAsync();

                foreach(Allergen allergen in allergens)
                {
                    await DbContext.IngredientAllergens.AddAsync(new IngredientAllergen
                    {
                        Allergen = allergen,
                        Ingredient = ingredient,
                        IngredientId = ingredient.ID,
                        AllergenId = allergen.ID
                    });
                }
            }

            recipe.RecipeIngredients = recipeIngredients;
            await DbContext.Recipes.AddAsync(recipe);

            for(int i = 0; i < ingredients.Count; i++)
            {
                Ingredient ingredient = ingredients[i];
                ingredient.RecipeIngredients = ingredient.RecipeIngredients
                    .Where(ri => ri.Ingredient == ingredient)
                    .ToList();

                await DbContext.Ingredients.AddAsync(ingredient);
            }

            await DbContext.RecipeIngredients.AddRangeAsync(recipeIngredients);

            await DbContext.SaveChangesAsync();

            await AppShell.Current.DisplayAlert("Success!", "The recipe and its ingredients were added successfully!", "OK");

            await AppShell.Current.Navigation.PopAsync();
        }

        private void RefreshSave()
        {
            SaveCommand.NotifyCanExecuteChanged();
        }
    }
}
