using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewFeedbacks_Interviews_InterviewId",
                table: "InterviewFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_InterviewFeedbacks_InterviewId",
                table: "InterviewFeedbacks");

            migrationBuilder.DropColumn(
                name: "InterviewId",
                table: "InterviewFeedbacks");

            migrationBuilder.AddColumn<Guid>(
                name: "FeedbackId",
                table: "Interviews",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_FeedbackId",
                table: "Interviews",
                column: "FeedbackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_InterviewFeedbacks_FeedbackId",
                table: "Interviews",
                column: "FeedbackId",
                principalTable: "InterviewFeedbacks",
                principalColumn: "FeedbackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_InterviewFeedbacks_FeedbackId",
                table: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_FeedbackId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "Interviews");

            migrationBuilder.AddColumn<Guid>(
                name: "InterviewId",
                table: "InterviewFeedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterviewFeedbacks_InterviewId",
                table: "InterviewFeedbacks",
                column: "InterviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewFeedbacks_Interviews_InterviewId",
                table: "InterviewFeedbacks",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "InterviewId");
        }
    }
}
