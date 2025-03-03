using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class AddedPrescriptionandDoctormodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodPressure",
                table: "Consultations");

            migrationBuilder.RenameColumn(
                name: "WaitPatientId",
                table: "WaitingPatients",
                newName: "WaitingPatientId");

            migrationBuilder.RenameColumn(
                name: "Prescriptions",
                table: "Consultations",
                newName: "Symptoms");

            migrationBuilder.RenameColumn(
                name: "LaboratoryTests",
                table: "Consultations",
                newName: "Notes");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsultationDate",
                table: "Consultations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Consultations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowupDate",
                table: "Consultations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    ContactInfo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.DoctorId);
                });

            migrationBuilder.CreateTable(
                name: "Prescription",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConsultationId = table.Column<int>(type: "integer", nullable: true),
                    MedicationName = table.Column<string>(type: "text", nullable: true),
                    Dosage = table.Column<string>(type: "text", nullable: true),
                    Frequency = table.Column<string>(type: "text", nullable: true),
                    Instructions = table.Column<string>(type: "text", nullable: true),
                    PrescribedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescription_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "ConsultationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_DoctorId",
                table: "Consultations",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_ConsultationId",
                table: "Prescription",
                column: "ConsultationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Doctor_DoctorId",
                table: "Consultations",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "DoctorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Doctor_DoctorId",
                table: "Consultations");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Prescription");

            migrationBuilder.DropIndex(
                name: "IX_Consultations_DoctorId",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "ConsultationDate",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "FollowupDate",
                table: "Consultations");

            migrationBuilder.RenameColumn(
                name: "WaitingPatientId",
                table: "WaitingPatients",
                newName: "WaitPatientId");

            migrationBuilder.RenameColumn(
                name: "Symptoms",
                table: "Consultations",
                newName: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Consultations",
                newName: "LaboratoryTests");

            migrationBuilder.AddColumn<string>(
                name: "BloodPressure",
                table: "Consultations",
                type: "text",
                nullable: true);
        }
    }
}
