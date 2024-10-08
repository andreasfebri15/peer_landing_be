﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class coba1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mst_user",
                columns: table => new
                {
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "mst_user2",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mst_user2_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mst_loans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    borrower_id = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    interest_rate = table.Column<decimal>(type: "numeric", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mst_loans_mst_user2_UserId",
                        column: x => x.UserId,
                        principalTable: "mst_user2",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "trn_funding",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    loan_id = table.Column<string>(type: "text", nullable: false),
                    lender_id = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    funded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoansId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trn_funding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trn_funding_mst_loans_LoansId",
                        column: x => x.LoansId,
                        principalTable: "mst_loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_trn_funding_mst_user2_UserId",
                        column: x => x.UserId,
                        principalTable: "mst_user2",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mst_loans_UserId",
                table: "mst_loans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_trn_funding_LoansId",
                table: "trn_funding",
                column: "LoansId");

            migrationBuilder.CreateIndex(
                name: "IX_trn_funding_UserId",
                table: "trn_funding",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mst_user");

            migrationBuilder.DropTable(
                name: "trn_funding");

            migrationBuilder.DropTable(
                name: "mst_loans");

            migrationBuilder.DropTable(
                name: "mst_user2");
        }
    }
}
