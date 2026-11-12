using System;

namespace PortalDeProdutosClientes.Domain.Entidades
{
    public class LogAuditoria
    {
        public int Id { get; set; }

        public string Acao { get; set; } = string.Empty; // Ex: "ATUALIZAR_CLIENTE", "CRIAR_PAGAMENTO"

        // Dados do usuário (Identity)
        public string UsuarioId { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;

        // --- NOVOS CAMPOS PARA VINCULAR AOS MÓDULOS ---
        public string EntidadeAfetada { get; set; } = string.Empty; // Ex: "Cliente", "Produto", "Pagamento"
        public int? RegistroId { get; set; } // O ID numérico do cliente, produto ou pagamento afetado

        public string Detalhes { get; set; } = string.Empty; // JSON com o que mudou (antes e depois)
        public DateTime DataOcorrencia { get; set; } = DateTime.UtcNow;
    }
}