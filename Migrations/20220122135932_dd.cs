using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTic.Migrations
{
    public partial class dd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gameDatas_RoomData_roomDataId",
                table: "gameDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gameDatas",
                table: "gameDatas");

            migrationBuilder.RenameTable(
                name: "gameDatas",
                newName: "GameData");

            migrationBuilder.RenameIndex(
                name: "IX_gameDatas_roomDataId",
                table: "GameData",
                newName: "IX_GameData_roomDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameData",
                table: "GameData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameData_RoomData_roomDataId",
                table: "GameData",
                column: "roomDataId",
                principalTable: "RoomData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameData_RoomData_roomDataId",
                table: "GameData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameData",
                table: "GameData");

            migrationBuilder.RenameTable(
                name: "GameData",
                newName: "gameDatas");

            migrationBuilder.RenameIndex(
                name: "IX_GameData_roomDataId",
                table: "gameDatas",
                newName: "IX_gameDatas_roomDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_gameDatas",
                table: "gameDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_gameDatas_RoomData_roomDataId",
                table: "gameDatas",
                column: "roomDataId",
                principalTable: "RoomData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
