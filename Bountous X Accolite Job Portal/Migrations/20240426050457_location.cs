using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bountous_X_Accolite_Job_Portal.Migrations
{
    /// <inheritdoc />
    public partial class location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Designations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Designations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Designations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Designations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Designations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Designations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Designations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
