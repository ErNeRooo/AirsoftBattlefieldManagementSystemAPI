using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateDeathTableAndRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kills_Locations_LocationID",
                table: "Kills");

            migrationBuilder.DropForeignKey(
                name: "FK_Kills_Players_KilledPlayerID",
                table: "Kills");

            migrationBuilder.DropForeignKey(
                name: "FK_Kills_Players_KillerPlayerID",
                table: "Kills");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocations_Locations_LocationID",
                table: "PlayerLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocations_Players_PlayerID",
                table: "PlayerLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Accounts_AccountID",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamID",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Battles_BattleID1",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Kills_KilledPlayerID",
                table: "Kills");

            migrationBuilder.DropColumn(
                name: "BattleID",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "PlayerLocations");

            migrationBuilder.DropColumn(
                name: "Datetime",
                table: "Kills");

            migrationBuilder.DropColumn(
                name: "KilledPlayerID",
                table: "Kills");

            migrationBuilder.RenameColumn(
                name: "TeamID",
                table: "Teams",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "BattleID1",
                table: "Teams",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_BattleID1",
                table: "Teams",
                newName: "IX_Teams_RoomId");

            migrationBuilder.RenameColumn(
                name: "TeamID",
                table: "Players",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Players",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "PlayerID",
                table: "Players",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_TeamID",
                table: "Players",
                newName: "IX_Players_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_AccountID",
                table: "Players",
                newName: "IX_Players_AccountId");

            migrationBuilder.RenameColumn(
                name: "PlayerID",
                table: "PlayerLocations",
                newName: "PlayerId");

            migrationBuilder.RenameColumn(
                name: "LocationID",
                table: "PlayerLocations",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "PlayerLocationID",
                table: "PlayerLocations",
                newName: "PlayerLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocations_PlayerID",
                table: "PlayerLocations",
                newName: "IX_PlayerLocations_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocations_LocationID",
                table: "PlayerLocations",
                newName: "IX_PlayerLocations_LocationId");

            migrationBuilder.RenameColumn(
                name: "LocationID",
                table: "Locations",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "LocationID",
                table: "Kills",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "KillID",
                table: "Kills",
                newName: "KillId");

            migrationBuilder.RenameColumn(
                name: "KillerPlayerID",
                table: "Kills",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_LocationID",
                table: "Kills",
                newName: "IX_Kills_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_KillerPlayerID",
                table: "Kills",
                newName: "IX_Kills_PlayerId");

            migrationBuilder.RenameColumn(
                name: "BattleID",
                table: "Battles",
                newName: "BattleId");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Battles",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Accounts",
                newName: "AccountId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "Locations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Death",
                columns: table => new
                {
                    DeathId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Death", x => x.DeathId);
                    table.ForeignKey(
                        name: "FK_Death_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Death_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Battles_RoomId",
                table: "Battles",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Death_LocationId",
                table: "Death",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Death_PlayerId",
                table: "Death",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_Room_RoomId",
                table: "Battles",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kills_Locations_LocationId",
                table: "Kills",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kills_Players_PlayerId",
                table: "Kills",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocations_Locations_LocationId",
                table: "PlayerLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocations_Players_PlayerId",
                table: "PlayerLocations",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Accounts_AccountId",
                table: "Players",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Room_RoomId",
                table: "Teams",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battles_Room_RoomId",
                table: "Battles");

            migrationBuilder.DropForeignKey(
                name: "FK_Kills_Locations_LocationId",
                table: "Kills");

            migrationBuilder.DropForeignKey(
                name: "FK_Kills_Players_PlayerId",
                table: "Kills");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocations_Locations_LocationId",
                table: "PlayerLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocations_Players_PlayerId",
                table: "PlayerLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Accounts_AccountId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Room_RoomId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Death");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Battles_RoomId",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Teams",
                newName: "TeamID");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Teams",
                newName: "BattleID1");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_RoomId",
                table: "Teams",
                newName: "IX_Teams_BattleID1");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Players",
                newName: "TeamID");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Players",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Players",
                newName: "PlayerID");

            migrationBuilder.RenameIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                newName: "IX_Players_TeamID");

            migrationBuilder.RenameIndex(
                name: "IX_Players_AccountId",
                table: "Players",
                newName: "IX_Players_AccountID");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "PlayerLocations",
                newName: "PlayerID");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "PlayerLocations",
                newName: "LocationID");

            migrationBuilder.RenameColumn(
                name: "PlayerLocationId",
                table: "PlayerLocations",
                newName: "PlayerLocationID");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocations_PlayerId",
                table: "PlayerLocations",
                newName: "IX_PlayerLocations_PlayerID");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocations_LocationId",
                table: "PlayerLocations",
                newName: "IX_PlayerLocations_LocationID");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Locations",
                newName: "LocationID");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Kills",
                newName: "LocationID");

            migrationBuilder.RenameColumn(
                name: "KillId",
                table: "Kills",
                newName: "KillID");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Kills",
                newName: "KillerPlayerID");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_LocationId",
                table: "Kills",
                newName: "IX_Kills_LocationID");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_PlayerId",
                table: "Kills",
                newName: "IX_Kills_KillerPlayerID");

            migrationBuilder.RenameColumn(
                name: "BattleId",
                table: "Battles",
                newName: "BattleID");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Battles",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Accounts",
                newName: "AccountID");

            migrationBuilder.AddColumn<int>(
                name: "BattleID",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "PlayerLocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Datetime",
                table: "Kills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "KilledPlayerID",
                table: "Kills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Kills_KilledPlayerID",
                table: "Kills",
                column: "KilledPlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Kills_Locations_LocationID",
                table: "Kills",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kills_Players_KilledPlayerID",
                table: "Kills",
                column: "KilledPlayerID",
                principalTable: "Players",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kills_Players_KillerPlayerID",
                table: "Kills",
                column: "KillerPlayerID",
                principalTable: "Players",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocations_Locations_LocationID",
                table: "PlayerLocations",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocations_Players_PlayerID",
                table: "PlayerLocations",
                column: "PlayerID",
                principalTable: "Players",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Accounts_AccountID",
                table: "Players",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamID",
                table: "Players",
                column: "TeamID",
                principalTable: "Teams",
                principalColumn: "TeamID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Battles_BattleID1",
                table: "Teams",
                column: "BattleID1",
                principalTable: "Battles",
                principalColumn: "BattleID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
