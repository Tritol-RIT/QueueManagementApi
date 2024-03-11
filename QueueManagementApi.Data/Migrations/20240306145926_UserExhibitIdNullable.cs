using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueManagementApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserExhibitIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Exhibits_ExhibitId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ExhibitId",
                table: "Users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Exhibits_ExhibitId",
                table: "Users",
                column: "ExhibitId",
                principalTable: "Exhibits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Exhibits_ExhibitId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ExhibitId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Exhibits_ExhibitId",
                table: "Users",
                column: "ExhibitId",
                principalTable: "Exhibits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
