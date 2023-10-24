using System;
using System.Collections.Generic;

namespace DL;

public partial class Dulcerium
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string? Descripcion { get; set; }

    public string? Imagen { get; set; }
}
