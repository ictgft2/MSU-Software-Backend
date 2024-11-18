using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class laboratorytestsandtypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LaboratoryTests",
                columns: table => new
                {
                    LaboratoryTestId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryTests", x => x.LaboratoryTestId);
                    table.ForeignKey(
                        name: "FK_LaboratoryTests_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LaboratoryTestTypes",
                columns: table => new
                {
                    LaboratorytestTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LaboratoryTestName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryTestTypes", x => x.LaboratorytestTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryTests_PatientId",
                table: "LaboratoryTests",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaboratoryTests");

            migrationBuilder.DropTable(
                name: "LaboratoryTestTypes");
        }
    }
}
