using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageStory.Migrations
{
    /// <inheritdoc />
    public partial class AddField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalaryAmount",
                table: "Internships",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Internships",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryAmount",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Internships");
        }
    }
}
