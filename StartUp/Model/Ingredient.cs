using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StartUp.Model
{
    [Table("Ingredient")]
    public class Ingredient
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string Name { get; set; }

        public double? Quantity { get; set; }

        public string? Unit { get; set; }

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
                return $"{Quantity} {Unit} {Name}";
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

        public Ingredient Clone()
        {
            return new Ingredient
            {
                ID = this.ID,
                Name = this.Name,
                Quantity = this.Quantity,
                Unit = this.Unit,
            };
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void NotifyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

    }
}
