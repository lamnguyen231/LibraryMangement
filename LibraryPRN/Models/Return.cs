using System;
using System.Collections.Generic;

namespace LibraryPRN.Models;

public partial class Return
{
    public int ReturnId { get; set; }

    public int? CheckoutId { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public virtual Checkout? Checkout { get; set; }
}
