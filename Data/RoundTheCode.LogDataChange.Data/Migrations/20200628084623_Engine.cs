using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoundTheCode.LogDataChange.Data.Migrations
{
    public partial class Engine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EngineId",
                schema: "dbo",
                table: "VideoGame",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Engine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engine", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoGame_EngineId",
                schema: "dbo",
                table: "VideoGame",
                column: "EngineId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoGame_Engine_EngineId",
                schema: "dbo",
                table: "VideoGame",
                column: "EngineId",
                principalTable: "Engine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoGame_Engine_EngineId",
                schema: "dbo",
                table: "VideoGame");

            migrationBuilder.DropTable(
                name: "Engine");

            migrationBuilder.DropIndex(
                name: "IX_VideoGame_EngineId",
                schema: "dbo",
                table: "VideoGame");

            migrationBuilder.DropColumn(
                name: "EngineId",
                schema: "dbo",
                table: "VideoGame");
        }
    }
}
