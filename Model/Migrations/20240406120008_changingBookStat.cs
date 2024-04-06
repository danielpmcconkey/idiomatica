using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class changingBookStat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookStat",
                table: "BookStat");

            migrationBuilder.AlterColumn<int>(
                name: "Key",
                table: "BookStat",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookStat",
                table: "BookStat",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookStat",
                table: "BookStat");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "BookStat",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookStat",
                table: "BookStat",
                columns: new[] { "BookId", "Key" });
        }
    }
}
