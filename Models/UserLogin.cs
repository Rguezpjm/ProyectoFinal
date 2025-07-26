using System;
using System.Collections.Generic;

namespace ProyectoFinal.Models;

public partial class UserLogin
{
    public int LogCodigo { get; set; }

    public string? LogUsuario { get; set; }

    public string? LogClave { get; set; }

    public string? LogNombre { get; set; }

    public string? LogApellido { get; set; }

    public string? LogNickname { get; set; }

    public string? LogCedula { get; set; }

    public string? LogPais { get; set; }

    public string? LogProvincia { get; set; }

    public string? LogDireccion { get; set; }

    public string? LogTelefono { get; set; }

    public string? LogCorreo { get; set; }

    public string? LogDepartamento { get; set; }

    public string? LogCargo { get; set; }

    public string? LogStatus { get; set; }
}
