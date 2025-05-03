using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Q_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Q_1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Q_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Q_3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Q_4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Q_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Test");
        }
    }
}
