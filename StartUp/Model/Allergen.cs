using SQLite;

namespace StartUp.Model
{
    [Table("Allergen")]
    public class Allergen
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string Name { get; set; }

        public ICollection<IngredientAllergen> IngredientAllergens { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as Allergen;

            return this.ID == that.ID && this.Name == that.Name
                && this.IngredientAllergens == that.IngredientAllergens;
        }

        public Allergen Clone()
        {
            return new Allergen { ID = this.ID, Name = this.Name };
        }
    }
}
