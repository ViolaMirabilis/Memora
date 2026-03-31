using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleAUTH.Migrations
{
    /// <inheritdoc />
    public partial class Added_IsSharingProperty_To_FlashcardSetModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSharing",
                table: "FlashcardSets",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSharing",
                table: "FlashcardSets");
        }
    }
}
