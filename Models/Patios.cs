using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sprint.Models
{

    public class Patio
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Nome não pode estar em branco")]
        [MaxLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Endereço não pode estar em branco")]
        [MaxLength(255, ErrorMessage = "Endereço não pode exceder 255 caracteres")]
        public string Endereco { get; set; }

        [JsonIgnore]
        public ICollection<Moto> Motos { get; set; }


    }
}
