using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samadApp.Migrations
{
    /// <inheritdoc />
    public partial class updateSell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "College",
                table: "Sell",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "College",
                table: "Sell");
        }
    }
}
