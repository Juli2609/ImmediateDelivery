using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmediateDelivery.Migrations
{
    public partial class AddPackageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Package_AspNetUsers_UserId",
                table: "Package");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Package",
                table: "Package");

            migrationBuilder.RenameTable(
                name: "Package",
                newName: "Packages");

            migrationBuilder.RenameIndex(
                name: "IX_Package_UserId",
                table: "Packages",
                newName: "IX_Packages_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Packages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PackageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageTypes_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_Id",
                table: "Packages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackageTypes_Description_Id",
                table: "PackageTypes",
                columns: new[] { "Description", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackageTypes_PackageId",
                table: "PackageTypes",
                column: "PackageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_AspNetUsers_UserId",
                table: "Packages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_AspNetUsers_UserId",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "PackageTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_Id",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Packages");

            migrationBuilder.RenameTable(
                name: "Packages",
                newName: "Package");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_UserId",
                table: "Package",
                newName: "IX_Package_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Package",
                table: "Package",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Package_AspNetUsers_UserId",
                table: "Package",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
