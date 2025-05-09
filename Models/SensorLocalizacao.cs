using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sprint.Models
{

    public class SensorLocalizacao
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Latitude não pode estar em branco")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude não pode estar em branco")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Data e hora da localização não podem estar em branco")]
        public DateTime TimeDaLocalizacao { get; set; }

        [ForeignKey("Moto")]
        public long MotoId { get; set; }

        [JsonIgnore]
        public Moto Moto { get; set; }


    }
}
