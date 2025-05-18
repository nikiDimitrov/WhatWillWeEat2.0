using StartUp.Model;
using WhatWillWeEat2._0.ViewModel;

namespace WhatWillWeEat2._0;

public partial class AllergenPage : ContentPage
{
	private AllergenPageViewModel allergenVM;

	public AllergenPage(Allergen allergen)
	{
		allergenVM = new AllergenPageViewModel(allergen);
        this.BindingContext = allergenVM;
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((AllergenPageViewModel)BindingContext).LoadAllergicRecipes();
    }
}