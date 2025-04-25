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
    [Table("t_CarritoItem")]
    public class CarritoItem
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
    public int Id { get; set; }

    public string UsuarioId { get; set; }
    public int ProductoId { get; set; }

    public string NombreProducto { get; set; }
    public string ImagenUrl { get; set; }
    public decimal PrecioUnitario { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioTotal => PrecioUnitario * Cantidad;

    public Producto Producto { get; set; }
    }

}