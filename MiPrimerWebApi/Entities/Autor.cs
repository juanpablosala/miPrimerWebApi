using MiPrimerWebApi.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApi.Entities
{
    public class Autor : IValidatableObject //validacion personalizada por modelo, se limita al modelo
    {
        public int Id { get; set; }
        [Required]
        //[PrimeraLetraMayuscula]
        [StringLength(20, ErrorMessage = "El campo Nombre debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }
        [Range(18,120)] //La edad debe ser entre 18 y 120
        public int Edad { get; set; }
        //[CreditCard]
        public string CreditCard { get; set; }
        [Url]
        public string Url { get; set; }
        public List<Libro> Libros { get; set; }

        //primero se hacen las validaciones por atributo y luego las validaciones por modelo

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if(primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula", new string[] { nameof(Nombre) });
                }
            }
        }
    }
}
