using SQLite;

namespace StartUp.Model
{
    [Table("Allergen")]
    public class Allergen
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }

        public ICollection<IngredientAllergen> IngredientAllergens { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as Allergen;

            return this.ID == that.ID && this.Name == that.Name && this.Quantity == that.Quantity && this.Unit == that.Unit
                && this.IngredientAllergens == that.IngredientAllergens;
        }
    }
}
