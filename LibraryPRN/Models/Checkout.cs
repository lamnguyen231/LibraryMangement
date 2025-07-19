using System;
using System.Collections.Generic;

namespace LibraryPRN.Models;

public partial class Checkout
{
    public int CheckoutId { get; set; }

    public int? MemberId { get; set; }

    public int? BookId { get; set; }

    public DateOnly? CheckoutDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Member? Member { get; set; }

    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();

    public DateOnly? ReturnDateDisplay => Returns.FirstOrDefault(r => r.CheckoutId == CheckoutId)?.ReturnDate;
}
