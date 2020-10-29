using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BioMad_backend.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BioMad_backend.Entities
{
  public class SocialAccount : IWithDateCreated
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Key { get; set; }

    public DateTime DateCreatedAt { get; set; }

    [JsonIgnore]
    public int ProviderId { get; set; }
    public virtual SocialAccountProvider Provider { get; set; }
    
    [JsonIgnore]
    public int? UserId { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }

    public SocialAccount()
    {
      DateCreatedAt = DateTime.UtcNow;
    }
  }
}