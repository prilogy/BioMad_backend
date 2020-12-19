﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "Articles" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    CONSTRAINT "PK_Articles" PRIMARY KEY ("Id")
);

CREATE TABLE "ArticleTypes" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Key" text NULL,
    "Name" text NULL,
    "Discriminator" text NOT NULL,
    CONSTRAINT "PK_ArticleTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "BiomarkerReferenceConfigRanges" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Lower" double precision NOT NULL,
    "Upper" double precision NOT NULL,
    CONSTRAINT "PK_BiomarkerReferenceConfigRanges" PRIMARY KEY ("Id")
);

CREATE TABLE "BiomarkerTypes" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    CONSTRAINT "PK_BiomarkerTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "Categories" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

CREATE TABLE "Cultures" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Key" text NULL,
    "Name" text NULL,
    CONSTRAINT "PK_Cultures" PRIMARY KEY ("Id")
);

CREATE TABLE "Genders" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Key" text NULL,
    CONSTRAINT "PK_Genders" PRIMARY KEY ("Id")
);

CREATE TABLE "Images" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Path" text NULL,
    CONSTRAINT "PK_Images" PRIMARY KEY ("Id")
);

CREATE TABLE "Roles" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Key" text NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);

CREATE TABLE "SocialAccountProviders" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NULL,
    CONSTRAINT "PK_SocialAccountProviders" PRIMARY KEY ("Id")
);

CREATE TABLE "UnitGroups" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "MainUnitId" integer NULL,
    CONSTRAINT "PK_UnitGroups" PRIMARY KEY ("Id")
);

CREATE TABLE "Units" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    CONSTRAINT "PK_Units" PRIMARY KEY ("Id")
);

CREATE TABLE "ArticleTranslation" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "CultureId" integer NOT NULL,
    "Name" text NULL,
    "Text" text NULL,
    "BaseEntityId" integer NOT NULL,
    CONSTRAINT "PK_ArticleTranslation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ArticleTranslation_Articles_BaseEntityId" FOREIGN KEY ("BaseEntityId") REFERENCES "Articles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ArticleTranslation_Cultures_CultureId" FOREIGN KEY ("CultureId") REFERENCES "Cultures" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BiomarkerTypeTranslation" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "CultureId" integer NOT NULL,
    "Name" text NULL,
    "Description" text NULL,
    "BaseEntityId" integer NOT NULL,
    CONSTRAINT "PK_BiomarkerTypeTranslation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BiomarkerTypeTranslation_BiomarkerTypes_BaseEntityId" FOREIGN KEY ("BaseEntityId") REFERENCES "BiomarkerTypes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BiomarkerTypeTranslation_Cultures_CultureId" FOREIGN KEY ("CultureId") REFERENCES "Cultures" ("Id") ON DELETE CASCADE
);

CREATE TABLE "CategoryTranslation" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "CultureId" integer NOT NULL,
    "Name" text NULL,
    "Description" text NULL,
    "BaseEntityId" integer NOT NULL,
    CONSTRAINT "PK_CategoryTranslation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CategoryTranslation_Categories_BaseEntityId" FOREIGN KEY ("BaseEntityId") REFERENCES "Categories" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CategoryTranslation_Cultures_CultureId" FOREIGN KEY ("CultureId") REFERENCES "Cultures" ("Id") ON DELETE CASCADE
);

