using System;
using System.Collections.Generic;

namespace CodeLearningSpectaclesAPI.Models;

public partial class Codeconstruct
{
    public int Codeconstructid { get; set; }

    public int Constructtypeid { get; set; }

    public string Name { get; set; } = null!;

    public virtual Constructtype Constructtype { get; set; } = null!;

    public virtual ICollection<Languageconstruct> Languageconstructs { get; set; } = new List<Languageconstruct>();
}
