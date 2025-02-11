using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Techa.DocumentGenerator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedureRoleBaseAAA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRoles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectRoles_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectRoles_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoredProcedureRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoredProcedureId = table.Column<int>(type: "int", nullable: false),
                    ProjectRoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredProcedureRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoredProcedureRoles_ProjectRoles_ProjectRoleId",
                        column: x => x.ProjectRoleId,
                        principalTable: "ProjectRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoredProcedureRoles_StoredProcedures_StoredProcedureId",
                        column: x => x.StoredProcedureId,
                        principalTable: "StoredProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoredProcedureRoles_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoredProcedureRoles_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRoles_CreatedByUserId",
                table: "ProjectRoles",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRoles_ModifiedByUserId",
                table: "ProjectRoles",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRoles_ProjectId",
                table: "ProjectRoles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredProcedureRoles_CreatedByUserId",
                table: "StoredProcedureRoles",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredProcedureRoles_ModifiedByUserId",
                table: "StoredProcedureRoles",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredProcedureRoles_ProjectRoleId",
                table: "StoredProcedureRoles",
                column: "ProjectRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredProcedureRoles_StoredProcedureId",
                table: "StoredProcedureRoles",
                column: "StoredProcedureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoredProcedureRoles");

            migrationBuilder.DropTable(
                name: "ProjectRoles");
        }
    }
}
