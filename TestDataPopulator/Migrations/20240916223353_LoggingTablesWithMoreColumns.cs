using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestDataPopulator.Migrations
{
    /// <inheritdoc />
    public partial class LoggingTablesWithMoreColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemberName",
                schema: "dbo",
                table: "LogMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceFilePath",
                schema: "dbo",
                table: "LogMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SourceLineNumber",
                schema: "dbo",
                table: "LogMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberName",
                schema: "dbo",
                table: "LogMessage");

            migrationBuilder.DropColumn(
                name: "SourceFilePath",
                schema: "dbo",
                table: "LogMessage");

            migrationBuilder.DropColumn(
                name: "SourceLineNumber",
                schema: "dbo",
                table: "LogMessage");
        }
    }
}
