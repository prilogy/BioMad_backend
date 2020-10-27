﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BioMad_backend.Entities
{
    public class Gender
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key { get; set; }

        public static Gender Male = new Gender { Id = 1, Key = GenderKeys.Male };
        public static Gender Female = new Gender { Id = 2, Key = GenderKeys.Female };
        public static Gender Neutral = new Gender { Id = 3, Key = GenderKeys.Neutral };


        public static GenderKeys Keys = new GenderKeys();
    }

    public class GenderKeys
    {
        public const string Male = "male";
        public const string Female = "female";
        public const string Neutral = "neutral";
    }
}