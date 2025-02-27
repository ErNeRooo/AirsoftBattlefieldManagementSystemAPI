using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class naming270220251647 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kills_Players_KilledPlayerId",
                table: "Kills");

            migrationBuilder.DropForeignKey(
                name: "FK_Kills_Players_KillerPlayerId",
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
                name: "FK_Teams_Battles_BattleId1",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "BattleId1",
                table: "Teams",
                newName: "BattleID1");

            migrationBuilder.RenameColumn(
                name: "BattleId",
                table: "Teams",
                newName: "BattleID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Teams",
                newName: "TeamID");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_BattleId1",
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
                name: "Id",
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
                name: "Id",
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
                name: "Id",
                table: "Locations",
                newName: "LocationID");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Kills",
                newName: "LocationID");

            migrationBuilder.RenameColumn(
                name: "KillerPlayerId",
                table: "Kills",
                newName: "KillerPlayerID");

            migrationBuilder.RenameColumn(
                name: "KilledPlayerId",
                table: "Kills",
                newName: "KilledPlayerID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Kills",
                newName: "KillID");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_KillerPlayerId",
                table: "Kills",
                newName: "IX_Kills_KillerPlayerID");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_KilledPlayerId",
                table: "Kills",
                newName: "IX_Kills_KilledPlayerID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Battles",
                newName: "BattleID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Accounts",
                newName: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Kills_LocationID",
                table: "Kills",
                column: "LocationID");

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
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Kills_Players_KillerPlayerID",
                table: "Kills",
                column: "KillerPlayerID",
                principalTable: "Players",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.NoAction);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_Kills_LocationID",
                table: "Kills");

            migrationBuilder.RenameColumn(
                name: "BattleID1",
                table: "Teams",
                newName: "BattleId1");

            migrationBuilder.RenameColumn(
                name: "BattleID",
                table: "Teams",
                newName: "BattleId");

            migrationBuilder.RenameColumn(
                name: "TeamID",
                table: "Teams",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_BattleID1",
                table: "Teams",
                newName: "IX_Teams_BattleId1");

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
                newName: "Id");

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
                newName: "Id");

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
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LocationID",
                table: "Kills",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "KillerPlayerID",
                table: "Kills",
                newName: "KillerPlayerId");

            migrationBuilder.RenameColumn(
                name: "KilledPlayerID",
                table: "Kills",
                newName: "KilledPlayerId");

            migrationBuilder.RenameColumn(
                name: "KillID",
                table: "Kills",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_KillerPlayerID",
                table: "Kills",
                newName: "IX_Kills_KillerPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_KilledPlayerID",
                table: "Kills",
                newName: "IX_Kills_KilledPlayerId");

            migrationBuilder.RenameColumn(
                name: "BattleID",
                table: "Battles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "Accounts",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kills_Players_KilledPlayerId",
                table: "Kills",
                column: "KilledPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kills_Players_KillerPlayerId",
                table: "Kills",
                column: "KillerPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocations_Locations_LocationId",
                table: "PlayerLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocations_Players_PlayerId",
                table: "PlayerLocations",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Accounts_AccountId",
                table: "Players",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Battles_BattleId1",
                table: "Teams",
                column: "BattleId1",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
