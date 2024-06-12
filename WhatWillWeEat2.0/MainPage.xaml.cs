using StartUp;
using WhatWillWeEat2._0.ViewModel;

namespace WhatWillWeEat2._0
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((MainPageViewModel)BindingContext).LoadRecipes();
            ((MainPageViewModel)BindingContext).RandomizeRecipeCommand.Execute(this);
        }
    }

}
