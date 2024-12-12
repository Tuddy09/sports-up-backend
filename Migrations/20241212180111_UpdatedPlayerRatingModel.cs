using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sports_up_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPlayerRatingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "PlayerRatings",
                newName: "Message");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "PlayerRatings",
                newName: "Category");
        }
    }
}
