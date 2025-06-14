﻿using SQLite;
using System.Collections.ObjectModel;

namespace StartUp.Model
{
    [Table("Recipe")]
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        public ObservableCollection<RecipeIngredient> RecipeIngredients { get; set; }

        public Recipe()
        {

        }
        public override string ToString()
        {
            return $"{Name}";
        }

        public override bool Equals(object obj)
        {
            var that = obj as Recipe;
            return this.ID == that.ID && this.Name == that.Name && this.RecipeIngredients.SequenceEqual(that.RecipeIngredients) && this.Description == that.Description;
        }

        public Recipe Clone()
        {
            return new Recipe
            {
                ID = this.ID,
                Name = this.Name,
                Description = this.Description,
                RecipeIngredients = this.RecipeIngredients
            };
        }

    }
}
