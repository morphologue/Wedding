using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wedding.Models.Migrations
{
    public partial class ResponseCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MoochTo",
                table: "Responses",
                type: "char(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoochFrom",
                table: "Responses",
                type: "char(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(2)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Responses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "OfferDay",
                table: "ResponseOffers",
                type: "char(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Responses");

            migrationBuilder.AlterColumn<string>(
                name: "MoochTo",
                table: "Responses",
                type: "nchar(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoochFrom",
                table: "Responses",
                type: "nchar(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OfferDay",
                table: "ResponseOffers",
                type: "nchar(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldNullable: true);
        }
    }
}
