using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sprint.Models
{
    public class Cliente
    {

        public long Id { get; set; }

        [Required(ErrorMessage = "Nome não pode estar em branco")]
        [MaxLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email não pode estar em branco")]
        [EmailAddress(ErrorMessage = "Email deve ser válido")]
        [MaxLength(100, ErrorMessage = "Email não pode exceder 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha não pode estar em branco")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 8 e 20 caracteres")]
        public string Senha { get; set; }


        [JsonIgnore]
        public ICollection<Moto> Motos { get; set; } = new List<Moto>();



    }
}
