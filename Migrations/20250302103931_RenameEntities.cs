using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battles_Room_RoomId",
                table: "Battles");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Locations_LocationId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Players_PlayerId",
                table: "Death");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerLocations",
                table: "PlayerLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kills",
                table: "Kills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Battles",
                table: "Battles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Player");

            migrationBuilder.RenameTable(
                name: "PlayerLocations",
                newName: "PlayerLocation");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.RenameTable(
                name: "Kills",
                newName: "Kill");

            migrationBuilder.RenameTable(
                name: "Battles",
                newName: "Battle");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_RoomId",
                table: "Team",
                newName: "IX_Team_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_TeamId",
                table: "Player",
                newName: "IX_Player_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_AccountId",
                table: "Player",
                newName: "IX_Player_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocations_PlayerId",
                table: "PlayerLocation",
                newName: "IX_PlayerLocation_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocations_LocationId",
                table: "PlayerLocation",
                newName: "IX_PlayerLocation_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_PlayerId",
                table: "Kill",
                newName: "IX_Kill_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Kills_LocationId",
                table: "Kill",
                newName: "IX_Kill_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Battles_RoomId",
                table: "Battle",
                newName: "IX_Battle_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                table: "Player",
                column: "PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerLocation",
                table: "PlayerLocation",
                column: "PlayerLocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kill",
                table: "Kill",
                column: "KillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Battle",
                table: "Battle",
                column: "BattleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Battle_Room_RoomId",
                table: "Battle",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Location_LocationId",
                table: "Death",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Player_PlayerId",
                table: "Death",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Location_LocationId",
                table: "Kill",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Player_PlayerId",
                table: "Kill",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Account_AccountId",
                table: "Player",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Team_TeamId",
                table: "Player",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Location_LocationId",
                table: "PlayerLocation",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Player_PlayerId",
                table: "PlayerLocation",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Room_RoomId",
                table: "Team",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battle_Room_RoomId",
                table: "Battle");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Location_LocationId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Player_PlayerId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Location_LocationId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Player_PlayerId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Account_AccountId",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Team_TeamId",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Location_LocationId",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Player_PlayerId",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Room_RoomId",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerLocation",
                table: "PlayerLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kill",
                table: "Kill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Battle",
                table: "Battle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "PlayerLocation",
                newName: "PlayerLocations");

            migrationBuilder.RenameTable(
                name: "Player",
                newName: "Players");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.RenameTable(
                name: "Kill",
                newName: "Kills");

            migrationBuilder.RenameTable(
                name: "Battle",
                newName: "Battles");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Team_RoomId",
                table: "Teams",
                newName: "IX_Teams_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocation_PlayerId",
                table: "PlayerLocations",
                newName: "IX_PlayerLocations_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocation_LocationId",
                table: "PlayerLocations",
                newName: "IX_PlayerLocations_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_TeamId",
                table: "Players",
                newName: "IX_Players_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AccountId",
                table: "Players",
                newName: "IX_Players_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Kill_PlayerId",
                table: "Kills",
                newName: "IX_Kills_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Kill_LocationId",
                table: "Kills",
                newName: "IX_Kills_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Battle_RoomId",
                table: "Battles",
                newName: "IX_Battles_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerLocations",
                table: "PlayerLocations",
                column: "PlayerLocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kills",
                table: "Kills",
                column: "KillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Battles",
                table: "Battles",
                column: "BattleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_Room_RoomId",
                table: "Battles",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Locations_LocationId",
                table: "Death",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Players_PlayerId",
                table: "Death",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
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
                principalColumn: "AccountId");

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
    }
}
