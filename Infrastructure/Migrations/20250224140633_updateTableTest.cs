using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTableTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Framework",
                table: "RequestQuestions",
                newName: "FrameworkName");

            migrationBuilder.AddColumn<string>(
                name: "FrameworkId",
                table: "Test",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrameworkId",
                table: "Test");

            migrationBuilder.RenameColumn(
                name: "FrameworkName",
                table: "RequestQuestions",
                newName: "Framework");
        }
    }
}
