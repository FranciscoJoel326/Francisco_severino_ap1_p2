using System.ComponentModel.DataAnnotations;

namespace FranciscoJoel_AP1_P2.Models;

public class VentasDetalle
{
    [Key]
    public int DetalleId { get; set; }

    public int VentaId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public Productos? Producto { get; set; }
}
