using System.ComponentModel.DataAnnotations;

namespace Sprint.Dtos
{
    public class SensorLocalizacaoDTO
    {
        public long? Id { get; set; } // Opcional para criação, obrigatório para atualização

        [Required(ErrorMessage = "Latitude não pode estar em branco")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude não pode estar em branco")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Data e hora da localização não podem estar em branco")]
        public DateTime TimeDaLocalizacao { get; set; }

        [Required(ErrorMessage = "MotoId não pode estar em branco")]
        public long MotoId { get; set; }
    }
}
