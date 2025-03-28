using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomIdFieldInTablesDeathKillAndPlayerLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "PlayerLocation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Kill",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Death",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLocation_RoomId",
                table: "PlayerLocation",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Kill_RoomId",
                table: "Kill",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Death_RoomId",
                table: "Death",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Death_Room_RoomId",
                table: "Death",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kill_Room_RoomId",
                table: "Kill",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerLocation_Room_RoomId",
                table: "PlayerLocation",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Death_Room_RoomId",
                table: "Death");

            migrationBuilder.DropForeignKey(
                name: "FK_Kill_Room_RoomId",
                table: "Kill");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerLocation_Room_RoomId",
                table: "PlayerLocation");

            migrationBuilder.DropIndex(
                name: "IX_PlayerLocation_RoomId",
                table: "PlayerLocation");

            migrationBuilder.DropIndex(
                name: "IX_Kill_RoomId",
                table: "Kill");

            migrationBuilder.DropIndex(
                name: "IX_Death_RoomId",
                table: "Death");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "PlayerLocation");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Kill");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Death");
        }
    }
}
