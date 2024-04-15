using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueManagementApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Insurances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Exhibits_ExhibitId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "VisitorPhotoIdRequired",
                table: "Insurances");

            migrationBuilder.AlterColumn<int>(
                name: "ExhibitId",
                table: "Insurances",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "VisitorImageUrl",
                table: "Insurances",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Exhibits_ExhibitId",
                table: "Insurances",
                column: "ExhibitId",
                principalTable: "Exhibits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Exhibits_ExhibitId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "VisitorImageUrl",
                table: "Insurances");

            migrationBuilder.AlterColumn<int>(
                name: "ExhibitId",
                table: "Insurances",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisitorPhotoIdRequired",
                table: "Insurances",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Exhibits_ExhibitId",
                table: "Insurances",
                column: "ExhibitId",
                principalTable: "Exhibits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
