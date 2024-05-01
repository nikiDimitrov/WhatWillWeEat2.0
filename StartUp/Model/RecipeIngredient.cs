using SQLite;

namespace StartUp.Model
{
    [Table("Recipe_Ingredient")]
    public class RecipeIngredient
    {
        [PrimaryKey]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [PrimaryKey]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as RecipeIngredient;

            return this.RecipeId == that.RecipeId && this.Recipe == that.Recipe
                && this.IngredientId == that.IngredientId && this.Ingredient == that.Ingredient;
        }
    }
}
