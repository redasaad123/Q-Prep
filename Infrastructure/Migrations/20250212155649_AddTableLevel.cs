using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LevelId",
                table: "IntermediateLevel",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "LevelId",
                table: "BeginnerLevel",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "LevelId",
                table: "AdvancedLevel",
                newName: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "IntermediateLevel",
                newName: "LevelId");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "BeginnerLevel",
                newName: "LevelId");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "AdvancedLevel",
                newName: "LevelId");
        }
    }
}
