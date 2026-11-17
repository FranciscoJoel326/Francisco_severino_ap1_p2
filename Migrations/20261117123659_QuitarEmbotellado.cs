it in Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FranciscoJoel_AP1_P2.Migrations
{
    /// <inheritdoc />
    public partial class QuitarEmbotellado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 1,
                column: "Nombre",
                value: "Cafe Moka");

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 2,
                column: "Nombre",
                value: "Cafe Vainilla");

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 3,
                column: "Nombre",
                value: "Cafe Caramelo");

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 4,
                column: "Nombre",
                value: "Doble Espresso");

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 5,
                column: "Nombre",
                value: "Cold Brew");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 1,
                column: "Nombre",
                value: "Cafe Moka Embotellado");

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 2,
                column: "Nombre",
                value: "Cafe Vainilla Embotellado");

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 3,
                column: "Nombre",
                value: "Cafe Caramelo Embotellado");

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 4,
                column: "Nombre",
                value: "Doble Espresso Embotellado");

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 5,
                column: "Nombre",
                value: "Cold Brew Embotellado");
        }
    }
}
