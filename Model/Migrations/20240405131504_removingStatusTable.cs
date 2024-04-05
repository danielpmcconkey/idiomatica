using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class removingStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Word_Status_StatusId",
                table: "Word");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Word_StatusId",
                table: "Word");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Word",
                newName: "StatusNew");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Sentence",
                newName: "Ordinal");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Paragraph",
                newName: "Ordinal");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Page",
                newName: "Ordinal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusNew",
                table: "Word",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "Ordinal",
                table: "Sentence",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "Ordinal",
                table: "Paragraph",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "Ordinal",
                table: "Page",
                newName: "Order");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Word_StatusId",
                table: "Word",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Word_Status_StatusId",
                table: "Word",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
