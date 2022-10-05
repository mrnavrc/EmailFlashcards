using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailFlashcards.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_FlashcardsSettings_FlashcardSettingsId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardsSettings_FlashcardSettingsId",
                table: "Flashcards");

            migrationBuilder.DropTable(
                name: "FlashcardSettingUser");

            migrationBuilder.DropIndex(
                name: "IX_Flashcards_FlashcardSettingsId",
                table: "Flashcards");

            migrationBuilder.DropIndex(
                name: "IX_Categories_FlashcardSettingsId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FlashcardSettingsId",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "FlashcardSettingsId",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardsSettings_UserId",
                table: "FlashcardsSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardsSettings_AspNetUsers_UserId",
                table: "FlashcardsSettings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardsSettings_AspNetUsers_UserId",
                table: "FlashcardsSettings");

            migrationBuilder.DropIndex(
                name: "IX_FlashcardsSettings_UserId",
                table: "FlashcardsSettings");

            migrationBuilder.AddColumn<int>(
                name: "FlashcardSettingsId",
                table: "Flashcards",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlashcardSettingsId",
                table: "Categories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FlashcardSettingUser",
                columns: table => new
                {
                    FlashcardSettingsId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardSettingUser", x => new { x.FlashcardSettingsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_FlashcardSettingUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashcardSettingUser_FlashcardsSettings_FlashcardSettingsId",
                        column: x => x.FlashcardSettingsId,
                        principalTable: "FlashcardsSettings",
                        principalColumn: "FlashcardSettingsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardSettingsId",
                table: "Flashcards",
                column: "FlashcardSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_FlashcardSettingsId",
                table: "Categories",
                column: "FlashcardSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSettingUser_UsersId",
                table: "FlashcardSettingUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_FlashcardsSettings_FlashcardSettingsId",
                table: "Categories",
                column: "FlashcardSettingsId",
                principalTable: "FlashcardsSettings",
                principalColumn: "FlashcardSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardsSettings_FlashcardSettingsId",
                table: "Flashcards",
                column: "FlashcardSettingsId",
                principalTable: "FlashcardsSettings",
                principalColumn: "FlashcardSettingsId");
        }
    }
}
