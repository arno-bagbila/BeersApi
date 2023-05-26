CREATE TABLE [dbo].[Beer]
(
	[Id] INT IDENTITY(1, 1) NOT NULL, 
    [UId] UNIQUEIDENTIFIER NOT NULL DEFAULT (newid()), 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(3000) NOT NULL, 
    [LogoUrl] NVARCHAR(2048) NOT NULL, 
    [AlcoholLevel] FLOAT NOT NULL CONSTRAINT max_alcohol_level CHECK (AlcoholLevel <= 100), 
    [TiwooRating] FLOAT NOT NULL CONSTRAINT max_tiwoo_rating CHECK (TiwooRating <= 5), 
    [CategoryId] INT NOT NULL,
    [ColorId] INT NOT NULL,
    [CountryId] INT NOT NULL,
    [DateRegistered] DATETIME NOT NULL, 
    CONSTRAINT [FK_Beer_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [FK_Beer_Color] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Color] ([Id]),
    CONSTRAINT [FK_Beer_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]), 
    CONSTRAINT [PK_Beer] PRIMARY KEY ([Id])
    
)
