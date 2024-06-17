using WhatWillWeEat2._0.ViewModel;

namespace WhatWillWeEat2._0;

public partial class MyAllergensPage : ContentPage
{
	public MyAllergensPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((MyAllergensPageViewModel)BindingContext).LoadAllergens();
    }
}