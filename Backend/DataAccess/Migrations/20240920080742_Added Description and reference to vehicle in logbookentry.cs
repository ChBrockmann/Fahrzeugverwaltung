using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedDescriptionandreferencetovehicleinlogbookentry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogBookEntries_ReservationModels_AssociatedReservationId",
                table: "LogBookEntries");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssociatedReservationId",
                table: "LogBookEntries",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedVehicleId",
                table: "LogBookEntries",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "CurrentNumber",
                table: "LogBookEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LogBookEntries",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LogBookEntries_AssociatedVehicleId",
                table: "LogBookEntries",
                column: "AssociatedVehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogBookEntries_ReservationModels_AssociatedReservationId",
                table: "LogBookEntries",
                column: "AssociatedReservationId",
                principalTable: "ReservationModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogBookEntries_VehicleModels_AssociatedVehicleId",
                table: "LogBookEntries",
                column: "AssociatedVehicleId",
                principalTable: "VehicleModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogBookEntries_ReservationModels_AssociatedReservationId",
                table: "LogBookEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LogBookEntries_VehicleModels_AssociatedVehicleId",
                table: "LogBookEntries");

            migrationBuilder.DropIndex(
                name: "IX_LogBookEntries_AssociatedVehicleId",
                table: "LogBookEntries");

            migrationBuilder.DropColumn(
                name: "AssociatedVehicleId",
                table: "LogBookEntries");

            migrationBuilder.DropColumn(
                name: "CurrentNumber",
                table: "LogBookEntries");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LogBookEntries");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssociatedReservationId",
                table: "LogBookEntries",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_LogBookEntries_ReservationModels_AssociatedReservationId",
                table: "LogBookEntries",
                column: "AssociatedReservationId",
                principalTable: "ReservationModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
