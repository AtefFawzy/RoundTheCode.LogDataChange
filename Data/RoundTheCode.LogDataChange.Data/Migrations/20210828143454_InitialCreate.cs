using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoundTheCode.LogDataChange.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ChangeView<VideoGame>",
                columns: table => new
                {
                    ChangeId = table.Column<int>(nullable: false),
                    ReferenceId = table.Column<int>(nullable: false),
                    CUD = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Current = table.Column<string>(nullable: true),
                    Original = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    ReferenceDisplayName = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "VideoGame-Change",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(nullable: false),
                    ChangeData = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoGame-Change", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeView<VideoGame>");

            migrationBuilder.DropTable(
                name: "VideoGame-Change",
                schema: "dbo");
        }
    }
}
