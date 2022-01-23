using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTic.Migrations
{
    public partial class addedgameDatatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gameDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roomDataId = table.Column<int>(type: "int", nullable: false),
                    cellCode = table.Column<int>(type: "int", nullable: false),
                    cellFiller = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gameDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_gameDatas_RoomData_roomDataId",
                        column: x => x.roomDataId,
                        principalTable: "RoomData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gameDatas_roomDataId",
                table: "gameDatas",
                column: "roomDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gameDatas");
        }
    }
}