CREATE TABLE "GenderTranslation" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "CultureId" integer NOT NULL,
    "Name" text NULL,
    "BaseEntityId" integer NOT NULL,
    CONSTRAINT "PK_GenderTranslation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_GenderTranslation_Genders_BaseEntityId" FOREIGN KEY ("BaseEntityId") REFERENCES "Genders" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_GenderTranslation_Cultures_CultureId" FOREIGN KEY ("CultureId") REFERENCES "Cultures" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ArticleImage" (
    "ArticleId" integer NOT NULL,
    "ImageId" integer NOT NULL,
    CONSTRAINT "PK_ArticleImage" PRIMARY KEY ("ImageId", "ArticleId"),
    CONSTRAINT "FK_ArticleImage_Articles_ArticleId" FOREIGN KEY ("ArticleId") REFERENCES "Articles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ArticleImage_Images_ImageId" FOREIGN KEY ("ImageId") REFERENCES "Images" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Users" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Email" text NULL,
    "Password" text NULL,
    "DateCreatedAt" timestamp without time zone NOT NULL,
    "CultureId" integer NULL,
    "RoleId" integer NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Users_Cultures_CultureId" FOREIGN KEY ("CultureId") REFERENCES "Cultures" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Users_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UnitGroupTranslation" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "CultureId" integer NOT NULL,
    "Name" text NULL,
    "Description" text NULL,
    "BaseEntityId" integer NOT NULL,
    CONSTRAINT "PK_UnitGroupTranslation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UnitGroupTranslation_UnitGroups_BaseEntityId" FOREIGN KEY ("BaseEntityId") REFERENCES "UnitGroups" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UnitGroupTranslation_Cultures_CultureId" FOREIGN KEY ("CultureId") REFERENCES "Cultures" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Biomarkers" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "TypeId" integer NOT NULL,
    "UnitGroupId" integer NOT NULL,
    "MainUnitId" integer NULL,
    CONSTRAINT "PK_Biomarkers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Biomarkers_Units_MainUnitId" FOREIGN KEY ("MainUnitId") REFERENCES "Units" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Biomarkers_BiomarkerTypes_TypeId" FOREIGN KEY ("TypeId") REFERENCES "BiomarkerTypes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Biomarkers_UnitGroups_UnitGroupId" FOREIGN KEY ("UnitGroupId") REFERENCES "UnitGroups" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UnitGroupUnits" (
    "UnitGroupId" integer NOT NULL,
    "UnitId" integer NOT NULL,
    CONSTRAINT "PK_UnitGroupUnits" PRIMARY KEY ("UnitGroupId", "UnitId"),
    CONSTRAINT "FK_UnitGroupUnits_UnitGroups_UnitGroupId" FOREIGN KEY ("UnitGroupId") REFERENCES "UnitGroups" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UnitGroupUnits_Units_UnitId" FOREIGN KEY ("UnitId") REFERENCES "Units" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UnitTransfers" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "UnitAId" integer NOT NULL,
    "UnitBId" integer NOT NULL,
    "Coefficient" double precision NOT NULL,
    CONSTRAINT "PK_UnitTransfers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UnitTransfers_Units_UnitAId" FOREIGN KEY ("UnitAId") REFERENCES "Units" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UnitTransfers_Units_UnitBId" FOREIGN KEY ("UnitBId") REFERENCES "Units" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UnitTranslation" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "CultureId" integer NOT NULL,
    "Name" text NULL,
    "BaseEntityId" integer NOT NULL,
    "Shorthand" text NULL,
    CONSTRAINT "PK_UnitTranslation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UnitTranslation_Units_BaseEntityId" FOREIGN KEY ("BaseEntityId") REFERENCES "Units" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UnitTranslation_Cultures_CultureId" FOREIGN KEY ("CultureId") REFERENCES "Cultures" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ConfirmationCodes" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Code" text NULL,
    "Type" integer NOT NULL,
    "IsConfirmed" boolean NOT NULL,
    "HelperValue" text NULL,
    "UserId" integer NOT NULL,
    "DateValidUntil" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_ConfirmationCodes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ConfirmationCodes_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Members" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NULL,
    "Color" text NULL,
    "DateCreatedAt" timestamp without time zone NOT NULL,
    "DateBirthday" timestamp without time zone NOT NULL,
    "GenderId" integer NOT NULL,
    "UserId" integer NOT NULL,
    CONSTRAINT "PK_Members" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Members_Genders_GenderId" FOREIGN KEY ("GenderId") REFERENCES "Genders" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Members_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "RefreshTokens" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Token" text NULL,
    "DateExpiration" timestamp without time zone NOT NULL,
    "UserId" integer NOT NULL,
    CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RefreshTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "SocialAccounts" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Key" text NULL,
    "DateCreatedAt" timestamp without time zone NOT NULL,
    "ProviderId" integer NOT NULL,
    "UserId" integer NULL,
    CONSTRAINT "PK_SocialAccounts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_SocialAccounts_SocialAccountProviders_ProviderId" FOREIGN KEY ("ProviderId") REFERENCES "SocialAccountProviders" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_SocialAccounts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "BiomarkerArticles" (
    "BiomarkerId" integer NOT NULL,
    "TypeId" integer NOT NULL,
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "ArticleId" integer NOT NULL,
    CONSTRAINT "PK_BiomarkerArticles" PRIMARY KEY ("TypeId", "BiomarkerId"),
    CONSTRAINT "FK_BiomarkerArticles_Articles_ArticleId" FOREIGN KEY ("ArticleId") REFERENCES "Articles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BiomarkerArticles_Biomarkers_BiomarkerId" FOREIGN KEY ("BiomarkerId") REFERENCES "Biomarkers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BiomarkerArticles_ArticleTypes_TypeId" FOREIGN KEY ("TypeId") REFERENCES "ArticleTypes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BiomarkerReferences" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "ValueA" double precision NOT NULL,
    "ValueB" double precision NOT NULL,
    "UnitId" integer NOT NULL,
    "BiomarkerId" integer NOT NULL,
    CONSTRAINT "PK_BiomarkerReferences" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BiomarkerReferences_Biomarkers_BiomarkerId" FOREIGN KEY ("BiomarkerId") REFERENCES "Biomarkers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BiomarkerReferences_Units_UnitId" FOREIGN KEY ("UnitId") REFERENCES "Units" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BiomarkerTranslation" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "CultureId" integer NOT NULL,
    "Name" text NULL,
    "Description" text NULL,
    "BaseEntityId" integer NOT NULL,
    CONSTRAINT "PK_BiomarkerTranslation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BiomarkerTranslation_Biomarkers_BaseEntityId" FOREIGN KEY ("BaseEntityId") REFERENCES "Biomarkers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BiomarkerTranslation_Cultures_CultureId" FOREIGN KEY ("CultureId") REFERENCES "Cultures" ("Id") ON DELETE CASCADE
);

CREATE TABLE "CategoryBiomarker" (
    "CategoryId" integer NOT NULL,
    "BiomarkerId" integer NOT NULL,
    CONSTRAINT "PK_CategoryBiomarker" PRIMARY KEY ("BiomarkerId", "CategoryId"),
    CONSTRAINT "FK_CategoryBiomarker_Biomarkers_BiomarkerId" FOREIGN KEY ("BiomarkerId") REFERENCES "Biomarkers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CategoryBiomarker_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

CREATE TABLE "MemberAnalyzes" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NULL,
    "Description" text NULL,
    "Date" timestamp without time zone NOT NULL,
    "MemberId" integer NOT NULL,
    "DateCreatedAt" timestamp without time zone NOT NULL,
    "UserId" integer NULL,
    CONSTRAINT "PK_MemberAnalyzes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_MemberAnalyzes_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MemberAnalyzes_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "MemberCategoryStates" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "State" double precision NOT NULL,
    "Difference" double precision NOT NULL,
    "CategoryId" integer NOT NULL,
    "MemberId" integer NOT NULL,
    "DateCreatedAt" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_MemberCategoryStates" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_MemberCategoryStates_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MemberCategoryStates_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Shared" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Token" text NULL,
    "DateCreatedAt" timestamp without time zone NOT NULL,
    "MemberId" integer NOT NULL,
    "BiomarkerIds" text NULL,
    CONSTRAINT "PK_Shared" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Shared_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BiomarkerReferenceConfigs" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "ReferenceId" integer NOT NULL,
    "AgeRangeId" integer NULL,
    "GenderId" integer NOT NULL,
    CONSTRAINT "PK_BiomarkerReferenceConfigs" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BiomarkerReferenceConfigs_BiomarkerReferenceConfigRanges_Ag~" FOREIGN KEY ("AgeRangeId") REFERENCES "BiomarkerReferenceConfigRanges" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_BiomarkerReferenceConfigs_Genders_GenderId" FOREIGN KEY ("GenderId") REFERENCES "Genders" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BiomarkerReferenceConfigs_BiomarkerReferences_ReferenceId" FOREIGN KEY ("ReferenceId") REFERENCES "BiomarkerReferences" ("Id") ON DELETE CASCADE
);

CREATE TABLE "MemberBiomarkerReferences" (
    "BiomarkerReferenceId" integer NOT NULL,
    "MemberId" integer NOT NULL,
    CONSTRAINT "PK_MemberBiomarkerReferences" PRIMARY KEY ("MemberId", "BiomarkerReferenceId"),
    CONSTRAINT "FK_MemberBiomarkerReferences_BiomarkerReferences_BiomarkerRefe~" FOREIGN KEY ("BiomarkerReferenceId") REFERENCES "BiomarkerReferences" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MemberBiomarkerReferences_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE CASCADE
);

CREATE TABLE "MemberBiomarkers" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Value" double precision NOT NULL,
    "DateCreatedAt" timestamp without time zone NOT NULL,
    "UnitId" integer NOT NULL,
    "BiomarkerId" integer NOT NULL,
    "AnalysisId" integer NOT NULL,
    CONSTRAINT "PK_MemberBiomarkers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_MemberBiomarkers_MemberAnalyzes_AnalysisId" FOREIGN KEY ("AnalysisId") REFERENCES "MemberAnalyzes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MemberBiomarkers_Biomarkers_BiomarkerId" FOREIGN KEY ("BiomarkerId") REFERENCES "Biomarkers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MemberBiomarkers_Units_UnitId" FOREIGN KEY ("UnitId") REFERENCES "Units" ("Id") ON DELETE CASCADE
);

INSERT INTO "ArticleTypes" ("Id", "Discriminator", "Key", "Name")
VALUES (2, 'BiomarkerArticleType', 'decrease', 'Как понизить значение');
INSERT INTO "ArticleTypes" ("Id", "Discriminator", "Key", "Name")
VALUES (1, 'BiomarkerArticleType', 'increase', 'Как повысить значение');
INSERT INTO "ArticleTypes" ("Id", "Discriminator", "Key", "Name")
VALUES (3, 'BiomarkerArticleType', 'decreased_desc', 'О пониженном значении');
INSERT INTO "ArticleTypes" ("Id", "Discriminator", "Key", "Name")
VALUES (4, 'BiomarkerArticleType', 'increased_desc', 'О повышенном значении');

INSERT INTO "Articles" ("Id")
VALUES (1);
INSERT INTO "Articles" ("Id")
VALUES (2);
INSERT INTO "Articles" ("Id")
VALUES (3);
INSERT INTO "Articles" ("Id")
VALUES (4);

INSERT INTO "BiomarkerReferenceConfigRanges" ("Id", "Lower", "Upper")
VALUES (1, 18.0, 25.0);

INSERT INTO "BiomarkerTypes" ("Id")
VALUES (1);
INSERT INTO "BiomarkerTypes" ("Id")
VALUES (2);

INSERT INTO "Categories" ("Id")
VALUES (1);
INSERT INTO "Categories" ("Id")
VALUES (2);

INSERT INTO "Cultures" ("Id", "Key", "Name")
VALUES (2, 'ru', 'Русский');
INSERT INTO "Cultures" ("Id", "Key", "Name")
VALUES (1, 'en', 'English');

INSERT INTO "Genders" ("Id", "Key")
VALUES (1, 'male');
INSERT INTO "Genders" ("Id", "Key")
VALUES (2, 'female');
INSERT INTO "Genders" ("Id", "Key")
VALUES (3, 'neutral');

INSERT INTO "Roles" ("Id", "Key")
VALUES (2, 'admin');
INSERT INTO "Roles" ("Id", "Key")
VALUES (1, 'user');

INSERT INTO "SocialAccountProviders" ("Id", "Name")
VALUES (1, 'Google');
INSERT INTO "SocialAccountProviders" ("Id", "Name")
VALUES (2, 'VK');
INSERT INTO "SocialAccountProviders" ("Id", "Name")
VALUES (3, 'Facebook');

INSERT INTO "UnitGroups" ("Id", "MainUnitId")
VALUES (1, 1);
INSERT INTO "UnitGroups" ("Id", "MainUnitId")
VALUES (2, 4);

INSERT INTO "Units" ("Id")
VALUES (1);
INSERT INTO "Units" ("Id")
VALUES (2);
INSERT INTO "Units" ("Id")
VALUES (3);
INSERT INTO "Units" ("Id")
VALUES (4);

INSERT INTO "ArticleTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Text")
VALUES (2, 1, 1, 'Article translation about increased value of weight', '### Article translation about increased value of weight');
INSERT INTO "ArticleTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Text")
VALUES (7, 4, 2, 'Перевод статьи о повышении значении выделений', '### Перевод статьи о повышении значении выделений');
INSERT INTO "ArticleTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Text")
VALUES (5, 3, 2, 'Перевод статьи пониженном значении выделений', '### Перевод статьи пониженном значении выделений');
INSERT INTO "ArticleTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Text")
VALUES (1, 1, 2, 'Перевод статьи про повышенное значение веса ', '### Перевод статьи про повышенное значение веса');
INSERT INTO "ArticleTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Text")
VALUES (3, 2, 2, 'Перевод статьи про понижение веса', '### Перевод статьи про понижение веса');
INSERT INTO "ArticleTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Text")
VALUES (8, 4, 1, 'Article translation about increasing discharge value', '### Article translation about increasing discharge value');
INSERT INTO "ArticleTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Text")
VALUES (6, 3, 1, 'Article translation about decreased discharge value', '### Article translation about decreased discharge value');
INSERT INTO "ArticleTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Text")
VALUES (4, 2, 1, 'Article translation about decreasing weight', '### Article translation about decreasing weight');

