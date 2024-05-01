using SQLite;

namespace StartUp.Model
{
    [Table("Ingredients")]
    public class Allergen
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }

        public List<IngredientAllergen> IngredientAllergens { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as Allergen;

            return this.ID == that.ID && this.Name == that.Name && this.Quantity == that.Quantity && this.Unit == that.Unit
                && this.IngredientAllergens == that.IngredientAllergens;
        }
    }
}
