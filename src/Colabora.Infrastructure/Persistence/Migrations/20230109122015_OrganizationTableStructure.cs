#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Colabora.Infrastructure.Persistence.Migrations
{
    public partial class OrganizationTableStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ORGANIZATION",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ORGANIZATION",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ORGANIZATION",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "ORGANIZATION",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ORGANIZATION",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "ORGANIZATION",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ORGANIZATION_Email",
                table: "ORGANIZATION",
                column: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ORGANIZATION_Email",
                table: "ORGANIZATION");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ORGANIZATION");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ORGANIZATION");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ORGANIZATION");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "ORGANIZATION");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ORGANIZATION");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ORGANIZATION");
        }
    }
}