INSERT INTO "BiomarkerTypeTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (3, 2, 1, 'Microns - lalalalalala alal', 'Microns');
INSERT INTO "BiomarkerTypeTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (2, 1, 2, 'гармон лалал ывфв', 'Гармоны');
INSERT INTO "BiomarkerTypeTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (4, 2, 2, 'Микроны лалалала лалалала ла', 'Микроны');
INSERT INTO "BiomarkerTypeTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (1, 1, 1, 'Harmon lalalalalala alal', 'Harmons');

INSERT INTO "Biomarkers" ("Id", "MainUnitId", "TypeId", "UnitGroupId")
VALUES (2, 4, 2, 2);
INSERT INTO "Biomarkers" ("Id", "MainUnitId", "TypeId", "UnitGroupId")
VALUES (1, 1, 1, 1);

INSERT INTO "CategoryTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (1, 1, 1, 'Hearth is gotta be good', 'Hearth');
INSERT INTO "CategoryTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (3, 2, 1, 'Nerves should be calm...', 'Nerves');
INSERT INTO "CategoryTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (2, 1, 2, 'Сердце должно быть в поряде!', 'Сердце');
INSERT INTO "CategoryTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (4, 2, 2, 'Нервы должны быть спокойны!...', 'Нервы');

INSERT INTO "GenderTranslation" ("Id", "BaseEntityId", "CultureId", "Name")
VALUES (6, 3, 2, 'Нейтральный');
INSERT INTO "GenderTranslation" ("Id", "BaseEntityId", "CultureId", "Name")
VALUES (5, 3, 1, 'Neutral');
INSERT INTO "GenderTranslation" ("Id", "BaseEntityId", "CultureId", "Name")
VALUES (3, 2, 1, 'Female');
INSERT INTO "GenderTranslation" ("Id", "BaseEntityId", "CultureId", "Name")
VALUES (1, 1, 1, 'Male');
INSERT INTO "GenderTranslation" ("Id", "BaseEntityId", "CultureId", "Name")
VALUES (2, 1, 2, 'Мужской');
INSERT INTO "GenderTranslation" ("Id", "BaseEntityId", "CultureId", "Name")
VALUES (4, 2, 2, 'Женский');

