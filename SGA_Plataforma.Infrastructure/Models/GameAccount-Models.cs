using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Game_Account")]
public class GameAccount : BaseEntity
{
    [Required]
    [Column("nickname")]
    public string Nickname { get; set; } = null!;

    [Column("tag")]
    public string? Tag { get; set; }

    [Required]
    [Column("provider")]
    public string Provider { get; set; } = null!;

    [Required]
    [Column("external_account_id")]
    public string ExternalAccountId { get; set; } = null!;

    [Column("region")]
    public string? Region { get; set; }

    [Column("shard")]
    public string? Shard { get; set; }

    [Column("last_verified_at")]
    public DateTime? LastVerifiedAt { get; set; }

    [Column("is_verified")]
    public bool IsVerified { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("player_id")]
    public int PlayerId { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }

    [ForeignKey(nameof(PlayerId))]
    public Player? Player { get; set; }
}