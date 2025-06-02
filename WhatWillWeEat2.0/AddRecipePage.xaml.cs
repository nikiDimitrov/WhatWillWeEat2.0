using StartUp.Model;
using WhatWillWeEat2._0.ViewModel;

namespace WhatWillWeEat2._0;

public partial class AddRecipePage : ContentPage
{
	public AddRecipePage()
	{
		InitializeComponent();
	}
    private void RemoveIngredient_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is RecipeIngredient ri)
        {
            var vm = BindingContext as AddRecipePageViewModel;
            vm.EditableIngredients.Remove(ri);
        }
    }

}