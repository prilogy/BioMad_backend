using System;

namespace BioMad_backend.Infrastructure.Interfaces
{
    public interface IWithDateCreated
    {
        DateTime DateCreatedAt { get; set; }
    }
}