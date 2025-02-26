using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class CorrectedCodeFirstRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Patients_PatientId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryTests_Patients_PatientId",
                table: "LaboratoryTests");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Vitals",
                type: "integer",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "LaboratoryTests",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Consultations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_PatientId",
                table: "Vitals",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Patients_PatientId",
                table: "Consultations",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryTests_Patients_PatientId",
                table: "LaboratoryTests",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vitals_Patients_PatientId",
                table: "Vitals",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Patients_PatientId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryTests_Patients_PatientId",
                table: "LaboratoryTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Vitals_Patients_PatientId",
                table: "Vitals");

            migrationBuilder.DropIndex(
                name: "IX_Vitals_PatientId",
                table: "Vitals");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientId",
                table: "Vitals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "LaboratoryTests",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Consultations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Patients_PatientId",
                table: "Consultations",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryTests_Patients_PatientId",
                table: "LaboratoryTests",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
