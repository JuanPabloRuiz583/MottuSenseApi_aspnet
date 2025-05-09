using Sprint.Models;
using System.ComponentModel.DataAnnotations;

namespace Sprint.Dtos
{
    public class MotoDTO
    {
        public long? Id { get; set; }

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

        [Required(ErrorMessage = "PatioId não pode estar em branco")]
        public long? PatioId { get; set; }

        [Required(ErrorMessage = "ClienteId não pode estar em branco")]
        public long? ClienteId { get; set; }
    }
}
