using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wedding.Models.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("alter database character set utf8mb4 collate utf8mb4_unicode_ci;");

            migrationBuilder.CreateTable(
                name: "Invitees",
                columns: table => new
                {
                    InviteeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthToken = table.Column<string>(nullable: true),
                    AuthTokenValidUntil = table.Column<DateTime>(nullable: true),
                    Code = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitees", x => x.InviteeId);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    ResponseId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Adults = table.Column<int>(nullable: false),
                    BusFrom = table.Column<bool>(nullable: false),
                    BusTo = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Dietary = table.Column<string>(nullable: true),
                    Driving = table.Column<bool>(nullable: false),
                    InviteeId = table.Column<int>(nullable: false),
                    MoochFrom = table.Column<string>(type: "nchar(2)", nullable: true),
                    MoochTo = table.Column<string>(type: "nchar(2)", nullable: true),
                    WineTour = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.ResponseId);
                    table.ForeignKey(
                        name: "FK_Responses_Invitees_InviteeId",
                        column: x => x.InviteeId,
                        principalTable: "Invitees",
                        principalColumn: "InviteeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponseOffers",
                columns: table => new
                {
                    ResponseOfferId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OfferCount = table.Column<int>(nullable: false),
                    OfferDay = table.Column<string>(type: "nchar(2)", nullable: true),
                    ResponseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseOffers", x => x.ResponseOfferId);
                    table.ForeignKey(
                        name: "FK_ResponseOffers_Responses_ResponseId",
                        column: x => x.ResponseId,
                        principalTable: "Responses",
                        principalColumn: "ResponseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResponseOffers_ResponseId",
                table: "ResponseOffers",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_InviteeId",
                table: "Responses",
                column: "InviteeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseOffers");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "Invitees");
        }
    }
}
