using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Death_Battle_BattleId1",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Player_PlayerId1",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Battle_BattleId1",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Player_PlayerId1",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Room_RoomId1",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Battle_BattleId1",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Player_PlayerId1",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Room_RoomId1",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_RoomId1",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Room_AdminPlayerId",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_PlayerLocation_BattleId1",
                table: "PlayerLocation");

            migrationBuilder.DropIndex(
                name: "IX_PlayerLocation_PlayerId1",
                table: "PlayerLocation");

            migrationBuilder.DropIndex(
                name: "IX_Player_RoomId1",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Kill_BattleId1",
                table: "Kill");

            migrationBuilder.DropIndex(
                name: "IX_Kill_PlayerId1",
                table: "Kill");

            migrationBuilder.DropIndex(
                name: "IX_Death_BattleId1",
                table: "Death");

            migrationBuilder.DropIndex(
                name: "IX_Death_PlayerId1",
                table: "Death");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "BattleId1",
                table: "PlayerLocation");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "PlayerLocation");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "BattleId1",
                table: "Kill");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Kill");

            migrationBuilder.DropColumn(
                name: "BattleId1",
                table: "Death");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Death");

            migrationBuilder.CreateIndex(
                name: "IX_Room_AdminPlayerId",
                table: "Room",
                column: "AdminPlayerId",
                unique: true,
                filter: "[AdminPlayerId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Room_AdminPlayerId",
                table: "Room");

            migrationBuilder.AddColumn<int>(
                name: "RoomId1",
                table: "Team",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BattleId1",
                table: "PlayerLocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId1",
                table: "PlayerLocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId1",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BattleId1",
                table: "Kill",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId1",
                table: "Kill",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BattleId1",
                table: "Death",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId1",
                table: "Death",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_RoomId1",
                table: "Team",
                column: "RoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_Room_AdminPlayerId",
                table: "Room",
                column: "AdminPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLocation_BattleId1",
                table: "PlayerLocation",
                column: "BattleId1");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLocation_PlayerId1",
                table: "PlayerLocation",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Player_RoomId1",
                table: "Player",
                column: "RoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_Kill_BattleId1",
                table: "Kill",
                column: "BattleId1");

            migrationBuilder.CreateIndex(
                name: "IX_Kill_PlayerId1",
                table: "Kill",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Death_BattleId1",
                table: "Death",
                column: "BattleId1");

            migrationBuilder.CreateIndex(
                name: "IX_Death_PlayerId1",
                table: "Death",
                column: "PlayerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Battle_BattleId1",
                table: "Death",
                column: "BattleId1",
                principalTable: "Battle",
                principalColumn: "BattleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Player_PlayerId1",
                table: "Death",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Battle_BattleId1",
                table: "Kill",
                column: "BattleId1",
                principalTable: "Battle",
                principalColumn: "BattleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Player_PlayerId1",
                table: "Kill",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Room_RoomId1",
                table: "Player",
                column: "RoomId1",
                principalTable: "Room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Battle_BattleId1",
                table: "PlayerLocation",
                column: "BattleId1",
                principalTable: "Battle",
                principalColumn: "BattleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Player_PlayerId1",
                table: "PlayerLocation",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Room_RoomId1",
                table: "Team",
                column: "RoomId1",
                principalTable: "Room",
                principalColumn: "RoomId");
        }
    }
}
