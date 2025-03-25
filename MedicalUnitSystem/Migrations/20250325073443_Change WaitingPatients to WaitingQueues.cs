using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class ChangeWaitingPatientstoWaitingQueues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaitingPatients_Patients_PatientId",
                table: "WaitingPatients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaitingPatients",
                table: "WaitingPatients");

            migrationBuilder.RenameTable(
                name: "WaitingPatients",
                newName: "WaitingQueues");

            migrationBuilder.RenameIndex(
                name: "IX_WaitingPatients_PatientId",
                table: "WaitingQueues",
                newName: "IX_WaitingQueues_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaitingQueues",
                table: "WaitingQueues",
                column: "WaitingQueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaitingQueues_Patients_PatientId",
                table: "WaitingQueues",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaitingQueues_Patients_PatientId",
                table: "WaitingQueues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaitingQueues",
                table: "WaitingQueues");

            migrationBuilder.RenameTable(
                name: "WaitingQueues",
                newName: "WaitingPatients");

            migrationBuilder.RenameIndex(
                name: "IX_WaitingQueues_PatientId",
                table: "WaitingPatients",
                newName: "IX_WaitingPatients_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaitingPatients",
                table: "WaitingPatients",
                column: "WaitingQueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaitingPatients_Patients_PatientId",
                table: "WaitingPatients",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
