using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApiApp.Migrations
{
    public partial class tapi1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    uniqueName = table.Column<string>(nullable: true),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Well",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    platformId = table.Column<int>(nullable: false),
                    uniqueName = table.Column<string>(nullable: true),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Well", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropTable(
                name: "Well");
        }
    }
}
