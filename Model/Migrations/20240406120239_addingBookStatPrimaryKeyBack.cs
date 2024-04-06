using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class addingBookStatPrimaryKeyBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookStat",
                table: "BookStat");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookStat",
                table: "BookStat",
                columns: new[] { "BookId", "Key" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookStat",
                table: "BookStat");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookStat",
                table: "BookStat",
                column: "BookId");
        }
    }
}
