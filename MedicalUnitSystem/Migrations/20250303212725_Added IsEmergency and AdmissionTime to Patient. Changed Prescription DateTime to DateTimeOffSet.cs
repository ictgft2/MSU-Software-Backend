using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class AddedIsEmergencyandAdmissionTimetoPatientChangedPrescriptionDateTimetoDateTimeOffSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionTime",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsEmergency",
                table: "Patients");
        }
    }
}
