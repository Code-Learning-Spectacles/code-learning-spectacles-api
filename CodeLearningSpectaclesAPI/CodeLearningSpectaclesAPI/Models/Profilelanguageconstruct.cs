using System;
using System.Collections.Generic;

namespace CodeLearningSpectaclesAPI.Models;

public partial class Profilelanguageconstruct
{
    public int Profilelanguageconstructid { get; set; }

    public int Profileid { get; set; }

    public int Languageconstructid { get; set; }

    public string? Notes { get; set; }

    public virtual Languageconstruct Languageconstruct { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;
}
