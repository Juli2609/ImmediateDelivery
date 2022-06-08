using ImmediateDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class Send
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [Display(Name = "Estado")]
        public HistoryStatus HistoryStatus { get; set; }

        public ICollection<SendDetail> SendDetails { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Líneas")]
        public int Lines => SendDetails == null ? 0 : SendDetails.Count;

    }
}
