using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NFinance.Infra.Migrations
{
    public partial class AtualizaDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Cliente_Id",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Investimentos_Cliente_Id",
                table: "Investimentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgate_Investimentos_Id",
                table: "Resgate");

            migrationBuilder.AddColumn<Guid>(
                name: "IdInvestimento",
                table: "Resgate",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IdCliente",
                table: "Investimentos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IdCliente",
                table: "Gastos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdInvestimento",
                table: "Resgate");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Investimentos");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Gastos");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Cliente_Id",
                table: "Gastos",
                column: "Id",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Investimentos_Cliente_Id",
                table: "Investimentos",
                column: "Id",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resgate_Investimentos_Id",
                table: "Resgate",
                column: "Id",
                principalTable: "Investimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
