using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalDeProdutosClientes.Data.Migrations
{
    public partial class AlterandoTocoParaTroco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Toco",
                table: "Pagamentos",
                newName: "Troco");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Troco",
                table: "Pagamentos",
                newName: "Toco");
        }
    }
}
