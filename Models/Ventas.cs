using System.ComponentModel.DataAnnotations;

namespace FranciscoJoel_AP1_P2.Models;

public class Ventas
{
    [Key]
    public int VentaId { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La observacion es obligatoria.")]
    [StringLength(200, ErrorMessage = "La observacion no puede pasar de 200 caracteres.")]
    public string Observacion { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public List<VentasDetalle> Detalle { get; set; } = [];
}
