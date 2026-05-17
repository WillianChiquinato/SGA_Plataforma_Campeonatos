using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGA_Plataforma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class matchesPositionBract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bracket_position",
                table: "Matches",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bracket_position",
                table: "Matches");
        }
    }
}
