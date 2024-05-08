using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Renamedproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToDateInclusive",
                table: "ReservationModels",
                newName: "StartDateInclusive");

            migrationBuilder.RenameColumn(
                name: "FromDateInclusive",
                table: "ReservationModels",
                newName: "EndDateInclusive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDateInclusive",
                table: "ReservationModels",
                newName: "ToDateInclusive");

            migrationBuilder.RenameColumn(
                name: "EndDateInclusive",
                table: "ReservationModels",
                newName: "FromDateInclusive");
        }
    }
}
