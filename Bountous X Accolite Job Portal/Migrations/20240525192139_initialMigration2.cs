using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClosedJobId",
                table: "SuccessfulJobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuccessfulJobs_ClosedJobId",
                table: "SuccessfulJobs",
                column: "ClosedJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuccessfulJobs_ClosedJobs_ClosedJobId",
                table: "SuccessfulJobs",
                column: "ClosedJobId",
                principalTable: "ClosedJobs",
                principalColumn: "ClosedJobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuccessfulJobs_ClosedJobs_ClosedJobId",
                table: "SuccessfulJobs");

            migrationBuilder.DropIndex(
                name: "IX_SuccessfulJobs_ClosedJobId",
                table: "SuccessfulJobs");

            migrationBuilder.DropColumn(
                name: "ClosedJobId",
                table: "SuccessfulJobs");
        }
    }
}