INSERT INTO "UnitGroupUnits" ("UnitGroupId", "UnitId")
VALUES (2, 3);
INSERT INTO "UnitGroupUnits" ("UnitGroupId", "UnitId")
VALUES (1, 2);
INSERT INTO "UnitGroupUnits" ("UnitGroupId", "UnitId")
VALUES (1, 1);
INSERT INTO "UnitGroupUnits" ("UnitGroupId", "UnitId")
VALUES (2, 4);

INSERT INTO "UnitTransfers" ("Id", "Coefficient", "UnitAId", "UnitBId")
VALUES (1, 1000.0, 2, 1);
INSERT INTO "UnitTransfers" ("Id", "Coefficient", "UnitAId", "UnitBId")
VALUES (2, 1000.0, 4, 3);

INSERT INTO "UnitTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Shorthand")
VALUES (6, 3, 2, 'Миллилитр', 'мл');
INSERT INTO "UnitTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Shorthand")
VALUES (4, 2, 2, 'Тонна', 'т');
INSERT INTO "UnitTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Shorthand")
VALUES (7, 4, 1, 'Liter', 'l');
INSERT INTO "UnitTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Shorthand")
VALUES (8, 4, 2, 'Литр', 'л');
INSERT INTO "UnitTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Shorthand")
VALUES (3, 2, 1, 'Ton', 'tn');
INSERT INTO "UnitTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Shorthand")
VALUES (2, 1, 2, 'Килограм', 'кг');
INSERT INTO "UnitTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Shorthand")
VALUES (1, 1, 1, 'Kilogram', 'kg');
INSERT INTO "UnitTranslation" ("Id", "BaseEntityId", "CultureId", "Name", "Shorthand")
VALUES (5, 3, 1, 'Milliliter', 'ml');

