using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Techa.DocumentGenerator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mig_20250403_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ScriptDebuggers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScriptDebuggers_ProjectId",
                table: "ScriptDebuggers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScriptDebuggers_Projects_ProjectId",
                table: "ScriptDebuggers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScriptDebuggers_Projects_ProjectId",
                table: "ScriptDebuggers");

            migrationBuilder.DropIndex(
                name: "IX_ScriptDebuggers_ProjectId",
                table: "ScriptDebuggers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ScriptDebuggers");
        }
    }
}
