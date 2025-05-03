using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableRequest1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RequestQuestions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_RequestQuestions_UserId",
                table: "RequestQuestions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestQuestions_UsersAccount_UserId",
                table: "RequestQuestions",
                column: "UserId",
                principalSchema: "security",
                principalTable: "UsersAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestQuestions_UsersAccount_UserId",
                table: "RequestQuestions");

            migrationBuilder.DropIndex(
                name: "IX_RequestQuestions_UserId",
                table: "RequestQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RequestQuestions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
