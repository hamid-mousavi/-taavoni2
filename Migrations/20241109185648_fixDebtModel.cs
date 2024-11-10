using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taavoni.Migrations
{
    /// <inheritdoc />
    public partial class fixDebtModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPenaltyAppliedDate",
                table: "Debts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPenaltyAppliedDate",
                table: "Debts");
        }
    }
}
