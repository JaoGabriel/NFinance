using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NFinance.Infra.Migrations
{
    public partial class AttDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Investimentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ganhos",
                table: "Ganhos");

            migrationBuilder.RenameTable(
                name: "Ganhos",
                newName: "Ganho");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ganho",
                table: "Ganho",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Gasto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeGasto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    QuantidadeParcelas = table.Column<int>(type: "int", nullable: false),
                    DataDoGasto = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gasto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Investimento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeInvestimento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m),
                    DataAplicacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimento", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gasto");

            migrationBuilder.DropTable(
                name: "Investimento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ganho",
                table: "Ganho");

            migrationBuilder.RenameTable(
                name: "Ganho",
                newName: "Ganhos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ganhos",
                table: "Ganhos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataDoGasto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeGasto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuantidadeParcelas = table.Column<int>(type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Investimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAplicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeInvestimento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "DECIMAL(38)", precision: 38, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimentos", x => x.Id);
                });
        }
    }
}
