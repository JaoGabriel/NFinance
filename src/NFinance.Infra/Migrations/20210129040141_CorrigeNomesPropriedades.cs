using Microsoft.EntityFrameworkCore.Migrations;

namespace NFinance.Infra.Migrations
{
    public partial class CorrigeNomesPropriedades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Investimentos",
                newName: "NomeInvestimento");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Gastos",
                newName: "NomeGasto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NomeInvestimento",
                table: "Investimentos",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "NomeGasto",
                table: "Gastos",
                newName: "Nome");
        }
    }
}
