using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WhatWillWeEat2._0.ViewModel
{
    public class RecipesListPageViewModel : ViewModelBase
    {
        private ObservableCollection<Recipe> recipes;
        private DatabaseContext _dbContext;
        private Recipe selectedRecipe;
        private ICommand addRecipeCommand;

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

        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                if(recipes == null)
                {
                    LoadRecipes();
                }
                
                return recipes;
            }
            set
            {
                if(recipes != value)
                {
                    recipes = value;
                    NotifyPropertyChanged(nameof(Recipes));
                }
            }
        }

        public Recipe SelectedRecipe
        {
            get
            {
                return selectedRecipe;
            }
            set
            {
                selectedRecipe = value;
                AppShell.Current.Navigation.PopAsync();
                AppShell.Current.Navigation.PushAsync(new RecipePage(selectedRecipe));
                NotifyPropertyChanged(nameof(SelectedRecipe));

                selectedRecipe = null;
                NotifyPropertyChanged(nameof(SelectedRecipe));
            }
        }

        public ICommand AddRecipeCommand
        {
            get
            {
                if(addRecipeCommand == null)
                {
                    addRecipeCommand = new RelayCommand(AddRecipe);
                }
                return addRecipeCommand;
            }
        }

        private async void AddRecipe()
        {
            await AppShell.Current.Navigation.PopAsync();
            await AppShell.Current.Navigation.PushAsync(new AddRecipePage());
        }

        internal async void LoadRecipes()
        {
            List<Recipe> recipesList = await DbContext.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();

            recipes = new ObservableCollection<Recipe>(recipesList);
            NotifyPropertyChanged(nameof(Recipes));
        }
    }
}
