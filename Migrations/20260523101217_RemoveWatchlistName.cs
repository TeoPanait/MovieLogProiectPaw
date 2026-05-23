using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieLog.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWatchlistName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Watchlists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Watchlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
