using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedOriginanddestinationadress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationAdress",
                table: "ReservationModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OriginAdress",
                table: "ReservationModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedReservationId",
                table: "LogBookEntries",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_LogBookEntries_AssociatedReservationId",
                table: "LogBookEntries",
                column: "AssociatedReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogBookEntries_ReservationModels_AssociatedReservationId",
                table: "LogBookEntries",
                column: "AssociatedReservationId",
                principalTable: "ReservationModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogBookEntries_ReservationModels_AssociatedReservationId",
                table: "LogBookEntries");

            migrationBuilder.DropIndex(
                name: "IX_LogBookEntries_AssociatedReservationId",
                table: "LogBookEntries");

            migrationBuilder.DropColumn(
                name: "DestinationAdress",
                table: "ReservationModels");

            migrationBuilder.DropColumn(
                name: "OriginAdress",
                table: "ReservationModels");

            migrationBuilder.DropColumn(
                name: "AssociatedReservationId",
                table: "LogBookEntries");
        }
    }
}
