using System;
using System.Collections.Generic;

namespace ProyectoFinal.Models;

public partial class Empresa
{
    public int EmpCodigo { get; set; }

    public string? EmpNombre { get; set; }

    public string? EmpDireccion { get; set; }

    public string? EmpTelefono { get; set; }

    public string? EmpRnc { get; set; }

    public byte[]? EmpLogo { get; set; }
}
