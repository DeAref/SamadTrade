using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samadApp.Migrations
{
    /// <inheritdoc />
    public partial class AddColleg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "College",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "College",
                table: "AspNetUsers");
        }
    }
}
