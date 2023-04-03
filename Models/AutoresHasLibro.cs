using System;
using System.Collections.Generic;

namespace App_Library.Models;

public partial class AutoresHasLibro
{
    public int AutoresId { get; set; }

    public long LibrosIsbn { get; set; }

    public virtual Autore Autores { get; set; } = null!;

    public virtual Libro LibrosIsbnNavigation { get; set; } = null!;
}
