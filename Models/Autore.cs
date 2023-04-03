using System;
using System.Collections.Generic;

namespace App_Library.Models;

public partial class Autore
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public virtual ICollection<AutoresHasLibro> AutoresHasLibros { get; } = new List<AutoresHasLibro>();
}
