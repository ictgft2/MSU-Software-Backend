using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class ChangeWaitingPatienttoWaitingQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WaitingPatientId",
                table: "WaitingPatients",
                newName: "WaitingQueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WaitingQueueId",
                table: "WaitingPatients",
                newName: "WaitingPatientId");
        }
    }
}
