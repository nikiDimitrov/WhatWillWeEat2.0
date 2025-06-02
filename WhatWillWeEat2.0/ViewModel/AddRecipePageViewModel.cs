using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private ICommand addIngredientCommand;

        public ObservableCollection<RecipeIngredient> EditableIngredients { get; set; } = new();

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
                RefreshSave();
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
                RefreshSave();
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
                    saveCommand = new RelayCommand(SaveRecipe);
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


        public ICommand AddIngredientCommand => addIngredientCommand ??= new RelayCommand(AddIngredient);

        private void AddIngredient()
        {
            var ingredient = new Ingredient();
            var recipeIngredient = new RecipeIngredient { Ingredient = ingredient };

            if (ingredient is INotifyPropertyChanged npc)
                npc.PropertyChanged += OnIngredientChanged;

            EditableIngredients.Add(recipeIngredient);
            RefreshSave();
        }



        private async void SaveRecipe()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description))
            {
                await AppShell.Current.DisplayAlert("Missing Info", "Name and description are required.", "OK");
                return;
            }

            Recipe recipe = new()
            {
                Name = Name.Trim(),
                Description = Description.Trim(),
                RecipeIngredients = EditableIngredients
            };

            foreach (var recipeIngredient in EditableIngredients)
            {
                var ingredient = recipeIngredient.Ingredient;
                recipeIngredient.Recipe = recipe;

                List<Allergen> allergens = await DbContext.Allergens
                    .Where(a => a.Name.ToLower() == ingredient.Name.ToLower())
                    .ToListAsync();

                foreach (var allergen in allergens)
                {
                    await DbContext.IngredientAllergens.AddAsync(new IngredientAllergen
                    {
                        Allergen = allergen,
                        Ingredient = ingredient,
                        IngredientId = ingredient.ID,
                        AllergenId = allergen.ID
                    });
                }

                await DbContext.Ingredients.AddAsync(ingredient);
            }

            await DbContext.Recipes.AddAsync(recipe);
            await DbContext.RecipeIngredients.AddRangeAsync(EditableIngredients);
            await DbContext.SaveChangesAsync();

            await AppShell.Current.DisplayAlert("Success!", "Recipe added successfully!", "OK");
            await AppShell.Current.Navigation.PopAsync();
        }

        private void RefreshSave()
        {
            SaveCommand.NotifyCanExecuteChanged();
        }

        private void OnIngredientChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshSave();
        }

    }
}
