using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGA_Plataforma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nullableWin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Team_winner_team_id",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_winner_team_id",
                table: "Matches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Matches_winner_team_id",
                table: "Matches",
                column: "winner_team_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Team_winner_team_id",
                table: "Matches",
                column: "winner_team_id",
                principalTable: "Team",
                principalColumn: "id");
        }
    }
}
