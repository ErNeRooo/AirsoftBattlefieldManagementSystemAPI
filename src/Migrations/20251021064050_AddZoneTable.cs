using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddZoneTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpawnZoneId",
                table: "Team",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapPing",
                columns: table => new
                {
                    MapPingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    BattleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapPing", x => x.MapPingId);
                    table.ForeignKey(
                        name: "FK_MapPing_Battle_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battle",
                        principalColumn: "BattleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapPing_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapPing_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    ZoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BattleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.ZoneId);
                    table.ForeignKey(
                        name: "FK_Zone_Battle_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battle",
                        principalColumn: "BattleId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ZoneVertex",
                columns: table => new
                {
                    ZoneVertexId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneVertex", x => x.ZoneVertexId);
                    table.ForeignKey(
                        name: "FK_ZoneVertex_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Team_SpawnZoneId",
                table: "Team",
                column: "SpawnZoneId",
                unique: true,
                filter: "[SpawnZoneId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MapPing_BattleId",
                table: "MapPing",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_MapPing_LocationId",
                table: "MapPing",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MapPing_PlayerId",
                table: "MapPing",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_BattleId",
                table: "Zone",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneVertex_ZoneId",
                table: "ZoneVertex",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Zone_SpawnZoneId",
                table: "Team",
                column: "SpawnZoneId",
                principalTable: "Zone",
                principalColumn: "ZoneId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Zone_SpawnZoneId",
                table: "Team");

            migrationBuilder.DropTable(
                name: "MapPing");

            migrationBuilder.DropTable(
                name: "ZoneVertex");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropIndex(
                name: "IX_Team_SpawnZoneId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "SpawnZoneId",
                table: "Team");
        }
    }
}
