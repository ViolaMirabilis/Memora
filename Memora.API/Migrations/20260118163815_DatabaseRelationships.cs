using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleAUTH.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSets_FolderId",
                table: "FlashcardSets",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSets_UserId",
                table: "FlashcardSets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardSetId",
                table: "Flashcards",
                column: "FlashcardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardFolders_UserId",
                table: "FlashcardFolders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardFolders_Users_UserId",
                table: "FlashcardFolders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetId",
                table: "Flashcards",
                column: "FlashcardSetId",
                principalTable: "FlashcardSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_FlashcardFolders_FolderId",
                table: "FlashcardSets",
                column: "FolderId",
                principalTable: "FlashcardFolders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_Users_UserId",
                table: "FlashcardSets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardFolders_Users_UserId",
                table: "FlashcardFolders");

            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetId",
                table: "Flashcards");

            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_FlashcardFolders_FolderId",
                table: "FlashcardSets");

            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_Users_UserId",
                table: "FlashcardSets");

            migrationBuilder.DropIndex(
                name: "IX_FlashcardSets_FolderId",
                table: "FlashcardSets");

            migrationBuilder.DropIndex(
                name: "IX_FlashcardSets_UserId",
                table: "FlashcardSets");

            migrationBuilder.DropIndex(
                name: "IX_Flashcards_FlashcardSetId",
                table: "Flashcards");

            migrationBuilder.DropIndex(
                name: "IX_FlashcardFolders_UserId",
                table: "FlashcardFolders");
        }
    }
}
