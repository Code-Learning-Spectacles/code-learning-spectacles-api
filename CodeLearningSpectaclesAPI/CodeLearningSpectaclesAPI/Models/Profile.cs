using System;
using System.Collections.Generic;

namespace CodeLearningSpectaclesAPI.Models;

public partial class Profile
{
    public int Profileid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Profilelanguageconstruct> Profilelanguageconstructs { get; set; } = new List<Profilelanguageconstruct>();
}
