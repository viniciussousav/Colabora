#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Colabora.Infrastructure.Persistence.Migrations
{
    public partial class SocialActionsTableStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SOCIAL_ACTIONS",
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
                    table.PrimaryKey("PK_SOCIAL_ACTIONS", x => x.SocialActionId);
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

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_ACTIONS_OrganizationId",
                table: "SOCIAL_ACTIONS",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_ACTIONS_VolunteerCreatorId",
                table: "SOCIAL_ACTIONS",
                column: "VolunteerCreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOCIAL_ACTIONS");
        }
    }
}
