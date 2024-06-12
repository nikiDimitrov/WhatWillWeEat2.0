
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StartUp;
using StartUp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WhatWillWeEat2._0.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private Recipe displayedRecipe;
        private ICommand randomizeRecipeCommand;
        private ObservableCollection<Recipe> recipes;
        private DatabaseContext _dbContext;

        public Recipe DisplayedRecipe
        {
            get
            {
                if(displayedRecipe == null)
                {
                    RandomizeRecipe();
                }
                return displayedRecipe;
            }
            set
            {
                displayedRecipe = value;
                NotifyPropertyChanged(nameof(DisplayedRecipe));
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
                }
                NotifyPropertyChanged(nameof(Recipes));
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

        public ICommand RandomizeRecipeCommand
        {
            get
            {
                if(randomizeRecipeCommand == null)
                {
                    randomizeRecipeCommand = new RelayCommand(RandomizeRecipe, CanRecipeBeRandomized);
                }
                return randomizeRecipeCommand;
            }
        }

        private void RandomizeRecipe()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int elementIndex = rnd.Next(0, Recipes.Count);

            DisplayedRecipe = Recipes.ToList().ElementAt(elementIndex);
            NotifyPropertyChanged(nameof(DisplayedRecipe));
        }

        private bool CanRecipeBeRandomized()
        {
            if(Recipes.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal void LoadRecipes()
        {
            _dbContext = null;
            List<Recipe> recipesList = DbContext.Recipes.ToList();
            recipes = new ObservableCollection<Recipe>(recipesList);
            NotifyPropertyChanged(nameof(Recipes));
        }

    }
}
