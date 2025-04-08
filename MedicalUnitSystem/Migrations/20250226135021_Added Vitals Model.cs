using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class AddedVitalsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vitals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    BloodPressure = table.Column<string>(type: "text", nullable: true),
                    DateOfVisit = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PatientId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vitals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vitals_Patients_PatientId1",
                        column: x => x.PatientId1,
                        principalTable: "Patients",
                        principalColumn: "PatientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_PatientId1",
                table: "Vitals",
                column: "PatientId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vitals");
        }
    }
}
