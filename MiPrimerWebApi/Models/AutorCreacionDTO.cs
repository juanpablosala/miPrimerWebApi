using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApi.Models
{
    public class AutorCreacionDTO
    {
  
        [Required]
        [StringLength(20, ErrorMessage = "El campo Nombre debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }
        [Range(18, 120)] //La edad debe ser entre 18 y 120
        public int Edad { get; set; }    
    }
}
