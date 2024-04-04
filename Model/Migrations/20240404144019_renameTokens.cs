using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class renameTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Sentence_SentenceId",
                table: "Tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Word_WordId",
                table: "Tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tokens",
                table: "Tokens");

            migrationBuilder.RenameTable(
                name: "Tokens",
                newName: "Token");

            migrationBuilder.RenameIndex(
                name: "IX_Tokens_WordId",
                table: "Token",
                newName: "IX_Token_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_Tokens_SentenceId",
                table: "Token",
                newName: "IX_Token_SentenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Token",
                table: "Token",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Token_Sentence_SentenceId",
                table: "Token",
                column: "SentenceId",
                principalTable: "Sentence",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Token_Word_WordId",
                table: "Token",
                column: "WordId",
                principalTable: "Word",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Token_Sentence_SentenceId",
                table: "Token");

            migrationBuilder.DropForeignKey(
                name: "FK_Token_Word_WordId",
                table: "Token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Token",
                table: "Token");

            migrationBuilder.RenameTable(
                name: "Token",
                newName: "Tokens");

            migrationBuilder.RenameIndex(
                name: "IX_Token_WordId",
                table: "Tokens",
                newName: "IX_Tokens_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_Token_SentenceId",
                table: "Tokens",
                newName: "IX_Tokens_SentenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tokens",
                table: "Tokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Sentence_SentenceId",
                table: "Tokens",
                column: "SentenceId",
                principalTable: "Sentence",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Word_WordId",
                table: "Tokens",
                column: "WordId",
                principalTable: "Word",
                principalColumn: "Id");
        }
    }
}
