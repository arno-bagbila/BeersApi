CREATE TABLE [dbo].[BeerFlavour]
(
	[BeerId] INT NOT NULL , 
    [FlavourId] INT NOT NULL,
	CONSTRAINT [FK_BeerFlavour_Beer] FOREIGN KEY ([BeerId]) REFERENCES [dbo].[Beer] ([Id]),
	CONSTRAINT [FK_BeerFlavour_Flavour] FOREIGN KEY ([FlavourId]) REFERENCES [dbo].[Flavour] ([Id]), 
    PRIMARY KEY ([BeerId], [FlavourId])
)
