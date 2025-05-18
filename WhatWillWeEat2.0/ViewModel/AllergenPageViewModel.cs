using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WhatWillWeEat2._0.ViewModel
{
    public class AllergenPageViewModel : ViewModelBase
    {
        private DatabaseContext _dbContext;
        private Allergen currentAllergen;
        private Allergen oldAllergen;
        private ObservableCollection<Recipe> allergicRecipes;
        private ICommand deleteCommand;

        public AllergenPageViewModel(Allergen allergen)
        {
            currentAllergen = allergen;
            oldAllergen = allergen.Clone();
        }

        public DatabaseContext DbContext
        {
            get
            {
                if(_dbContext == null)
                {
                    _dbContext = new DatabaseContext();
                }
                return _dbContext;
            }
        }
        public Allergen CurrentAllergen
        {
            get
            {
                return currentAllergen;
            }
            set
            {
                currentAllergen = value;
                NotifyPropertyChanged(nameof(CurrentAllergen));
            }
        }

        public Allergen OldAllergen
        {
            get
            {
                return oldAllergen;
            }
        }
        
        public ObservableCollection<Recipe> AllergicRecipes
        {
            get
            {
                if(allergicRecipes == null)
                {
                    LoadAllergicRecipes();
                    NotifyPropertyChanged(nameof(AllergicRecipes));
                }
                return allergicRecipes;
            }
            set
            {
                allergicRecipes = value;
                NotifyPropertyChanged(nameof(AllergicRecipes));
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if(deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(DeleteAllergen);
                }
                return deleteCommand;
            }
        }

        internal async void LoadAllergicRecipes()
        {
            List<Recipe> allergicRecipesList = new List<Recipe>();

            IngredientAllergen[] ingredientAllergens = await DbContext.IngredientAllergens
                .Include(ia => ia.Ingredient)
                .Where(ia => ia.AllergenId == CurrentAllergen.ID)
                .ToArrayAsync();

            foreach(IngredientAllergen ingredientAllergen in ingredientAllergens)
            {
                Ingredient ingredient = ingredientAllergen.Ingredient;
                RecipeIngredient[] recipeIngredients = await DbContext.RecipeIngredients
                    .Include(ri => ri.Recipe)
                    .Where(ri => ri.IngredientId == ingredient.ID)
                    .ToArrayAsync();

                allergicRecipesList.AddRange(recipeIngredients
                    .Select(ri => ri.Recipe));
            }

            allergicRecipes = new ObservableCollection<Recipe>(allergicRecipesList);
        }

        private async void DeleteAllergen()
        {
            List<IngredientAllergen> ingredientAllergens = await DbContext.IngredientAllergens
                .Include(ia => ia.Ingredient)
                .Where(ia => ia.AllergenId == OldAllergen.ID)
                .ToListAsync();

            DbContext.IngredientAllergens.RemoveRange(ingredientAllergens);

            DbContext.Allergens.Remove(OldAllergen);

            await DbContext.SaveChangesAsync();

            await AppShell.Current.DisplayAlert("Deleted!", "Allergen deleted successfully!", "OK");

            await AppShell.Current.Navigation.PopAsync();
        }
    }
}
