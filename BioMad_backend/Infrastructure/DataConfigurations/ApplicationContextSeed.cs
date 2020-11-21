using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Infrastructure.DataConfigurations
{
    public static class ApplicationContextSeed
    {
        public static void SeedData(this ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(new[]
            {
                new Category
                {
                    Id = 1
                },
                new Category
                {
                    Id = 2
                }
            });

            builder.Entity<CategoryTranslation>().HasData(new[]
            {
                new CategoryTranslation
                {
                    Id = 1,
                    CultureId = Culture.En.Id,
                    Name = "Hearth",
                    Description = "Hearth is gotta be good",
                    BaseEntityId = 1
                },
                new CategoryTranslation
                {
                    Id = 2,
                    CultureId = Culture.Ru.Id,
                    Name = "Сердце",
                    Description = "Сердце должно быть в поряде!",
                    BaseEntityId = 1
                },
                new CategoryTranslation
                {
                    Id = 3,
                    CultureId = Culture.En.Id,
                    Name = "Nerves",
                    Description = "Nerves should be calm...",
                    BaseEntityId = 2
                },
                new CategoryTranslation
                {
                    Id = 4,
                    CultureId = Culture.En.Id,
                    Name = "Нервы",
                    Description = "Нервы должны быть спокойны!...",
                    BaseEntityId = 2
                },
            });

            builder.Entity<BiomarkerType>().HasData(new[]
            {
                new BiomarkerType
                {
                    Id = 1,
                },
                new BiomarkerType
                {
                    Id = 2
                }
            });

            builder.Entity<BiomarkerTypeTranslation>().HasData(new[]
            {
                new BiomarkerTypeTranslation
                {
                    Id = 1,
                    CultureId = Culture.En.Id,
                    Name = "Harmons",
                    Description = "Harmon lalalalalala alal",
                    BaseEntityId = 1
                },
                new BiomarkerTypeTranslation
                {
                    Id = 2,
                    CultureId = Culture.Ru.Id,
                    Name = "Гармоны",
                    Description = "гармон лалал ывфв",
                    BaseEntityId = 1
                }
            });

            builder.Entity<Unit>().HasData(new[]
            {
                new Unit { Id = 1 },
                new Unit { Id = 2 },
                new Unit { Id = 3 },
                new Unit { Id = 4 }
            });

            builder.Entity<UnitTranslation>().HasData(new[]
            {
                new UnitTranslation
                {
                    Id = 1,
                    Name = "Kilogram",
                    CultureId = Culture.En.Id,
                    BaseEntityId = 1,
                    Shorthand = "kg"
                },
                new UnitTranslation
                {
                    Id = 2,
                    Name = "Килограм",
                    CultureId = Culture.En.Id,
                    BaseEntityId = 1,
                    Shorthand = "кг"
                },
                new UnitTranslation
                {
                    Id = 3,
                    Name = "Ton",
                    CultureId = Culture.En.Id,
                    BaseEntityId = 2,
                    Shorthand = "tn"
                },
                new UnitTranslation
                {
                    Id = 4,
                    Name = "Тонна",
                    CultureId = Culture.Ru.Id,
                    BaseEntityId = 2,
                    Shorthand = "т"
                },
                new UnitTranslation
                {
                    Id = 5,
                    Name = "Milliliter",
                    CultureId = Culture.En.Id,
                    BaseEntityId = 3,
                    Shorthand = "ml"
                },
                new UnitTranslation
                {
                    Id = 6,
                    Name = "Миллилитр",
                    CultureId = Culture.Ru.Id,
                    BaseEntityId = 3,
                    Shorthand = "мл"
                },
                new UnitTranslation
                {
                    Id = 7,
                    Name = "Liter",
                    CultureId = Culture.En.Id,
                    BaseEntityId = 4,
                    Shorthand = "l"
                },
                new UnitTranslation
                {
                    Id = 8,
                    Name = "Литр",
                    CultureId = Culture.Ru.Id,
                    BaseEntityId = 4,
                    Shorthand = "л"
                }
            });

            builder.Entity<Biomarker>().HasData(new[]
            {
                new Biomarker
                {
                    Id = 1,
                    TypeId = 1
                },
                new Biomarker
                {
                    Id = 2,
                    TypeId = 2
                }
            });

            builder.Entity<BiomarkerTranslation>().HasData(new[]
            {
                new BiomarkerTranslation
                {
                    Id = 1,
                    CultureId = Culture.En.Id,
                    Name = "Weight",
                    BaseEntityId = 1
                },
                new BiomarkerTranslation
                {
                    Id = 2,
                    CultureId = Culture.Ru.Id,
                    Name = "Вес",
                    BaseEntityId = 1
                },
                new BiomarkerTranslation
                {
                    Id = 3,
                    CultureId = Culture.En.Id,
                    Name = "Discharge",
                    BaseEntityId = 2
                },
                new BiomarkerTranslation
                {
                    Id = 4,
                    CultureId = Culture.Ru.Id,
                    Name = "Выделения",
                    BaseEntityId = 2
                }
            });

            builder.Entity<BiomarkerUnit>().HasData(new[]
            {
                new BiomarkerUnit
                {
                    BiomarkerId = 1,
                    UnitId = 1
                },
                new BiomarkerUnit
                {
                    BiomarkerId = 1,
                    UnitId = 2
                },
                new BiomarkerUnit
                {
                    BiomarkerId = 2,
                    UnitId = 3
                },
                new BiomarkerUnit
                {
                    BiomarkerId = 2,
                    UnitId = 4
                },
            });

            // biomarker 1 - вес(юниты 1,2), 2 - выделения(юниты 3,4)

            builder.Entity<BiomarkerReference>().HasData(new[]
            {
                new BiomarkerReference
                {
                    Id = 1,
                    UnitId = 1,
                    ValueA = 40.4,
                    ValueB = 52.5,
                    BiomarkerId = 1
                },
                new BiomarkerReference
                {
                    Id = 2,
                    UnitId = 1,
                    ValueA = 50.4,
                    ValueB = 62.5,
                    BiomarkerId = 1
                },
                new BiomarkerReference
                {
                    Id = 3,
                    UnitId = 4,
                    ValueA = 1,
                    ValueB = 3.3,
                    BiomarkerId = 2
                },
                new BiomarkerReference
                {
                    Id = 4,
                    UnitId = 4,
                    ValueA = 5,
                    ValueB = 8,
                    BiomarkerId = 2
                },
            });

            builder.Entity<BiomarkerReferenceConfigRange>().HasData(new[]
            {
                new BiomarkerReferenceConfigRange
                {
                    Id = 1,
                    Lower = 18,
                    Upper = 25
                }
            });

            builder.Entity<BiomarkerReferenceConfig>().HasData(new[]
            {
                new BiomarkerReferenceConfig
                {
                    Id = 1,
                    GenderId = 1,
                    ReferenceId = 1,
                    AgeRangeId = 1
                },
                new BiomarkerReferenceConfig
                {
                    Id = 2,
                    GenderId = 2,
                    ReferenceId = 2,
                    AgeRangeId = 1
                },
                new BiomarkerReferenceConfig
                {
                    Id = 3,
                    GenderId = 1,
                    ReferenceId = 4,
                    AgeRangeId = 1
                },
                new BiomarkerReferenceConfig
                {
                    Id = 4,
                    GenderId = 2,
                    ReferenceId = 3,
                    AgeRangeId = 1
                }
            });

            builder.Entity<CategoryBiomarker>().HasData(new[]
            {
                new CategoryBiomarker
                {
                    BiomarkerId = 1,
                    CategoryId = 1,
                },
                new CategoryBiomarker
                {
                    BiomarkerId = 2,
                    CategoryId = 1
                },
                new CategoryBiomarker
                {
                    BiomarkerId = 2,
                    CategoryId = 2
                }
            });

            builder.Entity<City>().HasData(new[]
            {
                new City
                {
                    Id = 1
                },
                new City
                {
                    Id = 2
                }
            });

            builder.Entity<CityTranslation>().HasData(new[]
            {
                new CityTranslation
                {
                    Id = 1,
                    CultureId = Culture.En.Id,
                    Name = "Penza",
                    BaseEntityId = 1,
                },
                new CityTranslation
                {
                    Id = 2,
                    CultureId = Culture.Ru.Id,
                    Name = "Пенза",
                    BaseEntityId = 1,
                },
                new CityTranslation
                {
                    Id = 3,
                    CultureId = Culture.En.Id,
                    Name = "Moscow",
                    BaseEntityId = 2,
                },
                new CityTranslation
                {
                    Id = 4,
                    CultureId = Culture.Ru.Id,
                    Name = "Москва",
                    BaseEntityId = 2,
                }
            });

            builder.Entity<Lab>().HasData(new[]
            {
                new Lab
                {
                    Id = 1,
                    CityId = 1,
                    PhoneNumber = "+79877775522"
                },
                new Lab
                {
                    Id = 2,
                    CityId = 2,
                    PhoneNumber = "+798784541512"
                },
                new Lab
                {
                    Id = 3,
                    CityId = 1,
                    PhoneNumber = "+79995255522"
                }
            });

            builder.Entity<LabTranslation>().HasData(new[]
            {
                new LabTranslation
                {
                    Id = 1,
                    CultureId = Culture.En.Id,
                    Name = "WEbbys lab",
                    Description = "Lab where webby cookin crack",
                    BaseEntityId = 1
                },
                new LabTranslation
                {
                    Id = 2,
                    CultureId = Culture.Ru.Id,
                    Name = "Лаба вебби",
                    Description = "Лаба где вебби готовит крэк",
                    BaseEntityId = 1
                }
            });
        }
    }
}