using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System.Windows.Input;

namespace WhatWillWeEat2._0.ViewModel
{
    public class AddAllergenPageViewModel : ViewModelBase
    {
        private string name;
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

        public DatabaseContext DbContext
        {
            get
            {
                if (_dbContext == null)
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
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(AddAllergen, () => CanBeSaved);
                }
                return saveCommand;
            }
        }

        public ICommand ReturnEntryCommand
        {
            get
            {
                if (returnEntryCommand == null)
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
                if (!String.IsNullOrWhiteSpace(Name))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private async void AddAllergen()
        {
            Allergen allergen = new Allergen()
            {
                Name = Name.Trim()
            };

            await DbContext.Allergens.AddAsync(allergen);
            LinkAllergenWithRecipes(allergen);
            await DbContext.SaveChangesAsync();


            await AppShell.Current.DisplayAlert("Success", $"Allergen '{Name}' added successfully!", "OK");
            await AppShell.Current.Navigation.PopAsync();
        }

        private async void LinkAllergenWithRecipes(Allergen allergen)
        {
            Recipe[] recipes = await DbContext.Recipes.ToArrayAsync();

            RecipeIngredient[] recipeIngredientsAllergic = await DbContext.RecipeIngredients
                .Include(ri => ri.Recipe)
                .Include(ri => ri.Ingredient)
                .Where(ri => ri.Ingredient.Name.ToLower().Equals(Name.ToLower()))
                .ToArrayAsync();

            List<IngredientAllergen> ingredientAllergens = new List<IngredientAllergen>();

            foreach(RecipeIngredient recipeIngredient in recipeIngredientsAllergic)
            {
                Ingredient ingredient = recipeIngredient.Ingredient;
                IngredientAllergen ingredientAllergen = new IngredientAllergen()
                {
                    IngredientId = ingredient.ID,
                    Ingredient = ingredient,
                    AllergenId = allergen.ID,
                    Allergen = allergen
                };
                ingredientAllergens.Add(ingredientAllergen);
            }

            await DbContext.IngredientAllergens.AddRangeAsync(ingredientAllergens);
        }

        private void RefreshSave()
        {
            SaveCommand.NotifyCanExecuteChanged();
        }
    }
}