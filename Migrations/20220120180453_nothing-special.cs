using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTic.Migrations
{
    public partial class nothingspecial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsJoinerREady",
                table: "RoomData",
                newName: "IsJoinerReady");

            migrationBuilder.RenameColumn(
                name: "IsHostREady",
                table: "RoomData",
                newName: "IsHostReady");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsJoinerReady",
                table: "RoomData",
                newName: "IsJoinerREady");

            migrationBuilder.RenameColumn(
                name: "IsHostReady",
                table: "RoomData",
                newName: "IsHostREady");
        }
    }
}
