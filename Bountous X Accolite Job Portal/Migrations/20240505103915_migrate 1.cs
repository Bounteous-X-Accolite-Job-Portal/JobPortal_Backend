using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class migrate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClosedJobApplications",
                columns: table => new
                {
                    ClosedJobApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AppliedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedJobApplications", x => x.ClosedJobApplicationId);
                    table.ForeignKey(
                        name: "FK_ClosedJobApplications_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId");
                    table.ForeignKey(
                        name: "FK_ClosedJobApplications_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId");
                    table.ForeignKey(
                        name: "FK_ClosedJobApplications_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedJobApplications_CandidateId",
                table: "ClosedJobApplications",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedJobApplications_JobId",
                table: "ClosedJobApplications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedJobApplications_StatusId",
                table: "ClosedJobApplications",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosedJobApplications");
        }
    }
}
