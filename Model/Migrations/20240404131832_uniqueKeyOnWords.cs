using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class uniqueKeyOnWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Word_LanguageUserId",
                table: "Word");

            migrationBuilder.AlterColumn<string>(
                name: "TextLowerCase",
                table: "Word",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Word_LanguageUserId_TextLowerCase",
                table: "Word",
                columns: new[] { "LanguageUserId", "TextLowerCase" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Word_LanguageUserId_TextLowerCase",
                table: "Word");

            migrationBuilder.AlterColumn<string>(
                name: "TextLowerCase",
                table: "Word",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Word_LanguageUserId",
                table: "Word",
                column: "LanguageUserId");
        }
    }
}
