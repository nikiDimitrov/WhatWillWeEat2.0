using SQLite;

namespace StartUp.Model
{
    [Table("IngredientAllergen")]
    public class IngredientAllergen
    {
        [PrimaryKey]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        [PrimaryKey]
        public int AllergenId { get; set; }

        public Allergen Allergen { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as IngredientAllergen;

            return this.IngredientId == that.IngredientId && this.Ingredient == that.Ingredient && this.AllergenId == that.AllergenId
                && this.Allergen == that.Allergen;
        }
    }
}
