using System;
using System.Collections.Generic;

namespace App_Library.Models;

public partial class Libro
{
    public long Isbn { get; set; }

    public int? EditorialesId { get; set; }

    public string? Titulo { get; set; }

    public string? Sinopsis { get; set; }

    public string? NPaginas { get; set; }

    public virtual AutoresHasLibro? AutoresHasLibro { get; set; }

    public virtual Editoriale? Editoriales { get; set; }
}
