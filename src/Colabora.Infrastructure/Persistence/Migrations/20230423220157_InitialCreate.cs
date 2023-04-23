using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Colabora.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORGANIZATION",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORGANIZATION", x => x.OrganizationId);
                    table.UniqueConstraint("AK_ORGANIZATION_Email", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "VOLUNTEER",
                columns: table => new
                {
                    VolunteerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Birthdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOLUNTEER", x => x.VolunteerId);
                    table.UniqueConstraint("AK_VOLUNTEER_Email", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "SOCIAL_ACTION",
                columns: table => new
                {
                    SocialActionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    VolunteerCreatorId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurrenceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOCIAL_ACTION", x => x.SocialActionId);
                    table.ForeignKey(
                        name: "FK_ORGANIZATION",
                        column: x => x.OrganizationId,
                        principalTable: "ORGANIZATION",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VOLUNTEER",
                        column: x => x.VolunteerCreatorId,
                        principalTable: "VOLUNTEER",
                        principalColumn: "VolunteerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PARTICIPATION",
                columns: table => new
                {
                    SocialActionId = table.Column<int>(type: "int", nullable: false),
                    VolunteerId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARTICIPATION", x => new { x.SocialActionId, x.VolunteerId });
                    table.ForeignKey(
                        name: "FK_PARTICIPATION_SOCIAL_ACTION_SocialActionId",
                        column: x => x.SocialActionId,
                        principalTable: "SOCIAL_ACTION",
                        principalColumn: "SocialActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PARTICIPATION_VOLUNTEER_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "VOLUNTEER",
                        principalColumn: "VolunteerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PARTICIPATION_VolunteerId",
                table: "PARTICIPATION",
                column: "VolunteerId");

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_ACTION_OrganizationId",
                table: "SOCIAL_ACTION",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_ACTION_VolunteerCreatorId",
                table: "SOCIAL_ACTION",
                column: "VolunteerCreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PARTICIPATION");

            migrationBuilder.DropTable(
                name: "SOCIAL_ACTION");

            migrationBuilder.DropTable(
                name: "ORGANIZATION");

            migrationBuilder.DropTable(
                name: "VOLUNTEER");
        }
    }
}
