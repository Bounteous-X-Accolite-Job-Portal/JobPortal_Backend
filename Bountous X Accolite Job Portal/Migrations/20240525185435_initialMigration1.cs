using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobId",
                table: "SuccessfulJobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuccessfulJobs_JobId",
                table: "SuccessfulJobs",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuccessfulJobs_Jobs_JobId",
                table: "SuccessfulJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuccessfulJobs_Jobs_JobId",
                table: "SuccessfulJobs");

            migrationBuilder.DropIndex(
                name: "IX_SuccessfulJobs_JobId",
                table: "SuccessfulJobs");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "SuccessfulJobs");
        }
    }
}
