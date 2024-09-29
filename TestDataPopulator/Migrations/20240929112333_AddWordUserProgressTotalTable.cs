using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestDataPopulator.Migrations
{
    /// <inheritdoc />
    public partial class AddWordUserProgressTotalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordUserProgressTotal",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordUserProgressTotal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordUserProgressTotal_LanguageUser_LanguageUserId",
                        column: x => x.LanguageUserId,
                        principalSchema: "Idioma",
                        principalTable: "LanguageUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordUserProgressTotal_LanguageUserId",
                schema: "Idioma",
                table: "WordUserProgressTotal",
                column: "LanguageUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordUserProgressTotal",
                schema: "Idioma");
        }
    }
}
