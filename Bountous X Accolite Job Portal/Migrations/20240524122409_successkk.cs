using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class successkk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_JobApplication_JobApplicationApplicationId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplication_Candidates_CandidateId",
                table: "JobApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplication_ClosedJobs_ClosedJobId",
                table: "JobApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplication_Jobs_JobId",
                table: "JobApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplication_Status_StatusId",
                table: "JobApplication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplication",
                table: "JobApplication");

            migrationBuilder.RenameTable(
                name: "JobApplication",
                newName: "JobApplications");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplication_StatusId",
                table: "JobApplications",
                newName: "IX_JobApplications_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplication_JobId",
                table: "JobApplications",
                newName: "IX_JobApplications_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplication_ClosedJobId",
                table: "JobApplications",
                newName: "IX_JobApplications_ClosedJobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplication_CandidateId",
                table: "JobApplications",
                newName: "IX_JobApplications_CandidateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                column: "ApplicationId");

            migrationBuilder.CreateTable(
                name: "SuccessfulJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JobApplicationApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuccessfulJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuccessfulJobs_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId");
                    table.ForeignKey(
                        name: "FK_SuccessfulJobs_JobApplications_JobApplicationApplicationId",
                        column: x => x.JobApplicationApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "ApplicationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuccessfulJobs_CandidateId",
                table: "SuccessfulJobs",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_SuccessfulJobs_JobApplicationApplicationId",
                table: "SuccessfulJobs",
                column: "JobApplicationApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_JobApplications_JobApplicationApplicationId",
                table: "Interviews",
                column: "JobApplicationApplicationId",
                principalTable: "JobApplications",
                principalColumn: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Candidates_CandidateId",
                table: "JobApplications",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_ClosedJobs_ClosedJobId",
                table: "JobApplications",
                column: "ClosedJobId",
                principalTable: "ClosedJobs",
                principalColumn: "ClosedJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Jobs_JobId",
                table: "JobApplications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Status_StatusId",
                table: "JobApplications",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_JobApplications_JobApplicationApplicationId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Candidates_CandidateId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_ClosedJobs_ClosedJobId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Jobs_JobId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Status_StatusId",
                table: "JobApplications");

            migrationBuilder.DropTable(
                name: "SuccessfulJobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.RenameTable(
                name: "JobApplications",
                newName: "JobApplication");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_StatusId",
                table: "JobApplication",
                newName: "IX_JobApplication_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_JobId",
                table: "JobApplication",
                newName: "IX_JobApplication_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_ClosedJobId",
                table: "JobApplication",
                newName: "IX_JobApplication_ClosedJobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_CandidateId",
                table: "JobApplication",
                newName: "IX_JobApplication_CandidateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplication",
                table: "JobApplication",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_JobApplication_JobApplicationApplicationId",
                table: "Interviews",
                column: "JobApplicationApplicationId",
                principalTable: "JobApplication",
                principalColumn: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplication_Candidates_CandidateId",
                table: "JobApplication",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplication_ClosedJobs_ClosedJobId",
                table: "JobApplication",
                column: "ClosedJobId",
                principalTable: "ClosedJobs",
                principalColumn: "ClosedJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplication_Jobs_JobId",
                table: "JobApplication",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplication_Status_StatusId",
                table: "JobApplication",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId");
        }
    }
}
