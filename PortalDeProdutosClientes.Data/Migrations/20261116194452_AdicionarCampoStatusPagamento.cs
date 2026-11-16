using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalDeProdutosClientes.Data.Migrations
{
    public partial class AdicionarCampoStatusPagamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusPagamento",
                table: "Pagamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusPagamento",
                table: "Pagamentos");
        }
    }
}
