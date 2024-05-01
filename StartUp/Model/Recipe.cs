using SQLite;

namespace StartUp.Model
{
    [Table("Recipes")]
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }
        public List<RecipeIngredient> RecipeIngredients { get; set; }
        public string Description { get; set; }

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
            return this.ID == that.ID && this.Name == that.Name && this.RecipeIngredients == that.RecipeIngredients && this.Description == that.Description;
        }
    }
}
