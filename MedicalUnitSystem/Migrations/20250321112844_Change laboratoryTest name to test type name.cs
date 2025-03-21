using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class ChangelaboratoryTestnametotesttypename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LaboratorytestTypeId",
                table: "LaboratoryTestTypes",
                newName: "LaboratoryTestTypeId");

            migrationBuilder.RenameColumn(
                name: "LaboratoryTestName",
                table: "LaboratoryTestTypes",
                newName: "LaboratoryTestTypeName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LaboratoryTestTypeId",
                table: "LaboratoryTestTypes",
                newName: "LaboratorytestTypeId");

            migrationBuilder.RenameColumn(
                name: "LaboratoryTestTypeName",
                table: "LaboratoryTestTypes",
                newName: "LaboratoryTestName");
        }
    }
}
