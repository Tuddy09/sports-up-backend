using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sports_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class updatedPlayerEntityRelationShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Users_OwnerId",
                table: "Lobbies");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Users_OwnerId",
                table: "Lobbies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Users_OwnerId",
                table: "Lobbies");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Users_OwnerId",
                table: "Lobbies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
