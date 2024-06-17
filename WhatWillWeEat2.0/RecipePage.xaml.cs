using StartUp.Model;
using WhatWillWeEat2._0.ViewModel;

namespace WhatWillWeEat2._0;

public partial class RecipePage : ContentPage
{
	public RecipePageViewModel recipeVM;

	public RecipePage(Recipe recipe)
	{
		recipeVM = new RecipePageViewModel(recipe);
		this.BindingContext = recipeVM;
		InitializeComponent();
	}
}