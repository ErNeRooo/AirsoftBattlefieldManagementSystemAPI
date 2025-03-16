using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTeamTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfficerPlayerId",
                table: "Team",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Team_OfficerPlayerId",
                table: "Team",
                column: "OfficerPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Player_OfficerPlayerId",
                table: "Team",
                column: "OfficerPlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Player_OfficerPlayerId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_OfficerPlayerId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "OfficerPlayerId",
                table: "Team");
        }
    }
}
