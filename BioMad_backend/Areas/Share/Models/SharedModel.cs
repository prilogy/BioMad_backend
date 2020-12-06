using System.Collections.Generic;
using BioMad_backend.Entities;

namespace BioMad_backend.Areas.Share.Models
{
  public class SharedModel
  {
    public Shared Shared { get; set; }
    public Member Member { get; set; }

    public Dictionary<int, List<MemberBiomarker>> History { get; set; }
  }
}