using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NFinance.Infra.Migrations
{
    public partial class AtualizaCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PainelDeControle");

            migrationBuilder.DropColumn(
                name: "RendaMensal",
                table: "Cliente");

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Cliente",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Cliente",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Cliente");

            migrationBuilder.AddColumn<decimal>(
                name: "RendaMensal",
                table: "Cliente",
                type: "DECIMAL(38)",
                precision: 38,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "PainelDeControle",
                columns: table => new
                {
                    IdPainelDeControle = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GastosAnual = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    GastosMensal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaldoAnual = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    SaldoMensal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    ValorInvestidoAnual = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    ValorInvestidoMensal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    ValorRecebidoAnual = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PainelDeControle", x => x.IdPainelDeControle);
                    table.ForeignKey(
                        name: "FK_PainelDeControle_Cliente_Id",
                        column: x => x.Id,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PainelDeControle_Id",
                table: "PainelDeControle",
                column: "Id");
        }
    }
}
