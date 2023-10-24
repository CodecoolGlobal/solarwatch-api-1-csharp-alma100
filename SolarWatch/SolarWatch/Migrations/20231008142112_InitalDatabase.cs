using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarWatch.Migrations
{
    /// <inheritdoc />
    public partial class InitalDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Long = table.Column<double>(type: "float", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SunriseSunsets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sunrise = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sunset = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Long = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SunriseSunsets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "SunriseSunsets");
        }
    }
}
