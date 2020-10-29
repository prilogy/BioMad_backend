﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
  public class SocialAccountProvider
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }

    public static SocialAccountProvider Google = new SocialAccountProvider { Id = 1, Name = Keys.Google };
    public static SocialAccountProvider Vk = new SocialAccountProvider { Id = 2, Name = Keys.Vk };
    public static SocialAccountProvider Facebook = new SocialAccountProvider { Id = 3, Name = Keys.Facebook };
    
    public static class Keys
    {
      public const string Google = "Google";
      public const string Vk = "VK";
      public const string Facebook = "Facebook";
    }
  }
}