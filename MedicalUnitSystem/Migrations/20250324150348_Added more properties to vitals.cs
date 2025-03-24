using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class Addedmorepropertiestovitals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BloodPressure",
                table: "Vitals",
                newName: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "DiastolicBloodPressure",
                table: "Vitals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeartRate",
                table: "Vitals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OxygenSaturation",
                table: "Vitals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RespiratoryRate",
                table: "Vitals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SystolicBloodPressure",
                table: "Vitals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Temperature",
                table: "Vitals",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "VitalsId",
                table: "Vitals",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiastolicBloodPressure",
                table: "Vitals");

            migrationBuilder.DropColumn(
                name: "HeartRate",
                table: "Vitals");

            migrationBuilder.DropColumn(
                name: "OxygenSaturation",
                table: "Vitals");

            migrationBuilder.DropColumn(
                name: "RespiratoryRate",
                table: "Vitals");

            migrationBuilder.DropColumn(
                name: "SystolicBloodPressure",
                table: "Vitals");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "Vitals");

            migrationBuilder.DropColumn(
                name: "VitalsId",
                table: "Vitals");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Vitals",
                newName: "BloodPressure");
        }
    }
}
