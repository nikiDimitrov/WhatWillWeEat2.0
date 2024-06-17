using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StartUp.Migrations
{
    /// <inheritdoc />
    public partial class AddTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Allergens",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "боб" },
                    { 2, "леща" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "ID", "Name", "Quantity", "Unit" },
                values: new object[,]
                {
                    { 1, "боб", 3.0, "кг" },
                    { 2, "леща", 0.0, "" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[,]
                {
                    { 3, "Свари боб", "Боб" },
                    { 4, "Свари леща", "Леща" }
                });

            migrationBuilder.InsertData(
                table: "IngredientAllergens",
                columns: new[] { "AllergenId", "IngredientId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "RecipeIngredients",
                columns: new[] { "IngredientId", "RecipeId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 2, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IngredientAllergens",
                keyColumns: new[] { "AllergenId", "IngredientId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "IngredientAllergens",
                keyColumns: new[] { "AllergenId", "IngredientId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "RecipeIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "RecipeIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Allergens",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "ID",
                keyValue: 4);
        }
    }
}
