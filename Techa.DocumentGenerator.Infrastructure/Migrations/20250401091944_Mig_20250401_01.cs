using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Techa.DocumentGenerator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mig_20250401_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScriptDebuggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Script = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptDebuggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScriptDebuggers_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScriptDebuggers_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScriptDebuggers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScriptDebuggerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScriptDebuggerId = table.Column<int>(type: "int", nullable: false),
                    ResultValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptDebuggerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScriptDebuggerDetails_ScriptDebuggers_ScriptDebuggerId",
                        column: x => x.ScriptDebuggerId,
                        principalTable: "ScriptDebuggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScriptDebuggerDetails_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScriptDebuggerDetails_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScriptDebuggerDetails_CreatedByUserId",
                table: "ScriptDebuggerDetails",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptDebuggerDetails_ModifiedByUserId",
                table: "ScriptDebuggerDetails",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptDebuggerDetails_ScriptDebuggerId",
                table: "ScriptDebuggerDetails",
                column: "ScriptDebuggerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptDebuggers_CreatedByUserId",
                table: "ScriptDebuggers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptDebuggers_ModifiedByUserId",
                table: "ScriptDebuggers",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptDebuggers_UserId",
                table: "ScriptDebuggers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScriptDebuggerDetails");

            migrationBuilder.DropTable(
                name: "ScriptDebuggers");
        }
    }
}
