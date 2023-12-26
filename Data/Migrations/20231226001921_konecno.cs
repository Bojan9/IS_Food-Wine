using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bojan_recipe.Data.Migrations
{
    public partial class konecno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tutorial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Gallery",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tutorial");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Gallery");
        }
    }
}
