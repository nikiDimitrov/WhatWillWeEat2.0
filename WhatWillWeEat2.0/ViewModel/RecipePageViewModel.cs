using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System.Windows.Input;

namespace WhatWillWeEat2._0.ViewModel
{
    public class RecipePageViewModel : ViewModelBase
    {
        private Recipe currentRecipe;
        private Recipe oldRecipe;
        private DatabaseContext _dbContext;
        private RelayCommand saveCommand;
        private RelayCommand deleteCommand;
        private ICommand returnEntryCommand;

        public RecipePageViewModel(Recipe recipe)
        {
            CurrentRecipe = recipe;
            OldRecipe = recipe.Clone();
        }

        public Recipe CurrentRecipe
        {
            get 
            { 
                return currentRecipe; 
            }
            set
            {
                currentRecipe = value;
                NotifyPropertyChanged(nameof(CurrentRecipe));
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

        public Recipe OldRecipe
        {
            get
            {
                return oldRecipe;
            }
            private set
            {
                oldRecipe = value;
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                if(saveCommand == null)
                {
                    saveCommand = new RelayCommand(SaveRecipe, () => IsRecipeEdited);
                }
                return saveCommand;
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                if(deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(DeleteRecipe);
                }
                return deleteCommand;
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

        public string IngredientsDisplay =>
            string.Join(", ", CurrentRecipe.RecipeIngredients
                .Select(ri => ri.Ingredient?.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s)));

        private async void SaveRecipe()
        {
            DbContext.Recipes.Update(currentRecipe);
            await DbContext.SaveChangesAsync();

            await AppShell.Current.DisplayAlert("Edited!", "Your recipe is edited successfully!", "OK");
            await AppShell.Current.Navigation.PopAsync();
        }

        private async void DeleteRecipe()
        {
            bool result = await AppShell.Current.DisplayAlert("Are you sure?", "Are you sure you want to delete this recipe?", "Yes", "No");
            if(result == true)
            {
                List<RecipeIngredient> recipeIngredients = await DbContext.RecipeIngredients
                    .Include(ri => ri.Ingredient)
                    .Where(ri => ri.RecipeId == OldRecipe.ID)
                    .ToListAsync();

                DbContext.RecipeIngredients.RemoveRange(recipeIngredients);

                foreach(RecipeIngredient recipeIngredient in recipeIngredients)
                {
                    Ingredient ingredient = recipeIngredient.Ingredient;
                    List<IngredientAllergen> allergenIngredients = await DbContext.IngredientAllergens
                        .Where(ai => ai.IngredientId == ingredient.ID)
                        .ToListAsync();

                    if(allergenIngredients.Count != 0)
                    {
                        DbContext.IngredientAllergens.RemoveRange(allergenIngredients);
                    }

                    DbContext.Ingredients.Remove(ingredient);
                }

                DbContext.Recipes.Remove(OldRecipe);

                try
                {
                    await DbContext.SaveChangesAsync();
                    await AppShell.Current.DisplayAlert("Deleted!", "Recipe successfully deleted!", "OK");
                    await AppShell.Current.Navigation.PopAsync();
                }
                catch
                {
                    await AppShell.Current.DisplayAlert("Not deleted", "Deleting recipe was not successful.", "OK");
                }
            }
        }

        private bool IsRecipeEdited
        {
            get
            {
                if(CurrentRecipe.Equals(OldRecipe))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void RefreshSave()
        {
            SaveCommand.NotifyCanExecuteChanged();
        }
    }
}
