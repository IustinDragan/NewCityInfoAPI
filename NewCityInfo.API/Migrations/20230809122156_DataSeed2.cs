using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewCityInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "A Gothic style cathedral, conceived by architects Jan and Piere Appelmans.");

            migrationBuilder.InsertData(
                table: "PointOfInterests",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 4, 2, "The finest example of railway architecture in Belgium.", "Antwerp Central Station" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "A Gothic style cathedral, conceived by architects...");
        }
    }
}
