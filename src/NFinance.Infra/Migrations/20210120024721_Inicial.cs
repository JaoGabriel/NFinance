using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NFinance.Infra.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RendaMensal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    QuantidadeParcelas = table.Column<int>(type: "int", nullable: false),
                    DataDoGasto = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gastos_Cliente_Id",
                        column: x => x.Id,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Investimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    DataAplicacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investimentos_Cliente_Id",
                        column: x => x.Id,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PainelDeControle",
                columns: table => new
                {
                    IdPainelDeControle = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaldoMensal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    SaldoAnual = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    GastosMensal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    GastosAnual = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    ValorInvestidoMensal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    ValorInvestidoAnual = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
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

            migrationBuilder.CreateTable(
                name: "Resgate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valor = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    MotivoResgate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataResgate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resgate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resgate_Investimentos_Id",
                        column: x => x.Id,
                        principalTable: "Investimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PainelDeControle_Id",
                table: "PainelDeControle",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "PainelDeControle");

            migrationBuilder.DropTable(
                name: "Resgate");

            migrationBuilder.DropTable(
                name: "Investimentos");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
