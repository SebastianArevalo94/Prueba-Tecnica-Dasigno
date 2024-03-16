using System;
using System.Collections.Generic;

namespace ApiRest.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string PrimerNombre { get; set; } = null!;

    public string? SegundoNombre { get; set; }

    public string PrimerApellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public double Sueldo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
