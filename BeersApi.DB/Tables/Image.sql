CREATE TABLE [dbo].[Image]
(
	[Id] INT IDENTITY(1, 1) NOT NULL, 
    [UId] UNIQUEIDENTIFIER NOT NULL DEFAULT (newid()), 
    [Title] NVARCHAR(50) NOT NULL, 
    [ImageUrl] NVARCHAR(2048) NOT NULL, 
    [BeerId] INT NOT NULL, 
    [DateRegistered] DATETIME NOT NULL, 
    CONSTRAINT [PK_Image] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Image_Beer] FOREIGN KEY ([BeerId]) REFERENCES [dbo].[Beer] ([Id]),
)
