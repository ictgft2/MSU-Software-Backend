using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class AddedWaitlistModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Waitlists",
                columns: table => new
                {
                    WaitlistId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateQueued = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waitlists", x => x.WaitlistId);
                    table.ForeignKey(
                        name: "FK_Waitlists_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Waitlists_PatientId",
                table: "Waitlists",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Waitlists");
        }
    }
}
