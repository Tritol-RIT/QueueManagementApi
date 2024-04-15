using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueManagementApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WaitTimeCalculation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Visits",
                newName: "PotentialStartTime");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Visits",
                newName: "PotentialEndTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndTime",
                table: "Visits",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualStartTime",
                table: "Visits",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualEndTime",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "ActualStartTime",
                table: "Visits");

            migrationBuilder.RenameColumn(
                name: "PotentialStartTime",
                table: "Visits",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "PotentialEndTime",
                table: "Visits",
                newName: "EndTime");
        }
    }
}
