using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string? Nombre { get; set; }

    public string Correo { get; set; } = null!;

    public string Password { get; set; } = null!;
}
