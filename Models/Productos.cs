using System.ComponentModel.DataAnnotations;

namespace FranciscoJoel_AP1_P2.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public int Stock { get; set; }
}
