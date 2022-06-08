using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class EditTemporalSendViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

    }
}
