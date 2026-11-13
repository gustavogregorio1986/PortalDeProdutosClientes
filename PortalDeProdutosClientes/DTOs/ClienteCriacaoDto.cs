using System.ComponentModel.DataAnnotations;

namespace PortalDeProdutosClientes.DTOs
{
    public class ClienteCriacaoDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string NomeCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string EmailCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string Cpf { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;
    }
}