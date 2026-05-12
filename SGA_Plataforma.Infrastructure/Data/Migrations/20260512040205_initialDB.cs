using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SGA_Plataforma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Platform_User",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    tag = table.Column<string>(type: "text", nullable: true),
                    logo_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                    table.ForeignKey(
                        name: "FK_Role_Games_game_id",
                        column: x => x.game_id,
                        principalTable: "Games",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    is_profile_public = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.id);
                    table.ForeignKey(
                        name: "FK_Player_Platform_User_user_id",
                        column: x => x.user_id,
                        principalTable: "Platform_User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    banner_url = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    format = table.Column<string>(type: "text", nullable: true),
                    bracket_type = table.Column<string>(type: "text", nullable: true),
                    max_teams = table.Column<int>(type: "integer", nullable: true),
                    organizer = table.Column<string>(type: "text", nullable: true),
                    rulebook_url = table.Column<string>(type: "text", nullable: true),
                    prize_pool = table.Column<decimal>(type: "numeric", nullable: true),
                    region = table.Column<string>(type: "text", nullable: true),
                    timezone = table.Column<string>(type: "text", nullable: true),
                    patch_version = table.Column<string>(type: "text", nullable: true),
                    roster_lock_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: true),
                    game_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tournament_Games_game_id",
                        column: x => x.game_id,
                        principalTable: "Games",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Tournament_Status_status_id",
                        column: x => x.status_id,
                        principalTable: "Status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Game_Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nickname = table.Column<string>(type: "text", nullable: false),
                    tag = table.Column<string>(type: "text", nullable: true),
                    provider = table.Column<string>(type: "text", nullable: false),
                    external_account_id = table.Column<string>(type: "text", nullable: false),
                    region = table.Column<string>(type: "text", nullable: true),
                    shard = table.Column<string>(type: "text", nullable: true),
                    last_verified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game_Account", x => x.id);
                    table.ForeignKey(
                        name: "FK_Game_Account_Games_game_id",
                        column: x => x.game_id,
                        principalTable: "Games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Game_Account_Player_player_id",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team_Participant",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    joined_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    left_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_starter = table.Column<bool>(type: "boolean", nullable: false),
                    is_captain = table.Column<bool>(type: "boolean", nullable: false),
                    is_substitute = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Participant", x => x.id);
                    table.ForeignKey(
                        name: "FK_Team_Participant_Player_player_id",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_Participant_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_Participant_Team_team_id",
                        column: x => x.team_id,
                        principalTable: "Team",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    type_id = table.Column<int>(type: "integer", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    starts_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ends_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    advancement_rules = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.id);
                    table.ForeignKey(
                        name: "FK_Stage_Tournament_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "Tournament",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stage_Type_type_id",
                        column: x => x.type_id,
                        principalTable: "Type",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Tournament_Team",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: true),
                    seed = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament_Team", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tournament_Team_Status_status_id",
                        column: x => x.status_id,
                        principalTable: "Status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Tournament_Team_Team_team_id",
                        column: x => x.team_id,
                        principalTable: "Team",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tournament_Team_Tournament_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "Tournament",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stage_id = table.Column<int>(type: "integer", nullable: false),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    winner_team_id = table.Column<int>(type: "integer", nullable: true),
                    game_id = table.Column<int>(type: "integer", nullable: false),
                    best_of = table.Column<int>(type: "integer", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    finished_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.id);
                    table.ForeignKey(
                        name: "FK_Matches_Games_game_id",
                        column: x => x.game_id,
                        principalTable: "Games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Stage_stage_id",
                        column: x => x.stage_id,
                        principalTable: "Stage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Status_status_id",
                        column: x => x.status_id,
                        principalTable: "Status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Team_winner_team_id",
                        column: x => x.winner_team_id,
                        principalTable: "Team",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Matches_Tournament_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "Tournament",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Match_Maps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    map_name = table.Column<string>(type: "text", nullable: true),
                    map_order = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: true),
                    team_1_score = table.Column<int>(type: "integer", nullable: true),
                    team_2_score = table.Column<int>(type: "integer", nullable: true),
                    winner_team_id = table.Column<int>(type: "integer", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match_Maps", x => x.id);
                    table.ForeignKey(
                        name: "FK_Match_Maps_Matches_match_id",
                        column: x => x.match_id,
                        principalTable: "Matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Maps_Status_status_id",
                        column: x => x.status_id,
                        principalTable: "Status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Match_Maps_Team_winner_team_id",
                        column: x => x.winner_team_id,
                        principalTable: "Team",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Matches_Teams",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    side = table.Column<string>(type: "text", nullable: true),
                    score = table.Column<int>(type: "integer", nullable: true),
                    is_winner = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches_Teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Matches_match_id",
                        column: x => x.match_id,
                        principalTable: "Matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Team_team_id",
                        column: x => x.team_id,
                        principalTable: "Team",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team_Match_Stats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    rounds_won = table.Column<int>(type: "integer", nullable: false),
                    rounds_lost = table.Column<int>(type: "integer", nullable: false),
                    plants = table.Column<int>(type: "integer", nullable: false),
                    defuses = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Match_Stats", x => x.id);
                    table.ForeignKey(
                        name: "FK_Team_Match_Stats_Matches_match_id",
                        column: x => x.match_id,
                        principalTable: "Matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_Match_Stats_Team_team_id",
                        column: x => x.team_id,
                        principalTable: "Team",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Match_Lineups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    match_map_id = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    source = table.Column<string>(type: "text", nullable: true),
                    submitted_by_user_id = table.Column<int>(type: "integer", nullable: true),
                    locked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match_Lineups", x => x.id);
                    table.ForeignKey(
                        name: "FK_Match_Lineups_Match_Maps_match_map_id",
                        column: x => x.match_map_id,
                        principalTable: "Match_Maps",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Match_Lineups_Matches_match_id",
                        column: x => x.match_id,
                        principalTable: "Matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Lineups_Platform_User_submitted_by_user_id",
                        column: x => x.submitted_by_user_id,
                        principalTable: "Platform_User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Match_Lineups_Team_team_id",
                        column: x => x.team_id,
                        principalTable: "Team",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Player_Match_stats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    game_account_id = table.Column<int>(type: "integer", nullable: true),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    match_map_id = table.Column<int>(type: "integer", nullable: true),
                    kills = table.Column<int>(type: "integer", nullable: false),
                    deaths = table.Column<int>(type: "integer", nullable: false),
                    assists = table.Column<int>(type: "integer", nullable: false),
                    adr = table.Column<decimal>(type: "numeric", nullable: false),
                    hs_percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    first_kills = table.Column<int>(type: "integer", nullable: false),
                    kast = table.Column<decimal>(type: "numeric", nullable: false),
                    acs = table.Column<decimal>(type: "numeric", nullable: false),
                    role_name = table.Column<string>(type: "text", nullable: true),
                    character_name = table.Column<string>(type: "text", nullable: true),
                    stats_json = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player_Match_stats", x => x.id);
                    table.ForeignKey(
                        name: "FK_Player_Match_stats_Game_Account_game_account_id",
                        column: x => x.game_account_id,
                        principalTable: "Game_Account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Player_Match_stats_Match_Maps_match_map_id",
                        column: x => x.match_map_id,
                        principalTable: "Match_Maps",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Player_Match_stats_Matches_match_id",
                        column: x => x.match_id,
                        principalTable: "Matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Player_Match_stats_Player_player_id",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_map_id = table.Column<int>(type: "integer", nullable: false),
                    round_number = table.Column<int>(type: "integer", nullable: false),
                    winning_team_id = table.Column<int>(type: "integer", nullable: true),
                    winner_type = table.Column<string>(type: "text", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    finished_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rounds_Match_Maps_match_map_id",
                        column: x => x.match_map_id,
                        principalTable: "Match_Maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rounds_Team_winning_team_id",
                        column: x => x.winning_team_id,
                        principalTable: "Team",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Match_Lineup_Players",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_lineup_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    game_account_id = table.Column<int>(type: "integer", nullable: true),
                    role_id = table.Column<int>(type: "integer", nullable: true),
                    is_starter = table.Column<bool>(type: "boolean", nullable: false),
                    is_captain = table.Column<bool>(type: "boolean", nullable: false),
                    is_substitute = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match_Lineup_Players", x => x.id);
                    table.ForeignKey(
                        name: "FK_Match_Lineup_Players_Game_Account_game_account_id",
                        column: x => x.game_account_id,
                        principalTable: "Game_Account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Match_Lineup_Players_Match_Lineups_match_lineup_id",
                        column: x => x.match_lineup_id,
                        principalTable: "Match_Lineups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Lineup_Players_Player_player_id",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Lineup_Players_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Match_Events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    match_map_id = table.Column<int>(type: "integer", nullable: true),
                    round_id = table.Column<int>(type: "integer", nullable: true),
                    game_id = table.Column<int>(type: "integer", nullable: true),
                    event_type = table.Column<string>(type: "text", nullable: true),
                    event_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    provider = table.Column<string>(type: "text", nullable: true),
                    provider_event_id = table.Column<string>(type: "text", nullable: true),
                    ingestion_session_id = table.Column<string>(type: "text", nullable: true),
                    sequence_number = table.Column<long>(type: "bigint", nullable: true),
                    processing_status = table.Column<string>(type: "text", nullable: true),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dedup_key = table.Column<string>(type: "text", nullable: true),
                    source = table.Column<string>(type: "text", nullable: true),
                    payload_json = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match_Events", x => x.id);
                    table.ForeignKey(
                        name: "FK_Match_Events_Games_game_id",
                        column: x => x.game_id,
                        principalTable: "Games",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Match_Events_Match_Maps_match_map_id",
                        column: x => x.match_map_id,
                        principalTable: "Match_Maps",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Match_Events_Matches_match_id",
                        column: x => x.match_id,
                        principalTable: "Matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Events_Rounds_round_id",
                        column: x => x.round_id,
                        principalTable: "Rounds",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_Account_game_id",
                table: "Game_Account",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_Game_Account_player_id",
                table: "Game_Account",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_Game_Account_provider_external_account_id",
                table: "Game_Account",
                columns: new[] { "provider", "external_account_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Match_Events_game_id",
                table: "Match_Events",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Events_match_id",
                table: "Match_Events",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Events_match_map_id",
                table: "Match_Events",
                column: "match_map_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Events_provider_provider_event_id",
                table: "Match_Events",
                columns: new[] { "provider", "provider_event_id" },
                unique: true,
                filter: "provider_event_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Events_round_id",
                table: "Match_Events",
                column: "round_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Lineup_Players_game_account_id",
                table: "Match_Lineup_Players",
                column: "game_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Lineup_Players_match_lineup_id_player_id",
                table: "Match_Lineup_Players",
                columns: new[] { "match_lineup_id", "player_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Match_Lineup_Players_player_id",
                table: "Match_Lineup_Players",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Lineup_Players_role_id",
                table: "Match_Lineup_Players",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Lineups_match_id_team_id_match_map_id",
                table: "Match_Lineups",
                columns: new[] { "match_id", "team_id", "match_map_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Match_Lineups_match_map_id",
                table: "Match_Lineups",
                column: "match_map_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Lineups_submitted_by_user_id",
                table: "Match_Lineups",
                column: "submitted_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Lineups_team_id",
                table: "Match_Lineups",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Maps_match_id",
                table: "Match_Maps",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Maps_status_id",
                table: "Match_Maps",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Maps_winner_team_id",
                table: "Match_Maps",
                column: "winner_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_game_id",
                table: "Matches",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_stage_id",
                table: "Matches",
                column: "stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_status_id",
                table: "Matches",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_tournament_id",
                table: "Matches",
                column: "tournament_id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_winner_team_id",
                table: "Matches",
                column: "winner_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Teams_match_id_team_id",
                table: "Matches_Teams",
                columns: new[] { "match_id", "team_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Teams_team_id",
                table: "Matches_Teams",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_Platform_User_email",
                table: "Platform_User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platform_User_login",
                table: "Platform_User",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_user_id",
                table: "Player",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Match_stats_game_account_id",
                table: "Player_Match_stats",
                column: "game_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Match_stats_match_id",
                table: "Player_Match_stats",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Match_stats_match_map_id",
                table: "Player_Match_stats",
                column: "match_map_id");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Match_stats_player_id",
                table: "Player_Match_stats",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_game_id",
                table: "Role",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_match_map_id",
                table: "Rounds",
                column: "match_map_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_winning_team_id",
                table: "Rounds",
                column: "winning_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_tournament_id",
                table: "Stage",
                column: "tournament_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_type_id",
                table: "Stage",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Match_Stats_match_id",
                table: "Team_Match_Stats",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Match_Stats_team_id",
                table: "Team_Match_Stats",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Participant_player_id",
                table: "Team_Participant",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Participant_role_id",
                table: "Team_Participant",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Participant_team_id_player_id_joined_at",
                table: "Team_Participant",
                columns: new[] { "team_id", "player_id", "joined_at" });

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_game_id",
                table: "Tournament",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_status_id",
                table: "Tournament",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Team_status_id",
                table: "Tournament_Team",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Team_team_id",
                table: "Tournament_Team",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Team_tournament_id_team_id",
                table: "Tournament_Team",
                columns: new[] { "tournament_id", "team_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Match_Events");

            migrationBuilder.DropTable(
                name: "Match_Lineup_Players");

            migrationBuilder.DropTable(
                name: "Matches_Teams");

            migrationBuilder.DropTable(
                name: "Player_Match_stats");

            migrationBuilder.DropTable(
                name: "Team_Match_Stats");

            migrationBuilder.DropTable(
                name: "Team_Participant");

            migrationBuilder.DropTable(
                name: "Tournament_Team");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Match_Lineups");

            migrationBuilder.DropTable(
                name: "Game_Account");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Match_Maps");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Platform_User");

            migrationBuilder.DropTable(
                name: "Stage");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Tournament");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
