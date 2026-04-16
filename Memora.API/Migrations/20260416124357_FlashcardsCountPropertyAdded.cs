using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleAUTH.Migrations
{
    /// <inheritdoc />
    public partial class FlashcardsCountPropertyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlashcardsCount",
                table: "FlashcardSets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlashcardsCount",
                table: "FlashcardSets");
        }
    }
}
