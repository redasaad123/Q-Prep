using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "security");

            migrationBuilder.CreateTable(
                name: "MainTrack",
                columns: table => new
                {
                    TrackId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TarckName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainTrack", x => x.TrackId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersAccount",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Framworks",
                columns: table => new
                {
                    FramworkId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FramworkName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainTrackId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framworks", x => x.FramworkId);
                    table.ForeignKey(
                        name: "FK_Framworks_MainTrack_MainTrackId",
                        column: x => x.MainTrackId,
                        principalTable: "MainTrack",
                        principalColumn: "TrackId");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "security",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokeOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => new { x.AppUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshToken_UsersAccount_AppUserId",
                        column: x => x.AppUserId,
                        principalSchema: "security",
                        principalTable: "UsersAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_UsersAccount_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "UsersAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                schema: "security",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_UsersAccount_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "UsersAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "security",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "security",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_UsersAccount_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "UsersAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                schema: "security",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_UsersAccount_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "UsersAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedLevel",
                columns: table => new
                {
                    LevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Questions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FramworkId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedLevel", x => x.LevelId);
                    table.ForeignKey(
                        name: "FK_AdvancedLevel_Framworks_FramworkId",
                        column: x => x.FramworkId,
                        principalTable: "Framworks",
                        principalColumn: "FramworkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BeginnerLevel",
                columns: table => new
                {
                    LevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Questions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FramworkId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeginnerLevel", x => x.LevelId);
                    table.ForeignKey(
                        name: "FK_BeginnerLevel_Framworks_FramworkId",
                        column: x => x.FramworkId,
                        principalTable: "Framworks",
                        principalColumn: "FramworkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntermediateLevel",
                columns: table => new
                {
                    LevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Questions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FramworkId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntermediateLevel", x => x.LevelId);
                    table.ForeignKey(
                        name: "FK_IntermediateLevel_Framworks_FramworkId",
                        column: x => x.FramworkId,
                        principalTable: "Framworks",
                        principalColumn: "FramworkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedLevel_FramworkId",
                table: "AdvancedLevel",
                column: "FramworkId");

            migrationBuilder.CreateIndex(
                name: "IX_BeginnerLevel_FramworkId",
                table: "BeginnerLevel",
                column: "FramworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Framworks_MainTrackId",
                table: "Framworks",
                column: "MainTrackId");

            migrationBuilder.CreateIndex(
                name: "IX_IntermediateLevel_FramworkId",
                table: "IntermediateLevel",
                column: "FramworkId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                schema: "security",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "security",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                schema: "security",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                schema: "security",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "security",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "security",
                table: "UsersAccount",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "security",
                table: "UsersAccount",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancedLevel");

            migrationBuilder.DropTable(
                name: "BeginnerLevel");

            migrationBuilder.DropTable(
                name: "IntermediateLevel");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "security");

            migrationBuilder.DropTable(
                name: "RoleClaim",
                schema: "security");

            migrationBuilder.DropTable(
                name: "UserClaim",
                schema: "security");

            migrationBuilder.DropTable(
                name: "UserLogin",
                schema: "security");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "security");

            migrationBuilder.DropTable(
                name: "UserToken",
                schema: "security");

            migrationBuilder.DropTable(
                name: "Framworks");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "security");

            migrationBuilder.DropTable(
                name: "UsersAccount",
                schema: "security");

            migrationBuilder.DropTable(
                name: "MainTrack");
        }
    }
}
