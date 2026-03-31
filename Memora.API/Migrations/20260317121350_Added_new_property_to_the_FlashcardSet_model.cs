
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleAUTH.Migrations
{
    /// <inheritdoc />
    public partial class Added_new_property_to_the_FlashcardSet_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SharingCode",
                table: "FlashcardSets",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SharingCode",
                table: "FlashcardSets");
        }
    }
}
