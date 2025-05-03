using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetableframework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedLevel_Framworks_FramworkId",
                table: "AdvancedLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_BeginnerLevel_Framworks_FramworkId",
                table: "BeginnerLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_IntermediateLevel_Framworks_FramworkId",
                table: "IntermediateLevel");

            migrationBuilder.RenameColumn(
                name: "FramworkId",
                table: "IntermediateLevel",
                newName: "FrameworkId");

            migrationBuilder.RenameIndex(
                name: "IX_IntermediateLevel_FramworkId",
                table: "IntermediateLevel",
                newName: "IX_IntermediateLevel_FrameworkId");

            migrationBuilder.RenameColumn(
                name: "FramworkName",
                table: "Framworks",
                newName: "FrameworkName");

            migrationBuilder.RenameColumn(
                name: "FramworkId",
                table: "Framworks",
                newName: "FrameworkId");

            migrationBuilder.RenameColumn(
                name: "FramworkId",
                table: "BeginnerLevel",
                newName: "FrameworkId");

            migrationBuilder.RenameIndex(
                name: "IX_BeginnerLevel_FramworkId",
                table: "BeginnerLevel",
                newName: "IX_BeginnerLevel_FrameworkId");

            migrationBuilder.RenameColumn(
                name: "FramworkId",
                table: "AdvancedLevel",
                newName: "FrameworkId");

            migrationBuilder.RenameIndex(
                name: "IX_AdvancedLevel_FramworkId",
                table: "AdvancedLevel",
                newName: "IX_AdvancedLevel_FrameworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedLevel_Framworks_FrameworkId",
                table: "AdvancedLevel",
                column: "FrameworkId",
                principalTable: "Framworks",
                principalColumn: "FrameworkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BeginnerLevel_Framworks_FrameworkId",
                table: "BeginnerLevel",
                column: "FrameworkId",
                principalTable: "Framworks",
                principalColumn: "FrameworkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IntermediateLevel_Framworks_FrameworkId",
                table: "IntermediateLevel",
                column: "FrameworkId",
                principalTable: "Framworks",
                principalColumn: "FrameworkId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedLevel_Framworks_FrameworkId",
                table: "AdvancedLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_BeginnerLevel_Framworks_FrameworkId",
                table: "BeginnerLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_IntermediateLevel_Framworks_FrameworkId",
                table: "IntermediateLevel");

            migrationBuilder.RenameColumn(
                name: "FrameworkId",
                table: "IntermediateLevel",
                newName: "FramworkId");

            migrationBuilder.RenameIndex(
                name: "IX_IntermediateLevel_FrameworkId",
                table: "IntermediateLevel",
                newName: "IX_IntermediateLevel_FramworkId");

            migrationBuilder.RenameColumn(
                name: "FrameworkName",
                table: "Framworks",
                newName: "FramworkName");

            migrationBuilder.RenameColumn(
                name: "FrameworkId",
                table: "Framworks",
                newName: "FramworkId");

            migrationBuilder.RenameColumn(
                name: "FrameworkId",
                table: "BeginnerLevel",
                newName: "FramworkId");

            migrationBuilder.RenameIndex(
                name: "IX_BeginnerLevel_FrameworkId",
                table: "BeginnerLevel",
                newName: "IX_BeginnerLevel_FramworkId");

            migrationBuilder.RenameColumn(
                name: "FrameworkId",
                table: "AdvancedLevel",
                newName: "FramworkId");

            migrationBuilder.RenameIndex(
                name: "IX_AdvancedLevel_FrameworkId",
                table: "AdvancedLevel",
                newName: "IX_AdvancedLevel_FramworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedLevel_Framworks_FramworkId",
                table: "AdvancedLevel",
                column: "FramworkId",
                principalTable: "Framworks",
                principalColumn: "FramworkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BeginnerLevel_Framworks_FramworkId",
                table: "BeginnerLevel",
                column: "FramworkId",
                principalTable: "Framworks",
                principalColumn: "FramworkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IntermediateLevel_Framworks_FramworkId",
                table: "IntermediateLevel",
                column: "FramworkId",
                principalTable: "Framworks",
                principalColumn: "FramworkId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
