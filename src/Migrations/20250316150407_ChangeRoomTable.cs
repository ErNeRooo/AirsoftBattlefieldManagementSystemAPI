using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoinRoomNumber",
                table: "Room",
                newName: "AdminPlayerId");

            migrationBuilder.AddColumn<string>(
                name: "JoinCode",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Room_AdminPlayerId",
                table: "Room",
                column: "AdminPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Player_AdminPlayerId",
                table: "Room",
                column: "AdminPlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Player_AdminPlayerId",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Room_AdminPlayerId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "JoinCode",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Room");

            migrationBuilder.RenameColumn(
                name: "AdminPlayerId",
                table: "Room",
                newName: "JoinRoomNumber");
        }
    }
}
