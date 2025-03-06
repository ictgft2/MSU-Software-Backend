using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class AddedAdmissionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionTime",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsEmergency",
                table: "Patients");

            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    AdmissionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    AdmissionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDischarged = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.AdmissionId);
                    table.ForeignKey(
                        name: "FK_Admissions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientId",
                table: "Admissions",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admissions");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AdmissionTime",
                table: "Patients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsEmergency",
                table: "Patients",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
