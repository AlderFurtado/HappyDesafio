using Microsoft.EntityFrameworkCore.Migrations;

namespace HyppeDesafio.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserCreatorId",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserCreatorId",
                table: "Events",
                column: "UserCreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserCreatorId",
                table: "Events",
                column: "UserCreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserCreatorId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserCreatorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UserCreatorId",
                table: "Events");
        }
    }
}
