using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class PosFase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identification",
                table: "Customers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "ALFKI",
                columns: new[] { "Email", "Identification" },
                values: new object[] { "alfki@example.com", "1710034065" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "ANTON",
                columns: new[] { "Email", "Identification" },
                values: new object[] { "anton@example.com", "1712345675" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "BOLID",
                columns: new[] { "Email", "Identification" },
                values: new object[] { "bolid@example.com", "0912345675" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "FAMIA",
                columns: new[] { "Email", "Identification" },
                values: new object[] { "famia@example.com", "1801234566" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: "GALED",
                columns: new[] { "Email", "Identification" },
                values: new object[] { "galed@example.com", "0102030400" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Identification",
                table: "Customers");
        }
    }
}
