using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicalUnitSystem.Migrations
{
    public partial class AddedgenderModelandForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Patients",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Doctors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GenderName = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_GenderId",
                table: "Patients",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_GenderId",
                table: "Doctors",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Genders_GenderId",
                table: "Doctors",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Genders_GenderId",
                table: "Patients",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "GenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Genders_GenderId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Genders_GenderId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropIndex(
                name: "IX_Patients_GenderId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_GenderId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Doctors",
                type: "text",
                nullable: true);
        }
    }
}
