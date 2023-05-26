IF '$(Intent)'  =  'Test'
BEGIN

 -- Data for tests Categories, Colors, Countries, Flavours

 INSERT INTO dbo.[Category]
	VALUES
	(NEWID(), 'Abbey (Abbaye, Abdji)', 'Belgian family of strong, fruity-tasting ales. Some Benedictine and Nobertine abbeys licence commercial brewers to produce these beers for them.Such products are inspired by those of the authentic Trappist monastery brewers.'),
	(NEWID(), 'Altbier', 'German style of beer similar to British bitter or pale ale. Especially associated with Dusseldorf.')

INSERT INTO dbo.[Color]
	VALUES
	(NEWID(), 'Black'),
	(NEWID(), 'Brown')


INSERT INTO dbo.[Country]
	VALUES
	(NEWID(), 'Belgium', 'be'),
	(NEWID(), 'France', 'fr')

INSERT INTO dbo.[Flavour]
	VALUES
	(NEWID(), 'Acidity', 'An appetizing acidity, sometimes lemony, comes from hops. A fruity acidity derives from the yeast in fermentation, especially in ales and even more so in Berliner Weisse, Belgian lambic styles, and Flemish brown and red ales.'),
	(NEWID(), 'Apples', 'A fresh, delicate, pleasant, dessert-apple character arises from the fermentation process in some English ales, famously Marstons. A more astringent, green-apple taste can arise from insufficient maturation.')

INSERT INTO dbo.[User]
	VALUES
	('5fd4d406-3161-4bc6-a4c3-5672b3d205a9', 'test@email.com', 0, 'username')

INSERT INTO dbo.[Beer]
	VALUES
	('7F5F3728-B0DB-4EE5-B663-432325D7EDA4', 'BeerTest', 'BeerTest description', 'http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg', 7.5, 4.6, 1, 1, 1, GETDATE());

INSERT INTO dbo.[BeerFlavour]
	VALUES
	(1, 1);

END
GO