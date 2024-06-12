using StartUp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatWillWeEat2._0.Services
{
    public static class IngredientUtils
    {
        public static Ingredient? ConvertStringToIngredient(string ingredientString)
        {
            string[] splittedIngredientString = ingredientString.Split(" ")
                .ToArray();

            try
            {
                switch (splittedIngredientString.Length)
                {
                    case 1:
                        return new Ingredient()
                        {
                            Name = ingredientString
                        };
                    case 2:
                        return new Ingredient()
                        {
                            Quantity = double.Parse(splittedIngredientString[0]),
                            Name = splittedIngredientString[1]
                        };
                    case 3:
                        return new Ingredient()
                        {
                            Quantity = double.Parse(splittedIngredientString[0]),
                            Unit = splittedIngredientString[1],
                            Name = splittedIngredientString[2]
                        };
                    default:
                        throw new InvalidIngredientException("Ingredient is in invalid format!");
                }
            }
            catch
            {
                throw new InvalidIngredientException("Ingredient is in invalid format!");
            }
            
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
