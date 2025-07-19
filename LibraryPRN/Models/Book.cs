using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace LibraryPRN.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string? Title { get; set; }

    public int? AuthorId { get; set; }

    public string? Genre { get; set; }

    public int? PublicationYear { get; set; }

    public string? Isbn { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public string Status
    {
        get
        {
            return Checkouts.Any(c => !c.Returns.Any()) ? "Loaned" : "Available";
        }
    }
}

