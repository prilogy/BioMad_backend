using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Infrastructure.DataConfigurations
{
    public static class ApplicationContextSeed
    {
        public static void SeedData(this ModelBuilder builder)
        {
            builder.Entity<Gender>().HasData(new[]
            {
                new Gender
                {
                    Id = 1,
                    Key = "male"
                },
                new Gender
                {
                    Id = 2,
                    Key = "female"
                },
                new Gender
                {
                    Id = 3,
                    Key = "neutral"
                }
            });

            builder.Entity<GenderTranslation>().HasData(new[]
            {
                new GenderTranslation
                {
                    Id = 1,
                    BaseEntityId = 1,
                    CultureId = Culture.En.Id,
                    Name = "Male"
                },
                new GenderTranslation
                {
                    Id = 2,
                    BaseEntityId = 1,
                    CultureId = Culture.Ru.Id,
                    Name = "Мужской"
                },
                new GenderTranslation
                {
                    Id = 3,
                    BaseEntityId = 2,
                    CultureId = Culture.En.Id,
                    Name = "Female"
                },
                new GenderTranslation
                {
                    Id = 4,
                    BaseEntityId = 2,
                    CultureId = Culture.Ru.Id,
                    Name = "Женский"
                },
                new GenderTranslation
                {
                    Id = 5,
                    BaseEntityId = 3,
                    CultureId = Culture.En.Id,
                    Name = "Neutral"
                },
                new GenderTranslation
                {
                    Id = 6,
                    BaseEntityId = 3,
                    CultureId = Culture.Ru.Id,
                    Name = "Нейтральный"
                }
            });
            
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
                    CultureId = Culture.Ru.Id,
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
                },
                new BiomarkerTypeTranslation
                {
                    Id = 3,
                    CultureId = Culture.En.Id,
                    Name = "Microns",
                    Description = "Microns - lalalalalala alal",
                    BaseEntityId = 2
                },
                new BiomarkerTypeTranslation
                {
                    Id = 4,
                    CultureId = Culture.Ru.Id,
                    Name = "Микроны",
                    Description = "Микроны лалалала лалалала ла",
                    BaseEntityId = 2
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
                    CultureId = Culture.Ru.Id,
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

            builder.Entity<UnitTransfer>().HasData(new []
            {
                new UnitTransfer
                {
                    Id = 1,
                    UnitAId = 2,
                    UnitBId = 1,
                    Coefficient = 1000
                },
                new UnitTransfer
                {
                    Id = 2,
                    UnitAId = 4,
                    UnitBId = 3,
                    Coefficient = 1000
                }
            });
            
            builder.Entity<UnitGroup>().HasData(new[]
            {
                new UnitGroup
                {
                    Id = 1,
                    MainUnitId = 1
                },
                new UnitGroup
                {
                    Id = 2,
                    MainUnitId = 4
                }
            });

            builder.Entity<UnitGroupUnit>().HasData(new[]
            {
                new UnitGroupUnit
                {
                    UnitId = 1,
                    UnitGroupId = 1
                },
                new UnitGroupUnit
                {
                    UnitId = 2,
                    UnitGroupId = 1
                }, 
                new UnitGroupUnit
                {
                    UnitId = 3,
                    UnitGroupId = 2
                }, 
                new UnitGroupUnit
                {
                    UnitId = 4,
                    UnitGroupId = 2
                },
            });

            builder.Entity<Biomarker>().HasData(new[]
            {
                new Biomarker
                {
                    Id = 1,
                    TypeId = 1,
                    UnitGroupId = 1,
                    MainUnitId = 1
                },
                new Biomarker
                {
                    Id = 2,
                    TypeId = 2,
                    UnitGroupId = 2,
                    MainUnitId = 4
                }
            });

            builder.Entity<BiomarkerTranslation>().HasData(new[]
            {
                new BiomarkerTranslation
                {
                    Id = 1,
                    CultureId = Culture.En.Id,
                    Name = "Weight",
                    BaseEntityId = 1,
                    Description = "Weight is weight!"
                },
                new BiomarkerTranslation
                {
                    Id = 2,
                    CultureId = Culture.Ru.Id,
                    Name = "Вес",
                    BaseEntityId = 1,
                    Description = "Вес это вес!"
                },
                new BiomarkerTranslation
                {
                    Id = 3,
                    CultureId = Culture.En.Id,
                    Name = "Discharge",
                    BaseEntityId = 2,
                    Description = "Discharge is discharge!!!"
                },
                new BiomarkerTranslation
                {
                    Id = 4,
                    CultureId = Culture.Ru.Id,
                    Name = "Выделения",
                    BaseEntityId = 2,
                    Description = "Выделения это выделения!!!"
                }
            });

            builder.Entity<Article>().HasData(new[]
            {
                new Article
                {
                    Id = 1
                },
                new Article
                {
                    Id = 2
                },
                new Article
                {
                    Id = 3
                },
                new Article
                {
                    Id = 4
                },
            });

            builder.Entity<ArticleTranslation>().HasData(new[]
            {
                new ArticleTranslation
                {
                    Id = 1,
                    Name = "Перевод статьи про повышенное значение веса ",
                    Text = "### Перевод статьи про повышенное значение веса",
                    BaseEntityId = 1,
                    CultureId = Culture.Ru.Id
                },
                new ArticleTranslation
                {
                    Id = 2,
                    Name = "Article translation about increased value of weight",
                    Text = "### Article translation about increased value of weight",
                    BaseEntityId = 1,
                    CultureId = Culture.En.Id
                },
                new ArticleTranslation
                {
                    Id = 3,
                    Name = "Перевод статьи про понижение веса",
                    Text = "### Перевод статьи про понижение веса",
                    BaseEntityId = 2,
                    CultureId = Culture.Ru.Id
                },
                new ArticleTranslation
                {
                    Id = 4,
                    Name = "Article translation about decreasing weight",
                    Text = "### Article translation about decreasing weight",
                    BaseEntityId = 2,
                    CultureId = Culture.En.Id
                },
                new ArticleTranslation
                {
                    Id = 5,
                    Name = "Перевод статьи пониженном значении выделений",
                    Text = "### Перевод статьи пониженном значении выделений",
                    BaseEntityId = 3,
                    CultureId = Culture.Ru.Id
                },
                new ArticleTranslation
                {
                    Id = 6,
                    Name = "Article translation about decreased discharge value",
                    Text = "### Article translation about decreased discharge value",
                    BaseEntityId = 3,
                    CultureId = Culture.En.Id
                },
                new ArticleTranslation
                {
                    Id = 7,
                    Name = "Перевод статьи о повышении значении выделений",
                    Text = "### Перевод статьи о повышении значении выделений",
                    BaseEntityId = 4,
                    CultureId = Culture.Ru.Id
                },
                new ArticleTranslation
                {
                    Id = 8,
                    Name = "Article translation about increasing discharge value",
                    Text = "### Article translation about increasing discharge value",
                    BaseEntityId = 4,
                    CultureId = Culture.En.Id
                },
            });

            builder.Entity<BiomarkerArticle>().HasData(new[]
            {
                new BiomarkerArticle
                {
                    Id = 1,
                    ArticleId = 1,
                    BiomarkerId = 1,
                    TypeId = BiomarkerArticleType.Increased.Id
                },
                new BiomarkerArticle
                {
                    Id = 1,
                    ArticleId = 2,
                    BiomarkerId = 1,
                    TypeId = BiomarkerArticleType.Decrease.Id
                },
                new BiomarkerArticle
                {
                    Id = 3,
                    ArticleId = 3,
                    BiomarkerId = 2,
                    TypeId = BiomarkerArticleType.Decreased.Id
                },
                new BiomarkerArticle
                {
                    Id = 4,
                    ArticleId = 4,
                    BiomarkerId = 2,
                    TypeId = BiomarkerArticleType.Increase.Id
                }
            });


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

            // builder.Entity<City>().HasData(new[]
            // {
            //     new City
            //     {
            //         Id = 1
            //     },
            //     new City
            //     {
            //         Id = 2
            //     }
            // });

            // builder.Entity<CityTranslation>().HasData(new[]
            // {
            //     new CityTranslation
            //     {
            //         Id = 1,
            //         CultureId = Culture.En.Id,
            //         Name = "Penza",
            //         BaseEntityId = 1,
            //     },
            //     new CityTranslation
            //     {
            //         Id = 2,
            //         CultureId = Culture.Ru.Id,
            //         Name = "Пенза",
            //         BaseEntityId = 1,
            //     },
            //     new CityTranslation
            //     {
            //         Id = 3,
            //         CultureId = Culture.En.Id,
            //         Name = "Moscow",
            //         BaseEntityId = 2,
            //     },
            //     new CityTranslation
            //     {
            //         Id = 4,
            //         CultureId = Culture.Ru.Id,
            //         Name = "Москва",
            //         BaseEntityId = 2,
            //     }
            // });
            //
            // builder.Entity<Lab>().HasData(new[]
            // {
            //     new Lab
            //     {
            //         Id = 1,
            //         CityId = 1,
            //         PhoneNumber = "+79877775522"
            //     },
            //     new Lab
            //     {
            //         Id = 2,
            //         CityId = 2,
            //         PhoneNumber = "+798784541512"
            //     },
            //     new Lab
            //     {
            //         Id = 3,
            //         CityId = 1,
            //         PhoneNumber = "+79995255522"
            //     }
            // });
            //
            // builder.Entity<LabTranslation>().HasData(new[]
            // {
            //     new LabTranslation
            //     {
            //         Id = 1,
            //         CultureId = Culture.En.Id,
            //         Name = "WEbbys lab",
            //         Description = "Lab where webby cookin crack",
            //         BaseEntityId = 1
            //     },
            //     new LabTranslation
            //     {
            //         Id = 2,
            //         CultureId = Culture.Ru.Id,
            //         Name = "Лаба вебби",
            //         Description = "Лаба где вебби готовит крэк",
            //         BaseEntityId = 1
            //     }
            // });
        }
    }
}