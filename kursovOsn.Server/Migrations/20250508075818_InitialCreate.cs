using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace kursovOsn.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tournament_Formats",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament_Formats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Sex = table.Column<string>(type: "text", nullable: false),
                    Sport_ID = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Sports_Sport_ID",
                        column: x => x.Sport_ID,
                        principalTable: "Sports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Sport_Statistics",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sport_ID = table.Column<int>(type: "integer", nullable: false),
                    Stat_Name = table.Column<string>(type: "text", nullable: false),
                    DataType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sport_Statistics", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sport_Statistics_Sports_Sport_ID",
                        column: x => x.Sport_ID,
                        principalTable: "Sports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Sport_ID = table.Column<int>(type: "integer", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Sports_Sport_ID",
                        column: x => x.Sport_ID,
                        principalTable: "Sports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Sport_ID = table.Column<int>(type: "integer", nullable: false),
                    Adress = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<string>(type: "text", nullable: false),
                    Info = table.Column<string>(type: "text", nullable: false),
                    Pol = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Format_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tournaments_Sports_Sport_ID",
                        column: x => x.Sport_ID,
                        principalTable: "Sports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tournaments_Tournament_Formats_Format_ID",
                        column: x => x.Format_ID,
                        principalTable: "Tournament_Formats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team_Entries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Team_ID = table.Column<int>(type: "integer", nullable: false),
                    User_ID = table.Column<string>(type: "text", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Entries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Team_Entries_AspNetUsers_User_ID",
                        column: x => x.User_ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Team_Entries_Teams_Team_ID",
                        column: x => x.Team_ID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Team_Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ID_User = table.Column<string>(type: "text", nullable: false),
                    ID_Team = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Players_AspNetUsers_ID_User",
                        column: x => x.ID_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Team_Players_Teams_ID_Team",
                        column: x => x.ID_Team,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tournament_Entries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ID_User = table.Column<string>(type: "text", nullable: true),
                    ID_Tournament = table.Column<int>(type: "integer", nullable: false),
                    ID_Team = table.Column<int>(type: "integer", nullable: true),
                    Info = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament_Entries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tournament_Entries_AspNetUsers_ID_User",
                        column: x => x.ID_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tournament_Entries_Teams_ID_Team",
                        column: x => x.ID_Team,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tournament_Entries_Tournaments_ID_Tournament",
                        column: x => x.ID_Tournament,
                        principalTable: "Tournaments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tournament_Participants",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tournament_ID = table.Column<int>(type: "integer", nullable: false),
                    User_ID = table.Column<string>(type: "text", nullable: true),
                    Team_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament_Participants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tournament_Participants_AspNetUsers_User_ID",
                        column: x => x.User_ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tournament_Participants_Teams_Team_ID",
                        column: x => x.Team_ID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tournament_Participants_Tournaments_Tournament_ID",
                        column: x => x.Tournament_ID,
                        principalTable: "Tournaments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tournament_Stages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tournament_ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Stage_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament_Stages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tournament_Stages_Tournaments_Tournament_ID",
                        column: x => x.Tournament_ID,
                        principalTable: "Tournaments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tournament_Standings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tournament_ID = table.Column<int>(type: "integer", nullable: false),
                    Team_ID = table.Column<int>(type: "integer", nullable: true),
                    Player_ID = table.Column<string>(type: "text", nullable: true),
                    Matches = table.Column<int>(type: "integer", nullable: false),
                    Wins = table.Column<int>(type: "integer", nullable: false),
                    Draws = table.Column<int>(type: "integer", nullable: false),
                    Losses = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    Goals_Scored = table.Column<int>(type: "integer", nullable: false),
                    Goals_Conceded = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament_Standings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournament_Standings_AspNetUsers_Player_ID",
                        column: x => x.Player_ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tournament_Standings_Teams_Team_ID",
                        column: x => x.Team_ID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tournament_Standings_Tournaments_Tournament_ID",
                        column: x => x.Tournament_ID,
                        principalTable: "Tournaments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Player1_ID = table.Column<string>(type: "text", nullable: true),
                    Player2_ID = table.Column<string>(type: "text", nullable: true),
                    Team1_ID = table.Column<int>(type: "integer", nullable: true),
                    Team2_ID = table.Column<int>(type: "integer", nullable: true),
                    Sport_ID = table.Column<int>(type: "integer", nullable: false),
                    Tournament_ID = table.Column<int>(type: "integer", nullable: false),
                    Stage_ID = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_AspNetUsers_Player1_ID",
                        column: x => x.Player1_ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Matches_AspNetUsers_Player2_ID",
                        column: x => x.Player2_ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Matches_Sports_Sport_ID",
                        column: x => x.Sport_ID,
                        principalTable: "Sports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Team1_ID",
                        column: x => x.Team1_ID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Team2_ID",
                        column: x => x.Team2_ID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Matches_Tournament_Stages_Stage_ID",
                        column: x => x.Stage_ID,
                        principalTable: "Tournament_Stages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_Tournament_ID",
                        column: x => x.Tournament_ID,
                        principalTable: "Tournaments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Play_Offs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Match_ID = table.Column<int>(type: "integer", nullable: false),
                    Next_Match_Winner_ID = table.Column<int>(type: "integer", nullable: true),
                    Next_Match_Loser_ID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Play_Offs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Play_Offs_Matches_Match_ID",
                        column: x => x.Match_ID,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Play_Offs_Matches_Next_Match_Loser_ID",
                        column: x => x.Next_Match_Loser_ID,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Play_Offs_Matches_Next_Match_Winner_ID",
                        column: x => x.Next_Match_Winner_ID,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Player_Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Match_ID = table.Column<int>(type: "integer", nullable: false),
                    Player_ID = table.Column<string>(type: "text", nullable: false),
                    Stats = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player_Statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Statistics_AspNetUsers_Player_ID",
                        column: x => x.Player_ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Player_Statistics_Matches_Match_ID",
                        column: x => x.Match_ID,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Team_Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Match_ID = table.Column<int>(type: "integer", nullable: false),
                    Team_ID = table.Column<int>(type: "integer", nullable: false),
                    Stats = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Statistics_Matches_Match_ID",
                        column: x => x.Match_ID,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Team_Statistics_Teams_Team_ID",
                        column: x => x.Team_ID,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Sport_ID",
                table: "AspNetUsers",
                column: "Sport_ID");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1_ID",
                table: "Matches",
                column: "Player1_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2_ID",
                table: "Matches",
                column: "Player2_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Sport_ID",
                table: "Matches",
                column: "Sport_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Stage_ID",
                table: "Matches",
                column: "Stage_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Team1_ID",
                table: "Matches",
                column: "Team1_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Team2_ID",
                table: "Matches",
                column: "Team2_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Tournament_ID",
                table: "Matches",
                column: "Tournament_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Play_Offs_Match_ID",
                table: "Play_Offs",
                column: "Match_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Play_Offs_Next_Match_Loser_ID",
                table: "Play_Offs",
                column: "Next_Match_Loser_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Play_Offs_Next_Match_Winner_ID",
                table: "Play_Offs",
                column: "Next_Match_Winner_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Statistics_Match_ID",
                table: "Player_Statistics",
                column: "Match_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Statistics_Player_ID",
                table: "Player_Statistics",
                column: "Player_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Sport_Statistics_Sport_ID",
                table: "Sport_Statistics",
                column: "Sport_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Entries_Team_ID",
                table: "Team_Entries",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Entries_User_ID",
                table: "Team_Entries",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Players_ID_Team",
                table: "Team_Players",
                column: "ID_Team");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Players_ID_User",
                table: "Team_Players",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Statistics_Match_ID",
                table: "Team_Statistics",
                column: "Match_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Statistics_Team_ID",
                table: "Team_Statistics",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Sport_ID",
                table: "Teams",
                column: "Sport_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Entries_ID_Team",
                table: "Tournament_Entries",
                column: "ID_Team");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Entries_ID_Tournament",
                table: "Tournament_Entries",
                column: "ID_Tournament");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Entries_ID_User",
                table: "Tournament_Entries",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Participants_Team_ID",
                table: "Tournament_Participants",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Participants_Tournament_ID",
                table: "Tournament_Participants",
                column: "Tournament_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Participants_User_ID",
                table: "Tournament_Participants",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Stages_Tournament_ID",
                table: "Tournament_Stages",
                column: "Tournament_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Standings_Player_ID",
                table: "Tournament_Standings",
                column: "Player_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Standings_Team_ID",
                table: "Tournament_Standings",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Standings_Tournament_ID",
                table: "Tournament_Standings",
                column: "Tournament_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_Format_ID",
                table: "Tournaments",
                column: "Format_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_Sport_ID",
                table: "Tournaments",
                column: "Sport_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Play_Offs");

            migrationBuilder.DropTable(
                name: "Player_Statistics");

            migrationBuilder.DropTable(
                name: "Sport_Statistics");

            migrationBuilder.DropTable(
                name: "Team_Entries");

            migrationBuilder.DropTable(
                name: "Team_Players");

            migrationBuilder.DropTable(
                name: "Team_Statistics");

            migrationBuilder.DropTable(
                name: "Tournament_Entries");

            migrationBuilder.DropTable(
                name: "Tournament_Participants");

            migrationBuilder.DropTable(
                name: "Tournament_Standings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Tournament_Stages");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropTable(
                name: "Tournament_Formats");
        }
    }
}
