using Microsoft.EntityFrameworkCore.Migrations;

namespace SmollApi.Migrations
{
    public partial class FifthMi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Bans",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Users",
                type: "nvarchar(50)",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Bans");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");
        }
    }
}
