using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json.Serialization;

namespace Sprint.Models
{

    public class Moto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Placa não pode estar em branco")]
        [MaxLength(10, ErrorMessage = "Placa não pode exceder 10 caracteres")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Modelo não pode estar em branco")]
        [MaxLength(50, ErrorMessage = "Modelo não pode exceder 50 caracteres")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "Número do Chassi não pode estar em branco")]
        [MaxLength(17, ErrorMessage = "Número do Chassi não pode exceder 17 caracteres")]
        public string NumeroChassi { get; set; }

        [Required(ErrorMessage = "Status não pode ser nulo")]
        public StatusMoto Status { get; set; }

        [ForeignKey("Patio")]
        public long PatioId { get; set; }

        [ForeignKey("Cliente")]
        public long ClienteId { get; set; }

        [JsonIgnore]
        public Cliente Cliente { get; set; }

        [JsonIgnore]
        public Patio Patio { get; set; }

        [JsonIgnore]
        public ICollection<SensorLocalizacao> Sensores { get; set; }


    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusMoto
    {
        
        DISPONIVEL,
        EM_USO,
        EM_MANUTENCAO,
        INATIVA
    }
}