INSERT INTO "BiomarkerArticles" ("TypeId", "BiomarkerId", "ArticleId", "Id")
VALUES (4, 1, 1, 1);
INSERT INTO "BiomarkerArticles" ("TypeId", "BiomarkerId", "ArticleId", "Id")
VALUES (2, 1, 2, 1);
INSERT INTO "BiomarkerArticles" ("TypeId", "BiomarkerId", "ArticleId", "Id")
VALUES (3, 2, 3, 3);
INSERT INTO "BiomarkerArticles" ("TypeId", "BiomarkerId", "ArticleId", "Id")
VALUES (1, 2, 4, 4);

INSERT INTO "BiomarkerReferences" ("Id", "BiomarkerId", "UnitId", "ValueA", "ValueB")
VALUES (1, 1, 1, 40.399999999999999, 52.5);
INSERT INTO "BiomarkerReferences" ("Id", "BiomarkerId", "UnitId", "ValueA", "ValueB")
VALUES (2, 1, 1, 50.399999999999999, 62.5);
INSERT INTO "BiomarkerReferences" ("Id", "BiomarkerId", "UnitId", "ValueA", "ValueB")
VALUES (3, 2, 4, 1.0, 3.2999999999999998);
INSERT INTO "BiomarkerReferences" ("Id", "BiomarkerId", "UnitId", "ValueA", "ValueB")
VALUES (4, 2, 4, 5.0, 8.0);

