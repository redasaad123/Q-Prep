using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inAppuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "security",
                table: "UsersAccount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthDay",
                schema: "security",
                table: "UsersAccount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "security",
                table: "UsersAccount",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "security",
                table: "UsersAccount");

            migrationBuilder.DropColumn(
                name: "BirthDay",
                schema: "security",
                table: "UsersAccount");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "security",
                table: "UsersAccount");
        }
    }
}
