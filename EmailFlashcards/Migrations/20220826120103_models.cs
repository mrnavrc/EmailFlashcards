using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailFlashcards.Migrations
{
    public partial class models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_AspNetUsers_UserId",
                table: "Contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "Flashcards");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_UserId",
                table: "Flashcards",
                newName: "IX_Flashcards_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flashcards",
                table: "Flashcards",
                column: "Flashcard_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_AspNetUsers_UserId",
                table: "Flashcards",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_AspNetUsers_UserId",
                table: "Flashcards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flashcards",
                table: "Flashcards");

            migrationBuilder.RenameTable(
                name: "Flashcards",
                newName: "Contacts");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_UserId",
                table: "Contacts",
                newName: "IX_Contacts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "Flashcard_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_AspNetUsers_UserId",
                table: "Contacts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
