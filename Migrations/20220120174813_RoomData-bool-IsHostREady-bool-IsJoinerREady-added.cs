using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTic.Migrations
{
    public partial class RoomDataboolIsHostREadyboolIsJoinerREadyadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHostREady",
                table: "RoomData",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsJoinerREady",
                table: "RoomData",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHostREady",
                table: "RoomData");

            migrationBuilder.DropColumn(
                name: "IsJoinerREady",
                table: "RoomData");
        }
    }
}
