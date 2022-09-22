using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailFlashcards.Migrations
{
    public partial class hour_removed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hour",
                table: "FlashcardsSettings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hour",
                table: "FlashcardsSettings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
