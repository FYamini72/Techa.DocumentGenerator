using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Techa.DocumentGenerator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOutParameterToStoredProcedureParametersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOutParameter",
                table: "StoredProcedureParameters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOutParameter",
                table: "StoredProcedureParameters");
        }
    }
}
