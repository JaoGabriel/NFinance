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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RendaMensal = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    QuantidadeParcelas = table.Column<int>(type: "int", nullable: false),
                    DataDoGasto = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Investimentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    DataAplicacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PainelDeControle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValorNaCarteira = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    SaldoMensal = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    SaldoAnual = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    GastosMensal = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    GastosAnual = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    ValorInvestidoMensal = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    ValorInvestidoAnual = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    ValorRecebidoAnual = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PainelDeControle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resgate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<decimal>(type: "FLOAT64(18,2)", nullable: false),
                    MotivoResgate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataResgate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resgate", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Investimentos");

            migrationBuilder.DropTable(
                name: "PainelDeControle");

            migrationBuilder.DropTable(
                name: "Resgate");
        }
    }
}
