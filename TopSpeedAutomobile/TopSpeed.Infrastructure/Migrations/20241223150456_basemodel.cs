using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopSpeed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class basemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateOn",
                table: "Brand",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Brand",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateOn",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Brand");
        }
    }
}
