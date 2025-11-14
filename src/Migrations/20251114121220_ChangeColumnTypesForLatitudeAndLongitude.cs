using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirsoftBattlefieldManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnTypesForLatitudeAndLongitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "ZoneVertex",
                type: "decimal(8,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "ZoneVertex",
                type: "decimal(7,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Location",
                type: "decimal(8,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Location",
                type: "decimal(7,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "ZoneVertex",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "ZoneVertex",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Location",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Location",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,5)");
        }
    }
}