INSERT INTO "BiomarkerTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (1, 1, 1, 'Weight is weight!', 'Weight');
INSERT INTO "BiomarkerTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (2, 1, 2, 'Вес это вес!', 'Вес');
INSERT INTO "BiomarkerTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (3, 2, 1, 'Discharge is discharge!!!', 'Discharge');
INSERT INTO "BiomarkerTranslation" ("Id", "BaseEntityId", "CultureId", "Description", "Name")
VALUES (4, 2, 2, 'Выделения это выделения!!!', 'Выделения');

INSERT INTO "CategoryBiomarker" ("BiomarkerId", "CategoryId")
VALUES (1, 1);
INSERT INTO "CategoryBiomarker" ("BiomarkerId", "CategoryId")
VALUES (2, 1);
INSERT INTO "CategoryBiomarker" ("BiomarkerId", "CategoryId")
VALUES (2, 2);

INSERT INTO "BiomarkerReferenceConfigs" ("Id", "AgeRangeId", "GenderId", "ReferenceId")
VALUES (1, 1, 1, 1);
INSERT INTO "BiomarkerReferenceConfigs" ("Id", "AgeRangeId", "GenderId", "ReferenceId")
VALUES (2, 1, 2, 2);
INSERT INTO "BiomarkerReferenceConfigs" ("Id", "AgeRangeId", "GenderId", "ReferenceId")
VALUES (4, 1, 2, 3);
INSERT INTO "BiomarkerReferenceConfigs" ("Id", "AgeRangeId", "GenderId", "ReferenceId")
VALUES (3, 1, 1, 4);

