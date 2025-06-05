using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAllDeleteBehavioursToSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_Death_Room_RoomId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Location_LocationId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Player_PlayerId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Room_RoomId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Room_RoomId",
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
                name: "FK_PlayerLocation_Room_RoomId",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Player_AdminPlayerId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Player_OfficerPlayerId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Room_RoomId",
                table: "Team");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Team",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OfficerPlayerId",
                table: "Team",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RoomId1",
                table: "Team",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminPlayerId",
                table: "Room",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "PlayerLocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "PlayerLocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "PlayerLocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId1",
                table: "PlayerLocation",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Kill",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Kill",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Kill",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId1",
                table: "Kill",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Death",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Death",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Death",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId1",
                table: "Death",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Battle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RoomId1",
                table: "Battle",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_RoomId1",
                table: "Team",
                column: "RoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLocation_PlayerId1",
                table: "PlayerLocation",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Kill_PlayerId1",
                table: "Kill",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Death_PlayerId1",
                table: "Death",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Battle_RoomId1",
                table: "Battle",
                column: "RoomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Battle_Room_RoomId",
                table: "Battle",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Battle_Room_RoomId1",
                table: "Battle",
                column: "RoomId1",
                principalTable: "Room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Location_LocationId",
                table: "Death",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Player_PlayerId",
                table: "Death",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Player_PlayerId1",
                table: "Death",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Room_RoomId",
                table: "Death",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Location_LocationId",
                table: "Kill",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Player_PlayerId",
                table: "Kill",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Player_PlayerId1",
                table: "Kill",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Room_RoomId",
                table: "Kill",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Room_RoomId",
                table: "Player",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Team_TeamId",
                table: "Player",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Location_LocationId",
                table: "PlayerLocation",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Player_PlayerId",
                table: "PlayerLocation",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Player_PlayerId1",
                table: "PlayerLocation",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Room_RoomId",
                table: "PlayerLocation",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Player_AdminPlayerId",
                table: "Room",
                column: "AdminPlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Player_OfficerPlayerId",
                table: "Team",
                column: "OfficerPlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Room_RoomId",
                table: "Team",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Room_RoomId1",
                table: "Team",
                column: "RoomId1",
                principalTable: "Room",
                principalColumn: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battle_Room_RoomId",
                table: "Battle");

            migrationBuilder.DropForeignKey(
                name: "FK_Battle_Room_RoomId1",
                table: "Battle");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Location_LocationId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Player_PlayerId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Player_PlayerId1",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Room_RoomId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Location_LocationId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Player_PlayerId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Player_PlayerId1",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Room_RoomId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Room_RoomId",
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
                name: "FK_PlayerLocation_Player_PlayerId1",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Room_RoomId",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Player_AdminPlayerId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Player_OfficerPlayerId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Room_RoomId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Room_RoomId1",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_RoomId1",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_PlayerLocation_PlayerId1",
                table: "PlayerLocation");

            migrationBuilder.DropIndex(
                name: "IX_Kill_PlayerId1",
                table: "Kill");

            migrationBuilder.DropIndex(
                name: "IX_Death_PlayerId1",
                table: "Death");

            migrationBuilder.DropIndex(
                name: "IX_Battle_RoomId1",
                table: "Battle");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "PlayerLocation");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Kill");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Death");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Battle");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Team",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OfficerPlayerId",
                table: "Team",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminPlayerId",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "PlayerLocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "PlayerLocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "PlayerLocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Kill",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Kill",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Kill",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Death",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Death",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Death",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Battle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Battle_Room_RoomId",
                table: "Battle",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
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
                name: "FK_Death_Room_RoomId",
                table: "Death",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");

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
                name: "FK_Kill_Room_RoomId",
                table: "Kill",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Room_RoomId",
                table: "Player",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Team_TeamId",
                table: "Player",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId");

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
                name: "FK_PlayerLocation_Room_RoomId",
                table: "PlayerLocation",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Player_AdminPlayerId",
                table: "Room",
                column: "AdminPlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Player_OfficerPlayerId",
                table: "Team",
                column: "OfficerPlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Room_RoomId",
                table: "Team",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
