using System.ComponentModel.DataAnnotations;

namespace Sprint.Dtos

{
    public class ClienteDTO
    {
        public long? Id { get; set; } // Opcional para criação, obrigatório para atualização

        [Required(ErrorMessage = "Nome não pode estar em branco")]
        [MaxLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email não pode estar em branco")]
        [EmailAddress(ErrorMessage = "Email deve ser válido")]
        [MaxLength(100, ErrorMessage = "Email não pode exceder 100 caracteres")]
        public string Email { get; set; }

        [StringLength(20, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 8 e 20 caracteres")]
        public string Senha { get; set; } // Opcional para atualização
    }
}
