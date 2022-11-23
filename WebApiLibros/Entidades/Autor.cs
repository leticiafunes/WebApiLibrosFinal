using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiLibros.Helpers;

namespace WebApiLibros.Entidades
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(30, ErrorMessage = "El nombre del autor puede tener hasta {1} caracteres")]
        public string Nombre { get; set; }

        [Range(18, 120)]
        public int Edad { get; set; }

        [CreditCard]
        public string TarjetaCredito { get; set; }

        [Url]
        public string Url { get; set; }

        public List<Libro> Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre.ToString()[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("Valida Modelo: La primer letra debe ser en mayúscula!", new string[] { nameof(Nombre)} );
                }
            }
        }
    }
}
