using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailFlashcards.Migrations
{
    public partial class CategoryModel_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "FlashcardsCategoryName",
                table: "Categories",
                newName: "FlashcardCategoryName");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "FlashcardCategoryName",
                table: "Categories",
                newName: "FlashcardsCategoryName");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
