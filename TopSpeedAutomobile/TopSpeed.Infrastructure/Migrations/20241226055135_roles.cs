using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TopSpeed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b21ef45b-0e5e-48b3-b00a-098927a59d78", "b21ef45b-0e5e-48b3-b00a-098927a59d78", "MASTERADMIN", "MASTERADMIN" },
                    { "d4c7e1a0-98d1-4c0c-a3e7-03528d189ca1", "d4c7e1a0-98d1-4c0c-a3e7-03528d189ca1", "ADMIN", "ADMIN" },
                    { "dd92e4e1-f1a3-43e8-a594-a7d9b559ed26", "dd92e4e1-f1a3-43e8-a594-a7d9b559ed26", "CUSTOMER", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b21ef45b-0e5e-48b3-b00a-098927a59d78");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4c7e1a0-98d1-4c0c-a3e7-03528d189ca1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd92e4e1-f1a3-43e8-a594-a7d9b559ed26");
        }
    }
}
