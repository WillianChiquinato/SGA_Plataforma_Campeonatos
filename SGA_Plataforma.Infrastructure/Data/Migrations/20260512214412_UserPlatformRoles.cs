using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SGA_Plataforma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPlatformRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Lineups_Platform_User_submitted_by_user_id",
                table: "Match_Lineups");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Platform_User_user_id",
                table: "Player");



            migrationBuilder.Sql(@"DROP TABLE IF EXISTS ""Platform_User"" CASCADE;");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_login",
                table: "User",
                column: "login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Lineups_User_submitted_by_user_id",
                table: "Match_Lineups",
                column: "submitted_by_user_id",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_User_user_id",
                table: "Player",
                column: "user_id",
                principalTable: "User",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the Platform_User table with CASCADE to remove dependencies
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS ""Platform_User"" CASCADE;");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_login",
                table: "User",
                column: "login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Lineups_User_submitted_by_user_id",
                table: "Match_Lineups",
                column: "submitted_by_user_id",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_User_user_id",
                table: "Player",
                column: "user_id",
                principalTable: "User",
                principalColumn: "id");
        }
    }
}
