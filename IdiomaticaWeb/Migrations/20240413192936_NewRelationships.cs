using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdiomaticaWeb.Migrations
{
    /// <inheritdoc />
    public partial class NewRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Idioma");

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Dict1URI = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Dict2URI = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GoogleTranslateURI = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CharacterSubstitutions = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RegexpSplitSentences = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ExceptionsSplitSentences = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RegexpWordCharacters = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RemoveSpaces = table.Column<bool>(type: "bit", nullable: false),
                    SplitEachChar = table.Column<bool>(type: "bit", nullable: false),
                    RightToLeft = table.Column<bool>(type: "bit", nullable: false),
                    ShowRomanization = table.Column<bool>(type: "bit", nullable: false),
                    ParserType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SourceURI = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AudioFilename = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Word",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TextLowerCase = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Romanization = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TokenCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Word", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Word_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageUser",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalWordsRead = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageUser_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Idioma",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSetting",
                schema: "Idioma",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => new { x.UserId, x.Key });
                    table.ForeignKey(
                        name: "FK_UserSetting_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Idioma",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookStat",
                schema: "Idioma",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookStat", x => new { x.BookId, x.Key });
                    table.ForeignKey(
                        name: "FK_BookStat_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "Idioma",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    OriginalText = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Page_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "Idioma",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookUser",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    LanguageUserId = table.Column<int>(type: "int", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    CurrentPageID = table.Column<int>(type: "int", nullable: false),
                    AudioCurrentPos = table.Column<float>(type: "real", nullable: false),
                    AudioBookmarks = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookUser_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "Idioma",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookUser_LanguageUser_LanguageUserId",
                        column: x => x.LanguageUserId,
                        principalSchema: "Idioma",
                        principalTable: "LanguageUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WordUser",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    LanguageUserId = table.Column<int>(type: "int", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusChanged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordUser_LanguageUser_LanguageUserId",
                        column: x => x.LanguageUserId,
                        principalSchema: "Idioma",
                        principalTable: "LanguageUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordUser_Word_WordId",
                        column: x => x.WordId,
                        principalSchema: "Idioma",
                        principalTable: "Word",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Paragraph",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraph", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paragraph_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "Idioma",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookUserStat",
                schema: "Idioma",
                columns: table => new
                {
                    BookUserId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookUserStat", x => new { x.BookUserId, x.Key });
                    table.ForeignKey(
                        name: "FK_BookUserStat_BookUser_BookUserId",
                        column: x => x.BookUserId,
                        principalSchema: "Idioma",
                        principalTable: "BookUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageUser",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookUserId = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageUser_BookUser_BookUserId",
                        column: x => x.BookUserId,
                        principalSchema: "Idioma",
                        principalTable: "BookUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PageUser_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "Idioma",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sentence",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParagraphId = table.Column<int>(type: "int", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sentence_Paragraph_ParagraphId",
                        column: x => x.ParagraphId,
                        principalSchema: "Idioma",
                        principalTable: "Paragraph",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    SentenceId = table.Column<int>(type: "int", nullable: false),
                    Display = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_Sentence_SentenceId",
                        column: x => x.SentenceId,
                        principalSchema: "Idioma",
                        principalTable: "Sentence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Token_Word_WordId",
                        column: x => x.WordId,
                        principalSchema: "Idioma",
                        principalTable: "Word",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_LanguageId",
                schema: "Idioma",
                table: "Book",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_BookId",
                schema: "Idioma",
                table: "BookUser",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_LanguageUserId",
                schema: "Idioma",
                table: "BookUser",
                column: "LanguageUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageUser_LanguageId",
                schema: "Idioma",
                table: "LanguageUser",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageUser_UserId",
                schema: "Idioma",
                table: "LanguageUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Page_BookId",
                schema: "Idioma",
                table: "Page",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_PageUser_BookUserId",
                schema: "Idioma",
                table: "PageUser",
                column: "BookUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PageUser_PageId",
                schema: "Idioma",
                table: "PageUser",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraph_PageId",
                schema: "Idioma",
                table: "Paragraph",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Sentence_ParagraphId",
                schema: "Idioma",
                table: "Sentence",
                column: "ParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_Token_SentenceId",
                schema: "Idioma",
                table: "Token",
                column: "SentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Token_WordId",
                schema: "Idioma",
                table: "Token",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_Word_LanguageId_TextLowerCase",
                schema: "Idioma",
                table: "Word",
                columns: new[] { "LanguageId", "TextLowerCase" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordUser_LanguageUserId",
                schema: "Idioma",
                table: "WordUser",
                column: "LanguageUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordUser_WordId",
                schema: "Idioma",
                table: "WordUser",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookStat",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "BookUserStat",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "PageUser",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Token",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "UserSetting",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "WordUser",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "BookUser",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Sentence",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Word",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "LanguageUser",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Paragraph",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Page",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "Idioma");
        }
    }
}
