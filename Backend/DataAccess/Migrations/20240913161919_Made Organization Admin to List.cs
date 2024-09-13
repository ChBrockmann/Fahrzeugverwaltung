using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MadeOrganizationAdmintoList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AspNetUsers_OwnerId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUserModel_AspNetUsers_UsersId",
                table: "OrganizationUserModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationUserModel",
                table: "OrganizationUserModel");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUserModel_UsersId",
                table: "OrganizationUserModel");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OwnerId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "OrganizationUserModel",
                newName: "AdminsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationUserModel",
                table: "OrganizationUserModel",
                columns: new[] { "AdminsId", "Organization1Id" });

            migrationBuilder.CreateTable(
                name: "OrganizationUserModel1",
                columns: table => new
                {
                    Organization2Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUserModel1", x => new { x.Organization2Id, x.UsersId });
                    table.ForeignKey(
                        name: "FK_OrganizationUserModel1_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationUserModel1_Organizations_Organization2Id",
                        column: x => x.Organization2Id,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserModel_Organization1Id",
                table: "OrganizationUserModel",
                column: "Organization1Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserModel1_UsersId",
                table: "OrganizationUserModel1",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUserModel_AspNetUsers_AdminsId",
                table: "OrganizationUserModel",
                column: "AdminsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUserModel_AspNetUsers_AdminsId",
                table: "OrganizationUserModel");

            migrationBuilder.DropTable(
                name: "OrganizationUserModel1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationUserModel",
                table: "OrganizationUserModel");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUserModel_Organization1Id",
                table: "OrganizationUserModel");

            migrationBuilder.RenameColumn(
                name: "AdminsId",
                table: "OrganizationUserModel",
                newName: "UsersId");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Organizations",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationUserModel",
                table: "OrganizationUserModel",
                columns: new[] { "Organization1Id", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserModel_UsersId",
                table: "OrganizationUserModel",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OwnerId",
                table: "Organizations",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AspNetUsers_OwnerId",
                table: "Organizations",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUserModel_AspNetUsers_UsersId",
                table: "OrganizationUserModel",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
