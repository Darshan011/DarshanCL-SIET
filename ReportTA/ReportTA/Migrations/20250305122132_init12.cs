using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportTA.Migrations
{
    /// <inheritdoc />
    public partial class init12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Income = table.Column<double>(type: "float", nullable: false),
                    Regime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxAmount = table.Column<double>(type: "float", nullable: false),
                    Cess = table.Column<double>(type: "float", nullable: false),
                    TotalTaxPayable = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxRequests");
        }
    }
}
