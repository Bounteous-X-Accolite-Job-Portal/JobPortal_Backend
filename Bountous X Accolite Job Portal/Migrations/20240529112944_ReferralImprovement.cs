using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class ReferralImprovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "Referrals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClosedApplicationId",
                table: "Referrals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClosedJobApplicationId",
                table: "Referrals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClosedJobId",
                table: "Referrals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JobApplicationApplicationId",
                table: "Referrals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ClosedJobApplicationId",
                table: "Referrals",
                column: "ClosedJobApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ClosedJobId",
                table: "Referrals",
                column: "ClosedJobId");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_JobApplicationApplicationId",
                table: "Referrals",
                column: "JobApplicationApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Referrals_ClosedJobApplications_ClosedJobApplicationId",
                table: "Referrals",
                column: "ClosedJobApplicationId",
                principalTable: "ClosedJobApplications",
                principalColumn: "ClosedJobApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Referrals_ClosedJobs_ClosedJobId",
                table: "Referrals",
                column: "ClosedJobId",
                principalTable: "ClosedJobs",
                principalColumn: "ClosedJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Referrals_JobApplications_JobApplicationApplicationId",
                table: "Referrals",
                column: "JobApplicationApplicationId",
                principalTable: "JobApplications",
                principalColumn: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Referrals_ClosedJobApplications_ClosedJobApplicationId",
                table: "Referrals");

            migrationBuilder.DropForeignKey(
                name: "FK_Referrals_ClosedJobs_ClosedJobId",
                table: "Referrals");

            migrationBuilder.DropForeignKey(
                name: "FK_Referrals_JobApplications_JobApplicationApplicationId",
                table: "Referrals");

            migrationBuilder.DropIndex(
                name: "IX_Referrals_ClosedJobApplicationId",
                table: "Referrals");

            migrationBuilder.DropIndex(
                name: "IX_Referrals_ClosedJobId",
                table: "Referrals");

            migrationBuilder.DropIndex(
                name: "IX_Referrals_JobApplicationApplicationId",
                table: "Referrals");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Referrals");

            migrationBuilder.DropColumn(
                name: "ClosedApplicationId",
                table: "Referrals");

            migrationBuilder.DropColumn(
                name: "ClosedJobApplicationId",
                table: "Referrals");

            migrationBuilder.DropColumn(
                name: "ClosedJobId",
                table: "Referrals");

            migrationBuilder.DropColumn(
                name: "JobApplicationApplicationId",
                table: "Referrals");
        }
    }
}
