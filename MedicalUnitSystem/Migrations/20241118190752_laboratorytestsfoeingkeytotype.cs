using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class laboratorytestsfoeingkeytotype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LaboratoryTestTypeId",
                table: "LaboratoryTests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryTests_LaboratoryTestTypeId",
                table: "LaboratoryTests",
                column: "LaboratoryTestTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryTests_LaboratoryTestTypes_LaboratoryTestTypeId",
                table: "LaboratoryTests",
                column: "LaboratoryTestTypeId",
                principalTable: "LaboratoryTestTypes",
                principalColumn: "LaboratorytestTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryTests_LaboratoryTestTypes_LaboratoryTestTypeId",
                table: "LaboratoryTests");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryTests_LaboratoryTestTypeId",
                table: "LaboratoryTests");

            migrationBuilder.DropColumn(
                name: "LaboratoryTestTypeId",
                table: "LaboratoryTests");
        }
    }
}
