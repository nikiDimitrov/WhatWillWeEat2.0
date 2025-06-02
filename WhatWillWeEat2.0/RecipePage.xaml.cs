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

    private void Entry_Completed(object sender, EventArgs e)
    {
        if (BindingContext is RecipePageViewModel vm)
            vm.SaveCommand.NotifyCanExecuteChanged();
    }

    private void RemoveIngredient_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var ingredient = (RecipeIngredient)button.BindingContext;
        recipeVM.RemoveIngredientCommand.Execute(ingredient);
    }
}