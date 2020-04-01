using System.ComponentModel.DataAnnotations;

namespace MiPrimerWebApi.Models
{
    public class LibroDTO
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public int AutorId { get; set; }
     
    }
}