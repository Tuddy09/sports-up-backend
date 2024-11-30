using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sports_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class StatusLobbyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Lobbies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Lobbies");
        }
    }
}
