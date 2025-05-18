using StartUp;
using StartUp.Model;

namespace WhatWillWeEat2._0.Services
{
    public static class IngredientUtils
    {
        private static DatabaseContext _dbContext = new DatabaseContext();
        public static Ingredient? ConvertStringToIngredient(string ingredientString)
        {
            string[] splittedIngredientString = ingredientString.Split(" ")
                .ToArray();

            Ingredient ingredient;

            try
            {
                switch (splittedIngredientString.Length)
                {
                    case 1:
                        ingredient = new Ingredient()
                        {
                            Name = ingredientString
                        };
                        break;
                    case 2:
                        ingredient = new Ingredient()
                        {
                            Quantity = double.Parse(splittedIngredientString[0]),
                            Name = splittedIngredientString[1]
                        };
                        break;
                    case 3:
                        ingredient = new Ingredient()
                        {
                            Quantity = double.Parse(splittedIngredientString[0]),
                            Unit = splittedIngredientString[1],
                            Name = splittedIngredientString[2]
                        };
                        break;
                    default:
                        throw new InvalidIngredientException("Ingredient is in invalid format!");
                }
            }
            catch
            {
                throw new InvalidIngredientException("Ingredient is in invalid format!");
            }

            return ingredient;
        }

        public static object? GetPropertyValue(object src, string propertyName)
        {
            if (propertyName.Contains("."))
            {
                var splitIndex = propertyName.IndexOf('.');
                var parent = propertyName.Substring(0, splitIndex);
                var child = propertyName.Substring(splitIndex + 1);
                var obj = src?.GetType().GetProperty(parent)?.GetValue(src, null);
                return GetPropertyValue(obj, child);
            }

            return src?.GetType().GetProperty(propertyName)?.GetValue(src, null);
        }
    }
}
