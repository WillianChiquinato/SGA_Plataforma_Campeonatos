using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGA_Plataforma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerRefactory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "login",
                table: "Player");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Player",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "login",
                table: "Player",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
