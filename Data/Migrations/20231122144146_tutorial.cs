using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bojan_recipe.Data.Migrations
{
    public partial class tutorial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    GalleryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.GalleryId);
                });

            migrationBuilder.CreateTable(
                name: "Tutorial",
                columns: table => new
                {
                    TutorialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorialDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorialVideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cateogry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutorial", x => x.TutorialId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropTable(
                name: "Tutorial");
        }
    }
}
