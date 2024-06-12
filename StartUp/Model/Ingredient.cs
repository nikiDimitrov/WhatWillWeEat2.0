using SQLite;

namespace StartUp.Model
{
    [Table("Ingredient")]
    public class Ingredient
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; set; }
        public List<IngredientAllergen> IngredientAllergens { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as Ingredient;

            return this.Name == that.Name && this.Quantity == that.Quantity && this.Unit == that.Unit && this.RecipeIngredients == that.RecipeIngredients
                && this.IngredientAllergens == that.IngredientAllergens;
        }

        public override string ToString()
        {
            if (Quantity != 0 && Unit != null)
            {
                return $"{Quantity}{Unit} {Name}";
            }
            else if (Quantity != 0 && Unit == null)
            {
                return $"{Quantity} {Name}";
            }
            else
            {
                return $"{Name}";
            }
        }
    }
}
