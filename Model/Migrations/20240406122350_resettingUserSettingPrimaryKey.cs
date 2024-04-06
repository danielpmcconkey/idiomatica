using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class resettingUserSettingPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSetting",
                table: "UserSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSetting",
                table: "UserSetting",
                columns: new[] { "UserId", "Key" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSetting",
                table: "UserSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSetting",
                table: "UserSetting",
                column: "UserId");
        }
    }
}
