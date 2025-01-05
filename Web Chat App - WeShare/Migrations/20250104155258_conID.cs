using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Chat_App___WeShare.Migrations
{
    /// <inheritdoc />
    public partial class conID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "conID",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "conID",
                table: "Users");
        }
    }
}
