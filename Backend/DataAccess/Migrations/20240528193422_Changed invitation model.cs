using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Changedinvitationmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_InvitationModels_InvitationModelId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_InvitationModelId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "InvitationModelId",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "IdentityRole<Guid>InvitationModel",
                columns: table => new
                {
                    InvitationModelId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RolesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole<Guid>InvitationModel", x => new { x.InvitationModelId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_IdentityRole<Guid>InvitationModel_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityRole<Guid>InvitationModel_InvitationModels_Invitatio~",
                        column: x => x.InvitationModelId,
                        principalTable: "InvitationModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole<Guid>InvitationModel_RolesId",
                table: "IdentityRole<Guid>InvitationModel",
                column: "RolesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole<Guid>InvitationModel");

            migrationBuilder.AddColumn<Guid>(
                name: "InvitationModelId",
                table: "AspNetRoles",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_InvitationModelId",
                table: "AspNetRoles",
                column: "InvitationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_InvitationModels_InvitationModelId",
                table: "AspNetRoles",
                column: "InvitationModelId",
                principalTable: "InvitationModels",
                principalColumn: "Id");
        }
    }
}
