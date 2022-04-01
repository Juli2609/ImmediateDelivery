using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmediateDelivery.Migrations
{
    public partial class AddIndexToCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.CreateIndex(
               name: "IX_Cities_Name",
               table: "Cities",
               column: "Name",
               unique: true);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropIndex(
                name: "IX_Cities_Name",
                table: "Cities");*/
        }
    }
}
