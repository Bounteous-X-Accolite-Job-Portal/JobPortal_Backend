using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class jobapp3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Candidates_CandidateId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_CandidateId",
                table: "JobApplications");

            migrationBuilder.AlterColumn<Guid>(
                name: "CandidateId",
                table: "JobApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_CandidateId",
                table: "JobApplications",
                column: "CandidateId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Candidates_CandidateId",
                table: "JobApplications",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "CandidateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Candidates_CandidateId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_CandidateId",
                table: "JobApplications");

            migrationBuilder.AlterColumn<Guid>(
                name: "CandidateId",
                table: "JobApplications",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_CandidateId",
                table: "JobApplications",
                column: "CandidateId",
                unique: true,
                filter: "[CandidateId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Candidates_CandidateId",
                table: "JobApplications",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "CandidateId");
        }
    }
}
