using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayersFieldToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId1",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_RoomId1",
                table: "Player",
                column: "RoomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Room_RoomId1",
                table: "Player",
                column: "RoomId1",
                principalTable: "Room",
                principalColumn: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Room_RoomId1",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_RoomId1",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Player");
        }
    }
}
