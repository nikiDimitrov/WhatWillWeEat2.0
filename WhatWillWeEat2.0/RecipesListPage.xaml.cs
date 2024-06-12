using StartUp.Model;
using WhatWillWeEat2._0.ViewModel;

namespace WhatWillWeEat2._0;

public partial class RecipesListPage : ContentPage
{
	public RecipesListPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((RecipesListPageViewModel)BindingContext).LoadRecipes();
    }
}