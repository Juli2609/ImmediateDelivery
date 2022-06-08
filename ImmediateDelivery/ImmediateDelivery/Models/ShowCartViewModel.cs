using ImmediateDelivery.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class ShowCartViewModel
    {
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        public ICollection<TemporalSend> TemporalSends { get; set; }

    }

}
