using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P2.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace P2.Models
{
    [Table("t_Pago")]
    public class Pago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "El nombre de la tarjeta es obligatorio.")]
        public string? NombreTarjeta { get; set; }

        [Required(ErrorMessage = "El número de la tarjeta es obligatorio.")]
        public string? NumeroTarjeta { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El mes/año es obligatorio.")]
        public string? DueDateYYMM { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El Cvv es obligatorio.")]
        public string? Cvv { get; set; }
        
        public Decimal MontoTotal { get; set; }

        public string? Status { get; set; }
        public string? UsuarioId { get; set; }

    }
}