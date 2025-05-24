using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationBetweenAccountAndPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Account_AccountId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_AccountId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_PlayerId",
                table: "Account",
                column: "PlayerId",
                unique: true,
                filter: "[PlayerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Player_PlayerId",
                table: "Account",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Player_PlayerId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_PlayerId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_AccountId",
                table: "Player",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Account_AccountId",
                table: "Player",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }
    }
}
