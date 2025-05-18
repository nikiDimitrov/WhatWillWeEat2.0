using Microsoft.EntityFrameworkCore;
using StartUp;
using StartUp.Model;
using System.Globalization;
using System.Text;

namespace WhatWillWeEat2._0.Converters
{
    public class IngredientToStringConverter : IValueConverter
    {
        private DatabaseContext _dbContext = new DatabaseContext();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            StringBuilder sb = new StringBuilder();

            HashSet<RecipeIngredient> ingredientsHashSet = value as HashSet<RecipeIngredient>;
            List<RecipeIngredient> recipeIngredients = ingredientsHashSet.ToList();
            List<Ingredient> ingredients = new List<Ingredient>();

            foreach (RecipeIngredient recipeIngredient in recipeIngredients)
            {
                Ingredient ingredient = _dbContext.RecipeIngredients
                    .Include(ri => ri.Ingredient)
                    .FirstOrDefault(ri => ri.IngredientId == recipeIngredient.IngredientId).Ingredient;

                ingredients.Add(ingredient);
            }
            

            if (ingredients == null || ingredients.Count == 0)
            {
                return "";
            }

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (i < ingredients.Count - 1)
                {
                    sb.Append($"{ingredients[i]}, ");
                }
                else
                {
                    sb.Append($"{ingredients[i]}");
                }

            }

            return sb.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
