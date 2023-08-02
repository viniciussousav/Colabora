using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Colabora.Infrastructure.Persistence.Migrations
{
    public partial class InitialStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORGANIZATION",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Interests = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Verified = table.Column<bool>(type: "boolean", nullable: false)
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
                    VolunteerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Birthdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Interests = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
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
                    SocialActionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    VolunteerCreatorId = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Interests = table.Column<string>(type: "text", nullable: false),
                    OccurrenceDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
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
                    SocialActionId = table.Column<int>(type: "integer", nullable: false),
                    VolunteerId = table.Column<int>(type: "integer", nullable: false),
                    JoinedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
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