CREATE INDEX "IX_ArticleImage_ArticleId" ON "ArticleImage" ("ArticleId");

CREATE UNIQUE INDEX "IX_ArticleImage_ImageId_ArticleId" ON "ArticleImage" ("ImageId", "ArticleId");

CREATE INDEX "IX_ArticleTranslation_BaseEntityId" ON "ArticleTranslation" ("BaseEntityId");

CREATE INDEX "IX_ArticleTranslation_CultureId" ON "ArticleTranslation" ("CultureId");

CREATE INDEX "IX_BiomarkerArticles_ArticleId" ON "BiomarkerArticles" ("ArticleId");

CREATE INDEX "IX_BiomarkerArticles_BiomarkerId" ON "BiomarkerArticles" ("BiomarkerId");

CREATE UNIQUE INDEX "IX_BiomarkerArticles_TypeId_BiomarkerId" ON "BiomarkerArticles" ("TypeId", "BiomarkerId");

CREATE INDEX "IX_BiomarkerReferenceConfigs_AgeRangeId" ON "BiomarkerReferenceConfigs" ("AgeRangeId");

CREATE INDEX "IX_BiomarkerReferenceConfigs_GenderId" ON "BiomarkerReferenceConfigs" ("GenderId");

CREATE UNIQUE INDEX "IX_BiomarkerReferenceConfigs_ReferenceId" ON "BiomarkerReferenceConfigs" ("ReferenceId");

CREATE INDEX "IX_BiomarkerReferences_BiomarkerId" ON "BiomarkerReferences" ("BiomarkerId");

CREATE INDEX "IX_BiomarkerReferences_UnitId" ON "BiomarkerReferences" ("UnitId");

CREATE INDEX "IX_Biomarkers_MainUnitId" ON "Biomarkers" ("MainUnitId");

CREATE INDEX "IX_Biomarkers_TypeId" ON "Biomarkers" ("TypeId");

CREATE INDEX "IX_Biomarkers_UnitGroupId" ON "Biomarkers" ("UnitGroupId");

CREATE INDEX "IX_BiomarkerTranslation_BaseEntityId" ON "BiomarkerTranslation" ("BaseEntityId");

CREATE INDEX "IX_BiomarkerTranslation_CultureId" ON "BiomarkerTranslation" ("CultureId");

CREATE INDEX "IX_BiomarkerTypeTranslation_BaseEntityId" ON "BiomarkerTypeTranslation" ("BaseEntityId");

CREATE INDEX "IX_BiomarkerTypeTranslation_CultureId" ON "BiomarkerTypeTranslation" ("CultureId");

