using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WhatWillWeEat2._0.ViewModel
{
    public class RecipePageViewModel : ViewModelBase
    {
        private Recipe currentRecipe;
        private Recipe oldRecipe;
        private DatabaseContext _dbContext;
        private RelayCommand saveCommand;
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

            await AppShell.Current.DisplayAlert("Edited!", "Your recipe is edited successfully!", "OK");
            await AppShell.Current.Navigation.PopAsync();
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
