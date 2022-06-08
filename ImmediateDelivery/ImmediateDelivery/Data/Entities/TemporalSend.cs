using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class TemporalSend
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Package Package { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

    }
}
