using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmailFlashcards.Migrations
{
    public partial class settings_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashcardsSettings",
                columns: table => new
                {
                    FlashcardSettingsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlashcardEmailAdress = table.Column<string>(type: "text", nullable: true),
                    FlashcardsPerDay = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardsSettings", x => x.FlashcardSettingsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashcardsSettings");
        }
    }
}
