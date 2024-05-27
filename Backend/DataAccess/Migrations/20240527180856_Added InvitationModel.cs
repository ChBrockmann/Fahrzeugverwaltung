using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedInvitationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InvitationModelId",
                table: "AspNetRoles",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "InvitationModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AcceptedById = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AcceptedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvitationModels_AspNetUsers_AcceptedById",
                        column: x => x.AcceptedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvitationModels_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_InvitationModelId",
                table: "AspNetRoles",
                column: "InvitationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InvitationModels_AcceptedById",
                table: "InvitationModels",
                column: "AcceptedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvitationModels_CreatedById",
                table: "InvitationModels",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_InvitationModels_InvitationModelId",
                table: "AspNetRoles",
                column: "InvitationModelId",
                principalTable: "InvitationModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_InvitationModels_InvitationModelId",
                table: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "InvitationModels");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_InvitationModelId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "InvitationModelId",
                table: "AspNetRoles");
        }
    }
}
