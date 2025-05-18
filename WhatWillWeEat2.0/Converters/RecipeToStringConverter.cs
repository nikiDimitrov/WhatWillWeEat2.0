using StartUp.Model;
using System.Globalization;

namespace WhatWillWeEat2._0.Converters
{
    public class RecipeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Recipe recipe = value as Recipe;
            return recipe.Name;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
