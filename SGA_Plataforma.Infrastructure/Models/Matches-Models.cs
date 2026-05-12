using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Matches")]
public class Match : BaseEntity
{
    [Column("stage_id")]
    public int StageId { get; set; }

    [Column("tournament_id")]
    public int TournamentId { get; set; }

    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("winner_team_id")]
    public int? WinnerTeamId { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("best_of")]
    public int? BestOf { get; set; }

    [Column("started_at")]
    public DateTime? StartedAt { get; set; }

    [Column("finished_at")]
    public DateTime? FinishedAt { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(StageId))]
    public Stage? Stage { get; set; }

    [ForeignKey(nameof(TournamentId))]
    public Tournament? Tournament { get; set; }

    [ForeignKey(nameof(StatusId))]
    public Status? Status { get; set; }

    [ForeignKey(nameof(WinnerTeamId))]
    public Team? WinnerTeam { get; set; }

    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }
}