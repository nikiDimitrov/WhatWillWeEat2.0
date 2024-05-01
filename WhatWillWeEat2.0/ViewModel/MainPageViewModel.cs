using StartUp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WhatWillWeEat2._0.Services;

namespace WhatWillWeEat2._0.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private Recipe displayedRecipe;
        private ICommand randomizeRecipeCommand;

        public Recipe DisplayedRecipe
        {
            get
            {
                return displayedRecipe;
            }
            set
            {
                displayedRecipe = value;
                NotifyPropertyChanged("DisplayedRecipe");
            }
        }

        public ICommand RandomizeRecipeCommand
        {
            get
            {
                if(randomizeRecipeCommand == null)
                {
                    randomizeRecipeCommand = new RelayCommand(RandomizeRecipe);
                }
                return randomizeRecipeCommand;
            }
        }

        private void RandomizeRecipe()
        {

        }

    }
}
