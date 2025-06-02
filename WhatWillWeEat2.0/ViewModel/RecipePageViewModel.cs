using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private ICommand addIngredientCommand;
        private ICommand removeIngredientCommand;
        public RecipePageViewModel(Recipe recipe)
        {
            CurrentRecipe = recipe;
            OldRecipe = recipe.Clone();

            foreach (var ri in CurrentRecipe.RecipeIngredients)
            {
                if (ri.Ingredient is INotifyPropertyChanged npc)
                {
                    npc.PropertyChanged += OnIngredientChanged;
                }
            }
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

        public ObservableCollection<RecipeIngredient> EditableIngredients
        {
            get => CurrentRecipe.RecipeIngredients;
            set
            {
                // Unsubscribe from previous
                foreach (var ri in CurrentRecipe.RecipeIngredients)
                {
                    if (ri.Ingredient is INotifyPropertyChanged npcOld)
                        npcOld.PropertyChanged -= OnIngredientChanged;
                }

                CurrentRecipe.RecipeIngredients = value;

                // Subscribe to new
                foreach (var ri in value)
                {
                    if (ri.Ingredient is INotifyPropertyChanged npcNew)
                        npcNew.PropertyChanged += OnIngredientChanged;
                }

                NotifyPropertyChanged(nameof(EditableIngredients));
                RefreshSave();
            }
        }


        public ICommand AddIngredientCommand
        {
            get
            {
                if(addIngredientCommand == null)
                {
                    addIngredientCommand = new RelayCommand(AddIngredient);
                }
                return addIngredientCommand;
            }
        }

        public ICommand RemoveIngredientCommand
        {
            get
            {
                if (removeIngredientCommand == null)
                {
                    removeIngredientCommand = new RelayCommand<RecipeIngredient>(RemoveIngredient);
                }
                return removeIngredientCommand;
            }
        }

        private void AddIngredient()
        {
            var newIngredient = new Ingredient();
            var newRecipeIngredient = new RecipeIngredient
            {
                Ingredient = newIngredient
            };

            EditableIngredients.Add(newRecipeIngredient);

            if (newIngredient is INotifyPropertyChanged npc)
                npc.PropertyChanged += OnIngredientChanged;

            RefreshSave();
        }


        private void RemoveIngredient(RecipeIngredient recipeIngredient)
        {
            EditableIngredients.Remove(recipeIngredient);

            Ingredient ingredientActual = DbContext.Ingredients.FirstOrDefault(i => i.Name == recipeIngredient.Ingredient.Name);

            DbContext.RecipeIngredients.Remove(recipeIngredient);
            DbContext.Ingredients.Remove(ingredientActual);
            DbContext.SaveChanges();
            RefreshSave();
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
                    saveCommand = new RelayCommand(SaveRecipe);
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

        private async void SaveRecipe()
        {
            DbContext.Recipes.Update(currentRecipe);
            await DbContext.SaveChangesAsync();

            var updatedRecipe = await DbContext.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.ID == currentRecipe.ID);

            if (updatedRecipe != null)
            {
                CurrentRecipe = updatedRecipe;
                EditableIngredients = new ObservableCollection<RecipeIngredient>(updatedRecipe.RecipeIngredients);
            }

            await AppShell.Current.DisplayAlert("Edited!", "Your recipe is edited successfully!", "OK");
            await AppShell.Current.Navigation.PopAsync();
        }

        private async void DeleteRecipe()
        {
            bool result = await AppShell.Current.DisplayAlert("Are you sure?", "Are you sure you want to delete this recipe?", "Yes", "No");
            if(result)
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

        private void OnIngredientChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshSave();
        }


        public void RefreshSave()
        {
            SaveCommand.NotifyCanExecuteChanged();
        }
    }
}
