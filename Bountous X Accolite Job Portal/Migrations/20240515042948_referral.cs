using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class referral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AutoPassword",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoPassword",
                table: "AspNetUsers");
        }
    }
}
