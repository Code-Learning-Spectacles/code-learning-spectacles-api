using System;
using System.Collections.Generic;

namespace CodeLearningSpectaclesAPI.Models;

public partial class Codinglanguage
{
    public int Codinglanguageid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Languageconstruct> Languageconstructs { get; set; } = new List<Languageconstruct>();
}
