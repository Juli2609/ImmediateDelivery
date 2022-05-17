﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }


        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7033/images/noimage.png"
            : $"https://shoppingzulu.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Imagen")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Despartamento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar un Departamento.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = "Ciudad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una ciudad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        [Display(Name = "Barrio / Vereda ")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar un Barrio/Vereda")]

        public int NeighborhoodId { get; set; }

        public IEnumerable<SelectListItem> Neighborhoods { get; set; }


    }

}
