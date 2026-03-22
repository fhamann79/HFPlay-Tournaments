using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Index("Tenant_Slug_Index", IsUnique = true)]
        [Display(Name = "Slug")]
        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