CREATE INDEX "IX_CategoryBiomarker_CategoryId" ON "CategoryBiomarker" ("CategoryId");

CREATE UNIQUE INDEX "IX_CategoryBiomarker_BiomarkerId_CategoryId" ON "CategoryBiomarker" ("BiomarkerId", "CategoryId");

CREATE INDEX "IX_CategoryTranslation_BaseEntityId" ON "CategoryTranslation" ("BaseEntityId");

CREATE INDEX "IX_CategoryTranslation_CultureId" ON "CategoryTranslation" ("CultureId");

CREATE INDEX "IX_ConfirmationCodes_UserId" ON "ConfirmationCodes" ("UserId");

CREATE INDEX "IX_GenderTranslation_BaseEntityId" ON "GenderTranslation" ("BaseEntityId");

CREATE INDEX "IX_GenderTranslation_CultureId" ON "GenderTranslation" ("CultureId");

CREATE INDEX "IX_MemberAnalyzes_MemberId" ON "MemberAnalyzes" ("MemberId");

CREATE INDEX "IX_MemberAnalyzes_UserId" ON "MemberAnalyzes" ("UserId");

CREATE UNIQUE INDEX "IX_MemberBiomarkerReferences_BiomarkerReferenceId" ON "MemberBiomarkerReferences" ("BiomarkerReferenceId");

CREATE UNIQUE INDEX "IX_MemberBiomarkerReferences_MemberId_BiomarkerReferenceId" ON "MemberBiomarkerReferences" ("MemberId", "BiomarkerReferenceId");

CREATE INDEX "IX_MemberBiomarkers_AnalysisId" ON "MemberBiomarkers" ("AnalysisId");

CREATE INDEX "IX_MemberBiomarkers_BiomarkerId" ON "MemberBiomarkers" ("BiomarkerId");

CREATE INDEX "IX_MemberBiomarkers_UnitId" ON "MemberBiomarkers" ("UnitId");

CREATE INDEX "IX_MemberCategoryStates_CategoryId" ON "MemberCategoryStates" ("CategoryId");

CREATE INDEX "IX_MemberCategoryStates_MemberId" ON "MemberCategoryStates" ("MemberId");

CREATE INDEX "IX_Members_GenderId" ON "Members" ("GenderId");

CREATE INDEX "IX_Members_UserId" ON "Members" ("UserId");

CREATE INDEX "IX_RefreshTokens_UserId" ON "RefreshTokens" ("UserId");

CREATE INDEX "IX_Shared_MemberId" ON "Shared" ("MemberId");

CREATE INDEX "IX_SocialAccounts_ProviderId" ON "SocialAccounts" ("ProviderId");

CREATE UNIQUE INDEX "IX_SocialAccounts_UserId_ProviderId" ON "SocialAccounts" ("UserId", "ProviderId");

CREATE INDEX "IX_UnitGroupTranslation_BaseEntityId" ON "UnitGroupTranslation" ("BaseEntityId");

CREATE INDEX "IX_UnitGroupTranslation_CultureId" ON "UnitGroupTranslation" ("CultureId");

CREATE INDEX "IX_UnitGroupUnits_UnitId" ON "UnitGroupUnits" ("UnitId");

CREATE INDEX "IX_UnitTransfers_UnitAId" ON "UnitTransfers" ("UnitAId");

CREATE INDEX "IX_UnitTransfers_UnitBId" ON "UnitTransfers" ("UnitBId");

CREATE INDEX "IX_UnitTranslation_BaseEntityId" ON "UnitTranslation" ("BaseEntityId");

CREATE INDEX "IX_UnitTranslation_CultureId" ON "UnitTranslation" ("CultureId");

CREATE INDEX "IX_Users_CultureId" ON "Users" ("CultureId");

CREATE INDEX "IX_Users_RoleId" ON "Users" ("RoleId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20201219172421_Initial', '3.1.9');

