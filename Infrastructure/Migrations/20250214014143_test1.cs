using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Q_4",
                table: "Test",
                newName: "Qeuestion");

            migrationBuilder.RenameColumn(
                name: "Q_3",
                table: "Test",
                newName: "A_4");

            migrationBuilder.RenameColumn(
                name: "Q_2",
                table: "Test",
                newName: "A_3");

            migrationBuilder.RenameColumn(
                name: "Q_1",
                table: "Test",
                newName: "A_2");

            migrationBuilder.RenameColumn(
                name: "Answers",
                table: "Test",
                newName: "CorrectAnswers");

            migrationBuilder.AddColumn<string>(
                name: "A_1",
                table: "Test",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "A_1",
                table: "Test");

            migrationBuilder.RenameColumn(
                name: "Qeuestion",
                table: "Test",
                newName: "Q_4");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswers",
                table: "Test",
                newName: "Answers");

            migrationBuilder.RenameColumn(
                name: "A_4",
                table: "Test",
                newName: "Q_3");

            migrationBuilder.RenameColumn(
                name: "A_3",
                table: "Test",
                newName: "Q_2");

            migrationBuilder.RenameColumn(
                name: "A_2",
                table: "Test",
                newName: "Q_1");
        }
    }
}
