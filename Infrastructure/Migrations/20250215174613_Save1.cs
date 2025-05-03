using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Save1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "SaveQuestions",
                newName: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "SaveQuestions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "SaveQuestions");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "SaveQuestions",
                newName: "QuestionId");
        }
    }
}
