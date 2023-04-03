using System;
using System.Collections.Generic;

namespace App_Library.Models;

public partial class Editoriale
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Sede { get; set; }

    public virtual ICollection<Libro> Libros { get; } = new List<Libro>();
}
