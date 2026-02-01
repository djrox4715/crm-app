using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLeadFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Leads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Leads",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Leads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Leads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Leads",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Leads");
        }
    }
}
