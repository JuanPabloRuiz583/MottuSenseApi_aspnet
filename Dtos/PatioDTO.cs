using System.ComponentModel.DataAnnotations;

namespace Sprint.Dtos
{
    public class PatioDTO
    {

        public long? Id { get; set; } // Opcional para criação, obrigatório para atualização

        [Required(ErrorMessage = "Nome não pode estar em branco")]
        [MaxLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Endereço não pode estar em branco")]
        [MaxLength(255, ErrorMessage = "Endereço não pode exceder 255 caracteres")]
        public string Endereco { get; set; }
    }
}
