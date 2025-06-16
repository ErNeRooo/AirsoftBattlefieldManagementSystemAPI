using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_PlayerLocation_Location_LocationId",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Player_PlayerId",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Room_RoomId",
                table: "PlayerLocation");

            migrationBuilder.DropIndex(
                name: "IX_Battle_RoomId",
                table: "Battle");

            migrationBuilder.DropIndex(
                name: "IX_Battle_RoomId1",
                table: "Battle");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Battle");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "PlayerLocation",
                newName: "BattleId1");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocation_RoomId",
                table: "PlayerLocation",
                newName: "IX_PlayerLocation_BattleId1");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Kill",
                newName: "BattleId1");

            migrationBuilder.RenameIndex(
                name: "IX_Kill_RoomId",
                table: "Kill",
                newName: "IX_Kill_BattleId1");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Death",
                newName: "BattleId1");

            migrationBuilder.RenameIndex(
                name: "IX_Death_RoomId",
                table: "Death",
                newName: "IX_Death_BattleId1");

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

            migrationBuilder.AddColumn<int>(
                name: "BattleId",
                table: "PlayerLocation",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<int>(
                name: "BattleId",
                table: "Kill",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<int>(
                name: "BattleId",
                table: "Death",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLocation_BattleId",
                table: "PlayerLocation",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Kill_BattleId",
                table: "Kill",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Death_BattleId",
                table: "Death",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Battle_RoomId",
                table: "Battle",
                column: "RoomId",
                unique: true,
                filter: "[RoomId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Battle_BattleId",
                table: "Death",
                column: "BattleId",
                principalTable: "Battle",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Battle_BattleId1",
                table: "Death",
                column: "BattleId1",
                principalTable: "Battle",
                principalColumn: "BattleId");

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
                name: "FK_Kill_Battle_BattleId",
                table: "Kill",
                column: "BattleId",
                principalTable: "Battle",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Battle_BattleId1",
                table: "Kill",
                column: "BattleId1",
                principalTable: "Battle",
                principalColumn: "BattleId");

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
                name: "FK_PlayerLocation_Battle_BattleId",
                table: "PlayerLocation",
                column: "BattleId",
                principalTable: "Battle",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Battle_BattleId1",
                table: "PlayerLocation",
                column: "BattleId1",
                principalTable: "Battle",
                principalColumn: "BattleId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Death_Battle_BattleId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Battle_BattleId1",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Location_LocationId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Death_Player_PlayerId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Battle_BattleId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Battle_BattleId1",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Location_LocationId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Player_PlayerId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Battle_BattleId",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Battle_BattleId1",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Location_LocationId",
                table: "PlayerLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Player_PlayerId",
                table: "PlayerLocation");

            migrationBuilder.DropIndex(
                name: "IX_PlayerLocation_BattleId",
                table: "PlayerLocation");

            migrationBuilder.DropIndex(
                name: "IX_Kill_BattleId",
                table: "Kill");

            migrationBuilder.DropIndex(
                name: "IX_Death_BattleId",
                table: "Death");

            migrationBuilder.DropIndex(
                name: "IX_Battle_RoomId",
                table: "Battle");

            migrationBuilder.DropColumn(
                name: "BattleId",
                table: "PlayerLocation");

            migrationBuilder.DropColumn(
                name: "BattleId",
                table: "Kill");

            migrationBuilder.DropColumn(
                name: "BattleId",
                table: "Death");

            migrationBuilder.RenameColumn(
                name: "BattleId1",
                table: "PlayerLocation",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerLocation_BattleId1",
                table: "PlayerLocation",
                newName: "IX_PlayerLocation_RoomId");

            migrationBuilder.RenameColumn(
                name: "BattleId1",
                table: "Kill",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Kill_BattleId1",
                table: "Kill",
                newName: "IX_Kill_RoomId");

            migrationBuilder.RenameColumn(
                name: "BattleId1",
                table: "Death",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Death_BattleId1",
                table: "Death",
                newName: "IX_Death_RoomId");

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
                name: "RoomId1",
                table: "Battle",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Battle_RoomId",
                table: "Battle",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Battle_RoomId1",
                table: "Battle",
                column: "RoomId1");

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
                name: "FK_Kill_Room_RoomId",
                table: "Kill",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
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
                name: "FK_PlayerLocation_Room_RoomId",
                table: "PlayerLocation",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
