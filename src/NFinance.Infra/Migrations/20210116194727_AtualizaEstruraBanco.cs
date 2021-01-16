using Microsoft.EntityFrameworkCore.Migrations;

namespace NFinance.Infra.Migrations
{
    public partial class AtualizaEstruraBanco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PainelDeControle",
                table: "PainelDeControle");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Resgate");

            migrationBuilder.DropColumn(
                name: "ValorNaCarteira",
                table: "PainelDeControle");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Resgate",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PainelDeControle",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdPainelDeControle",
                table: "PainelDeControle",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cliente",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PainelDeControle",
                table: "PainelDeControle",
                column: "IdPainelDeControle");

            migrationBuilder.CreateIndex(
                name: "IX_PainelDeControle_Id",
                table: "PainelDeControle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Gastos_Id",
                table: "Cliente",
                column: "Id",
                principalTable: "Gastos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Investimentos_Id",
                table: "Cliente",
                column: "Id",
                principalTable: "Investimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PainelDeControle_Cliente_Id",
                table: "PainelDeControle",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Gastos_Id",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Investimentos_Id",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_PainelDeControle_Cliente_Id",
                table: "PainelDeControle");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgate_Investimentos_Id",
                table: "Resgate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PainelDeControle",
                table: "PainelDeControle");

            migrationBuilder.DropIndex(
                name: "IX_PainelDeControle_Id",
                table: "PainelDeControle");

            migrationBuilder.DropColumn(
                name: "IdPainelDeControle",
                table: "PainelDeControle");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Resgate",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Resgate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PainelDeControle",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorNaCarteira",
                table: "PainelDeControle",
                type: "FLOAT64(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cliente",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PainelDeControle",
                table: "PainelDeControle",
                column: "Id");
        }
    }
}
