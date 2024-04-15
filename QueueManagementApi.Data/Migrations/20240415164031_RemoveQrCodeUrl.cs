using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueManagementApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQrCodeUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCodeUrl",
                table: "Visits");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCodeUrl",
                table: "Visits",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
