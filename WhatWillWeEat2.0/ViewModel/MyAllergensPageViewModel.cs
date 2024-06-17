using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WhatWillWeEat2._0.ViewModel
{
    public class MyAllergensPageViewModel : ViewModelBase
    {
        private ObservableCollection<Allergen> allergens;
        private DatabaseContext _dbContext;
        private Allergen selectedAllergen;
        private ICommand addAllergenCommand;

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


        public ObservableCollection<Allergen> Allergens
        {
            get
            {
                if(allergens == null)
                {
                    LoadAllergens();
                }
                return allergens;
            }
            set
            {
                if(allergens != value)
                {
                    allergens = value;
                    NotifyPropertyChanged(nameof(Allergens));
                }
            }
        }

        public Allergen SelectedAllergen
        {
            get
            {
                return selectedAllergen;
            }
            set
            {
                selectedAllergen = value;
                AppShell.Current.Navigation.PopAsync();
                AppShell.Current.Navigation.PushAsync(new AllergenPage(selectedAllergen));
                NotifyPropertyChanged(nameof(SelectedAllergen));

                selectedAllergen = null;
                NotifyPropertyChanged(nameof(SelectedAllergen));
            }
        }

        public ICommand AddAllergenCommand
        {
            get
            {
                if(addAllergenCommand == null)
                {
                    addAllergenCommand = new RelayCommand(AddAllergen);
                }
                return addAllergenCommand;
            }
        }

        internal void LoadAllergens()
        {
            List<Allergen> allergensList = DbContext.Allergens
                .Include(i => i.IngredientAllergens)
                .ToList();
            allergens = new ObservableCollection<Allergen>(allergensList);
            NotifyPropertyChanged(nameof(Allergens));
        }

        private async void AddAllergen()
        {
            await AppShell.Current.Navigation.PopAsync();
            //await AppShell.Current.Navigation.PushAsync(new AddAllergenPage());
        }
    }
}
