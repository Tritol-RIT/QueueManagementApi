using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueManagementApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsuranceExhibitRemoveNavProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Exhibits_ExhibitId",
                table: "Insurances");

            migrationBuilder.DropIndex(
                name: "IX_Insurances_ExhibitId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "ExhibitId",
                table: "Insurances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExhibitId",
                table: "Insurances",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_ExhibitId",
                table: "Insurances",
                column: "ExhibitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Exhibits_ExhibitId",
                table: "Insurances",
                column: "ExhibitId",
                principalTable: "Exhibits",
                principalColumn: "Id");
        }
    }
}
