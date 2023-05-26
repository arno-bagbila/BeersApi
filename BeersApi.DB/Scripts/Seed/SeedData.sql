
IF '$(Intent)' IN ('Release')
BEGIN

/*
	Categories
*/

--------Load Categories
IF OBJECT_ID('TempDb..#beersApiCategory', 'U') IS NOT NULL
    DROP TABLE #beersApiCategory
;

SELECT
    [UId],
    [Name],
    [Description]
INTO
    #beersApiCategory
FROM
    dbo.[Category]
WHERE
    1 = 2
;

INSERT INTO #beersApiCategory
VALUES
    (NEWID(), 'Abbey (Abbaye, Abdji)', 'Belgian family of strong, fruity-tasting ales. Some Benedictine and Nobertine abbeys licence commercial brewers to produce these beers for them.Such products are inspired by those of the authentic Trappist monastery brewers.'),
    (NEWID(), 'Altbier', 'German style of beer similar to British bitter or pale ale. Especially associated with Dusseldorf.'),
    (NEWID(), 'Biere de garde', 'Strong, ale-like style, originally brewed to be laid down. Typical in northwest of France'),
    (NEWID(), 'Bitter', 'Implies a well hopped ale.'),
    (NEWID(), 'ESB', 'Extra Special Bitter. In Britain, a specific beer from the Fuller''s brewery. Has inspired many similar beers in North America.'),
    (NEWID(), 'Imperial stout', 'Extra-strong stout, originally popular in Imperial Russia.'),
    (NEWID(), 'IPA', 'Indian Pale Ale. Type of ale originally made for the Indian Empire. Should be above average in both hop bitterness and alcohol content.'),
    (NEWID(), 'Irish ale', 'Typically has a reddish colour and a malt accent. It sometimes has a suggestion of butterscotch.'),
    (NEWID(), 'Kellerbier', '"Cellar beer" in German. It is usually an unfiltered lager, high in hop and low in carbonation.'),
    (NEWID(), 'Lambic', 'Belgian fermented beers with wild yeasts.'),
    (NEWID(), 'Marzenbier', 'Traditionally, a beer brewed in March and matured until September or October. In Germany and US, usually implies a reddish-bronze, aromatically malty, medium-strong (around 5.5abv/4.4w or more) lager.'),
    (NEWID(), 'Munich-style', 'Typically a malt accented lager of conventional strength, whether pale (Helles) or dark (Dunkel).'),
    (NEWID(), 'Geuze', 'Young and old lambics, blended to achieve a sparkling, champagne-like beer.'),
    (NEWID(), 'Old ale', 'Usually dark and classically medium-strong (around 6.0abv/4.8w). Some are much stronger.'),
    (NEWID(), 'Pale ale', 'Originally a British style. Classically ranges from bronze to a full copper colour. "Pale" as opposed to a brown ale or porter.'),
    (NEWID(), 'Pils/Plilsner/Pilsener', 'Widely misused term. A Pilsner is more than just a standard golden lager of around 4.25-5.25abv/3.4-4.2w. It should be an all-malt brew, with a pronounced, flowery hop aroma and dryness, typically using the Saaz variety. The original is Pilsner Urquell.'),
    (NEWID(), 'Porter', 'Dark brown or black. Made with highly-kilned malts, with a good hop balance, and traditionally top-fermenting. Traditionally associated with London.'),
    (NEWID(), 'Red ale', 'Like the Irish ale.'),
    (NEWID(), 'Saison', 'Style of dry, sometimes slightly sour, refreshing, but strongish (5.0-8.0abv/4.0-6.4w) summer ale, often bottle-conditioned. Typical in the province of Hainault, Belgium.'),
    (NEWID(), 'Schwarzbier', '"Black" beer. Usually a very dark lager with a bitter-chocolate character. Especially associated with Thuringia and the former East Germany.'),
    (NEWID(), 'Scotch ale', 'Smooth, malty style classically made in Scotland. Often dark, sometimes strong.'),
    (NEWID(), 'Stout', 'Dark brown to black. Made with highly-roasted grains and traditionally top-fermenting. Sweet stouts are historically associated with London, hoppier dry examples with Dublin and Cork.'),
    (NEWID(), 'Trappist', 'Strong ales of great character made in several monasteries in Belgium. Labels include a logo saying "Authentic Trappist".'),
    (NEWID(), 'Tripel/Triple', 'Usually an extra-strong, golden, aromatic hoppy golden ale, modelled on Westmalle Tripel.'),
    (NEWID(), 'Vienna-style lager', 'Bronze-to-red lager with a sweetish malt aroma and flavour. No longer readily available in its city of origin, but increasingly made in the US. The Marzen-Oktoberfest type is a stronger version.'),
    (NEWID(), 'Weisse/Weissbier', 'Wheat beer, pale head and often a cloudy brew.')

MERGE [dbo].[Category] AS TARGET
USING #beersApiCategory AS SOURCE
ON TARGET.[UId] = SOURCE.[UId]
WHEN MATCHED THEN
    UPDATE SET
        TARGET.[Name] = SOURCE.[Name],
        TARGET.[Description] = SOURCE.[Description]
WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [UId],
        [Name],
        [Description]
        ) VALUES (
            SOURCE.[UId],
            SOURCE.[Name],
            SOURCE.[Description]
        )
;

/*
    Colors
*/
---------Load Colors
IF OBJECT_ID('TempDb..#beersApiColor', 'U') IS NOT NULL
    DROP TABLE #beersApiColor
;

SELECT
    [UId],
    [Name]
INTO
    #beersApiColor
FROM
    dbo.[Color]
WHERE
    1 = 2
;

INSERT INTO #beersApiColor
VALUES
    (NEWID(), 'Black'),
    (NEWID(), 'Brown'),
    (NEWID(), 'Orange'),
    (NEWID(), 'Red'),
    (NEWID(), 'White'),
    (NEWID(), 'Blond')

MERGE [dbo].[Color] AS TARGET
USING #beersApiColor AS SOURCE
ON TARGET.[UId] = SOURCE.[UId]
WHEN MATCHED THEN
    UPDATE SET
        TARGET.[Name] = SOURCE.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [UId],
        [Name]
        ) VALUES (
            SOURCE.[UId],
            SOURCE.[Name]
        )
;

/*
    Countries
*/

-----------Load Countries
IF OBJECT_ID('TempDb..#beersApiCountry', 'U') IS NOT NULL
    DROP TABLE #beersApiCountry
;

SELECT
    [UId],
    [Name],
    [Code]
INTO
    #beersApiCountry
FROM
    dbo.[Country]
WHERE
    1 = 2
;

INSERT INTO #beersApiCountry
VALUES
    (NEWID(), 'Afghanistan', 'af'),
    (NEWID(), 'Aland Islands', 'ax'),
    (NEWID(), 'Albania', 'al'),
    (NEWID(), 'Algeria', 'dz'),
    (NEWID(), 'American Samoa', 'as'),
    (NEWID(), 'Andorra', 'ad'),
    (NEWID(), 'Angola', 'ao'),
    (NEWID(), 'Anguilla', 'ai'),
    (NEWID(), 'Antigua', 'ag'),
    (NEWID(), 'Argentina', 'ar'),    
	(NEWID(), 'Armenia', 'am'),
    (NEWID(), 'Aruba', 'aw'),
    (NEWID(), 'Australia', 'au'),
    (NEWID(), 'Austria', 'at'),
    (NEWID(), 'Azerbaijan', 'az'),
    (NEWID(), 'Bahamas', 'bs'),
    (NEWID(), 'Bahrain', 'bh'),
    (NEWID(), 'Bangladesh', 'bd'),
    (NEWID(), 'Barbados', 'bb'),
    (NEWID(), 'Belarus', 'by'),
    (NEWID(), 'Belgium', 'be'),
    (NEWID(), 'Belize', 'bz'),
    (NEWID(), 'Benin', 'bj'),
    (NEWID(), 'Bermuda', 'bm'),
    (NEWID(), 'Bhutan', 'bt'),
    (NEWID(), 'Bolivia', 'bo'),
    (NEWID(), 'Bosnia', 'ba'),
    (NEWID(), 'Botswana', 'bw'),
    (NEWID(), 'Bouvet Island', 'bv'),
    (NEWID(), 'Brazil', 'br'),
    (NEWID(), 'British Virgin Islands', 'vg'),
    (NEWID(), 'Brunei', 'bn'),
    (NEWID(), 'Bulgaria', 'bg'),
    (NEWID(), 'Burkina Faso', 'bf'),
    (NEWID(), 'Burma', 'mm'),
    (NEWID(), 'Burundi', 'bi'),
    (NEWID(), 'Caicos Islands', 'tc'),
    (NEWID(), 'Cambodia', 'kh'),
    (NEWID(), 'Cameroon', 'cm'),
    (NEWID(), 'Canada', 'ca'),
    (NEWID(), 'Cape Verde', 'cv'),
    (NEWID(), 'Cayman Islands', 'ky'),
    (NEWID(), 'Central African Republic', 'cf'),
    (NEWID(), 'Chad', 'td'),
    (NEWID(), 'Chile', 'cl'),
    (NEWID(), 'China', 'cn'),
    (NEWID(), 'Christmas Island', 'cx'),
    (NEWID(), 'Cocos Islands', 'cc'),
    (NEWID(), 'Colombia', 'co'),
    (NEWID(), 'Comoros', 'km'),
    (NEWID(), 'Congo', 'cd'),
    (NEWID(), 'Congo Brazzaville', 'cg'),
    (NEWID(), 'Cook Islands', 'ck'),
    (NEWID(), 'Costa Rica', 'cr'),
    (NEWID(), 'Cote Divoire', 'ci'),
    (NEWID(), 'Croatia', 'hr'),
    (NEWID(), 'Cuba', 'cu'),
    (NEWID(), 'Cyprus', 'cy'),
    (NEWID(), 'Czech Republic', 'cz'),
    (NEWID(), 'Denmark', 'dk'),
    (NEWID(), 'Djibouti', 'dj'),
    (NEWID(), 'Dominica', 'dm'),
    (NEWID(), 'Dominican Republic', 'do'),
    (NEWID(), 'Ecuador', 'ec'),
    (NEWID(), 'Egypt', 'eg'),
    (NEWID(), 'El Salvador', 'sv'),
    (NEWID(), 'Equatorial Guinea', 'gq'),
    (NEWID(), 'Eritrea', 'er'),
    (NEWID(), 'Estonia', 'ee'),
    (NEWID(), 'Ethiopia', 'et'),
    (NEWID(), 'Europeanunion', 'eu'),
    (NEWID(), 'Falkland Islands', 'fk'),
    (NEWID(), 'Faroe Islands', 'fo'),
    (NEWID(), 'Fiji', 'fj'),
    (NEWID(), 'Finland', 'fi'),
    (NEWID(), 'France', 'fr'),
    (NEWID(), 'French Guiana', 'gf'),
    (NEWID(), 'French Polynesia', 'pf'),
    (NEWID(), 'French Territories', 'tf'),
    (NEWID(), 'Gabon', 'ga'),
    (NEWID(), 'Gambia', 'gm'),
    (NEWID(), 'Georgia', 'ge'),
    (NEWID(), 'Germany', 'de'),
    (NEWID(), 'Ghana', 'gh'),
    (NEWID(), 'Gibraltar', 'gi'),
    (NEWID(), 'Greece', 'gr'),
    (NEWID(), 'Greenland', 'gl'),
    (NEWID(), 'Grenada', 'gd'),
    (NEWID(), 'Guadeloupe', 'gp'),
    (NEWID(), 'Guam', 'gu'),
    (NEWID(), 'Guatemala', 'gt'),
    (NEWID(), 'Guinea', 'gn'),
    (NEWID(), 'Guinea-Bissau', 'gw'),
    (NEWID(), 'Guyana', 'gy'),
    (NEWID(), 'Haiti', 'ht'),
    (NEWID(), 'Heard Island', 'hm'),
    (NEWID(), 'Honduras', 'hn'),
    (NEWID(), 'Hong Kong', 'hk'),
    (NEWID(), 'Hungary', 'hu'),
    (NEWID(), 'Iceland', 'is'),
    (NEWID(), 'India', 'in'),
    (NEWID(), 'Indian Ocean Territory', 'io'),
    (NEWID(), 'Indonesia', 'id'),
    (NEWID(), 'Iran', 'ir'),
    (NEWID(), 'Iraq', 'iq'),
    (NEWID(), 'Ireland', 'ie'),
    (NEWID(), 'Israel', 'il'),
    (NEWID(), 'Italy', 'it'),
    (NEWID(), 'Jamaica', 'jm'),
    (NEWID(), 'Jan Mayen', 'sj'),
    (NEWID(), 'Japan', 'jp'),
    (NEWID(), 'Jordan', 'jo'),
    (NEWID(), 'Kazakhstan', 'kz'),
    (NEWID(), 'Kenya', 'ke'),
    (NEWID(), 'Kiribati', 'ki'),
    (NEWID(), 'Kuwait', 'kw'),
    (NEWID(), 'Kyrgyzstan', 'kg'),
    (NEWID(), 'Laos', 'la'),
    (NEWID(), 'Latvia', 'lv'),
    (NEWID(), 'Lebanon', 'lb'),
    (NEWID(), 'Lesotho', 'ls'),
    (NEWID(), 'Liberia', 'lr'),
    (NEWID(), 'Libya', 'ly'),
    (NEWID(), 'Liechtenstein', 'li'),
    (NEWID(), 'Lithuania', 'lt'),
    (NEWID(), 'Luxembourg', 'lu'),
    (NEWID(), 'Macau', 'mo'),
    (NEWID(), 'Macedonia', 'mk'),
    (NEWID(), 'Madagascar', 'mg'),
    (NEWID(), 'Malawi', 'mw'),
    (NEWID(), 'Malaysia', 'my'),
    (NEWID(), 'Maldives', 'mv'),
    (NEWID(), 'Mali', 'ml'),
    (NEWID(), 'Malta', 'mt'),
    (NEWID(), 'Marshall Islands', 'mh'),
    (NEWID(), 'Martinique', 'mq'),
    (NEWID(), 'Mauritania', 'mr'),
    (NEWID(), 'Mauritius', 'mu'),
    (NEWID(), 'Mayotte', 'yt'),
    (NEWID(), 'Mexico', 'mx'),
    (NEWID(), 'Micronesia', 'fm'),
    (NEWID(), 'Moldova', 'md'),
    (NEWID(), 'Monaco', 'mc'),
    (NEWID(), 'Mongolia', 'mn'),
    (NEWID(), 'Montenegro', 'me'),
    (NEWID(), 'Montserrat', 'ms'),
    (NEWID(), 'Morocco', 'ma'),
    (NEWID(), 'Mozambique', 'mz'),
    (NEWID(), 'Namibia', 'na'),
    (NEWID(), 'Nauru', 'nr'),
    (NEWID(), 'Nepal', 'np'),
    (NEWID(), 'Netherlands', 'nl'),
    (NEWID(), 'Netherlandsantilles', 'an'),
    (NEWID(), 'New Caledonia', 'nc'),
    (NEWID(), 'New Guinea', 'pg'),
    (NEWID(), 'New Zealand', 'nz'),
    (NEWID(), 'Nicaragua', 'ni'),
    (NEWID(), 'Niger', 'ne'),
    (NEWID(), 'Nigeria', 'ng'),
    (NEWID(), 'Niue', 'nu'),
    (NEWID(), 'Norfolk Island', 'nf'),
    (NEWID(), 'North Korea', 'kp'),
    (NEWID(), 'Northern Mariana Islands', 'mp'),
    (NEWID(), 'Norway', 'no'),
    (NEWID(), 'Oman', 'om'),
    (NEWID(), 'Pakistan', 'pk'),
    (NEWID(), 'Palau', 'pw'),
    (NEWID(), 'Palestine', 'ps'),
    (NEWID(), 'Panama', 'pa'),
    (NEWID(), 'Paraguay', 'py'),
    (NEWID(), 'Peru', 'pe'),
    (NEWID(), 'Philippines', 'ph'),
    (NEWID(), 'Pitcairn Islands', 'pn'),
    (NEWID(), 'Poland', 'pl'),
    (NEWID(), 'Portugal', 'pt'),
    (NEWID(), 'Puerto Rico', 'pr'),
    (NEWID(), 'Qatar', 'qa'),
    (NEWID(), 'Reunion', 're'),
    (NEWID(), 'Romania', 'ro'),
    (NEWID(), 'Russia', 'ru'),
    (NEWID(), 'Rwanda', 'rw'),
    (NEWID(), 'Saint Helena', 'sh'),
    (NEWID(), 'Saint Kitts and Nevis', 'kn'),
    (NEWID(), 'Saint Lucia', 'lc'),
    (NEWID(), 'Saint Pierre', 'pm'),
    (NEWID(), 'Saint Vincent', 'vc'),
    (NEWID(), 'Samoa', 'ws'),
    (NEWID(), 'San Marino', 'sm'),
    (NEWID(), 'Sandwich Islands', 'gs'),
    (NEWID(), 'Sao Tome', 'st'),
    (NEWID(), 'Saudi Arabia', 'sa'),
    (NEWID(), 'Scotland', 'gb sct'),
    (NEWID(), 'Senegal', 'sn'),
    (NEWID(), 'Serbia', 'cs'),
    (NEWID(), 'Serbia', 'rs'),
    (NEWID(), 'Seychelles', 'sc'),
    (NEWID(), 'Sierra Leone', 'sl'),
    (NEWID(), 'Singapore', 'sg'),
    (NEWID(), 'Slovakia', 'sk'),
    (NEWID(), 'Slovenia', 'si'),
    (NEWID(), 'Solomon Islands', 'sb'),
    (NEWID(), 'Somalia', 'so'),
    (NEWID(), 'South Africa', 'za'),
    (NEWID(), 'South Korea', 'kr'),
    (NEWID(), 'Spain', 'es'),
    (NEWID(), 'Sri Lanka', 'lk'),
    (NEWID(), 'Sudan', 'sd'),
    (NEWID(), 'Suriname', 'sr'),
    (NEWID(), 'Swaziland', 'sz'),
    (NEWID(), 'Sweden', 'se'),
    (NEWID(), 'Switzerland', 'ch'),
    (NEWID(), 'Syria', 'sy'),
    (NEWID(), 'Taiwan', 'tw'),
    (NEWID(), 'Tajikistan', 'tj'),
    (NEWID(), 'Tanzania', 'tz'),
    (NEWID(), 'Thailand', 'th'),
    (NEWID(), 'Timorleste', 'tl'),
    (NEWID(), 'Togo', 'tg'),
    (NEWID(), 'Tokelau', 'tk'),
    (NEWID(), 'Tonga', 'to'),
    (NEWID(), 'Trinidad', 'tt'),
    (NEWID(), 'Tunisia', 'tn'),
    (NEWID(), 'Turkey', 'tr'),
    (NEWID(), 'Turkmenistan', 'tm'),
    (NEWID(), 'Tuvalu', 'tv'),
    (NEWID(), 'U.A.E.', 'ae'),
    (NEWID(), 'Uganda', 'ug'),
    (NEWID(), 'Ukraine', 'ua'),
    (NEWID(), 'United Kingdom', 'gb'),
    (NEWID(), 'United States', 'us'),
    (NEWID(), 'Uruguay', 'uy'),
    (NEWID(), 'US Minor Islands', 'um'),
    (NEWID(), 'US Virgin Islands', 'vi'),
    (NEWID(), 'Uzbekistan', 'uz'),
    (NEWID(), 'Vanuatu', 'vu'),
    (NEWID(), 'Vatican City', 'va'),
    (NEWID(), 'Venezuela', 've'),
    (NEWID(), 'Vietnam', 'vn'),
    (NEWID(), 'Wales', 'gb wls'),
    (NEWID(), 'Wallis and Futuna', 'wf'),
    (NEWID(), 'Western Sahara', 'eh'),
    (NEWID(), 'Yemen', 'ye'),
    (NEWID(), 'Zambia', 'zm'),
    (NEWID(), 'Zimbabwe', 'zw')

MERGE [dbo].[Country] AS TARGET
USING #beersApiCountry AS SOURCE
ON TARGET.[UId] = SOURCE.[UId]
WHEN MATCHED THEN
    UPDATE SET
        TARGET.[Name] = SOURCE.[Name],
        TARGET.[Code] = SOURCE.[Code]
WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [UId],
        [Name],
        [Code]
        ) VALUES (
            SOURCE.[UId],
            SOURCE.[Name],
            SOURCE.[Code]
        )
;

/*
    Flavours
*/

----------------Load Flavours
IF OBJECT_ID('TempDb..#beersApiFlavour', 'U') IS NOT NULL
    DROP TABLE #beersApiFlavour
;

SELECT
    [UId],
    [Name],
    [Description]
INTO
    #beersApiFlavour
FROM
    dbo.[Flavour]
WHERE
    1 = 2
;

INSERT INTO #beersApiFlavour
VALUES
    (NEWID(), 'Acidity', 'An appetizing acidity, sometimes lemony, comes from hops. A fruity acidity derives from the yeast in fermentation, especially in ales and even more so in Berliner Weisse, Belgian lambic styles, and Flemish brown and red ales.'),
    (NEWID(), 'Apples', 'A fresh, delicate, pleasant, dessert-apple character arises from the fermentation process in some English ales, famously Marstons. A more astringent, green-apple taste can arise from insufficient maturation.'),
    (NEWID(), 'Bananas', 'Very appropriate in some South German wheat beers.'),
    (NEWID(), 'Bitterness', 'Sounds negative, but it is positive. "Good" bitterness comes from the hop. It is present to varying degrees in all beers, and especially appropriate in a British bitter. Robust bitterness, as in Anchor Liberty Ale, is appetizing. Astringency is not.'),
    (NEWID(), 'Body', 'Not actually a taste, but a sensation of texture or \"mouth feel\", ranging from thin to firm to syrupy. Thinness may mean a beer has been very fully fermented, perhaps to create a light, quenching character. Firm, textured, or grainy beers have been mashed at high temperatures to create some unfermentable sugars. Syrupy ones have been made from high density of malts, possibly with some holding back of fermentation.'),
    (NEWID(), 'Bubble-gum', 'Very appropriate in some South German wheat beers. Arises from compounds called guaiacols created in fermentation.'),
    (NEWID(), 'Burnt', 'Pleasant burnt flavours arise from highly kilned barley or malt in some stouts. Burnt plastic, deriving from excessive phenol, is a defect caused by yeast problems.'),
    (NEWID(), 'Butterscotch', 'Very appropriate in certain British ales, especially some from the north of England and Scotland. Unpleasant in lagers. This flavour derives from a compound called diacetyl created in fermentation.'),
    (NEWID(), 'Caramel', 'Most often a malt characteristic, though brewers do sometimes also add caramel itself. A malty caramel character is positive in restrained form in many types of beers. Too much can be overwhelming.'),
    (NEWID(), 'Cedary', 'A hop character.'),
    (NEWID(), 'Chocolatey', 'A malt character in some brown ales, porters and stouts. Typically arising from chocolate malt.'),
    (NEWID(), 'Cloves', 'Very appropriate in some South German wheat beers. Arises from phenols created in fermentation.'),
    (NEWID(), 'Coffeeish', 'A malt character in some dark lagers, brown ales, porters, and stouts.'),
    (NEWID(), 'Cookie-like', 'Typical character of pale malt. Suggests a fresh beer with a good malt character.'),
    (NEWID(), 'Earthy', 'Typical character of traditional English hops. Positive characteristic in British ales.'),
    (NEWID(), 'Fresh bread', 'See Cookie-like.'),
    (NEWID(), 'Grapefruit', 'Typical character of American hops, especially the Cascade variety.'),
    (NEWID(), 'Grass/Hay', 'Can be a hop characteristic. Fresh, new-mown hat is typical in some classic European lagers. It arises from a compound called dimethyl sulphide, caused by fermentation with traditional lager yeasts.'),
    (NEWID(), 'Herbal', 'Hop characteristic. Examples of herbal flavours are bay leaves, mint and spearmint.'),
    (NEWID(), 'Hoppy', 'Herbal, zesty, earthy, cedary, piney, appetizingly bitter.'),
    (NEWID(), 'Licorice', 'A characteristic of some dark malts, in German Schwarzbier, English old ales, porters, and stouts. In the English-speaking world, licorice itself is sometimes use as an addictive.'),
    (NEWID(), 'Madeira', 'Caused by oxidation. In very strong bottle-conditioned beers that have been aged many years, this will be in a pleasant balance. In another type of beer, it is likely to be unpleasant.'),
    (NEWID(), 'Malty', 'See Cookie-like, fresh bread, nuts, tea, toast, and toffee.'),
    (NEWID(), 'Minty', 'Hop characteristic, especially spearmint.'),
    (NEWID(), 'Nuts', 'Typical malt characteristic in many types of beer, especially northern English brown ales. Arises from chrystal malt.'),
    (NEWID(), 'Orangey', 'Typical of several hop varieties. Can also arise from some ale yeasts. Positive if not overwhelming.'),
    (NEWID(), 'Pears', 'Yeast characteristic in some ales. If overwhelming, suggest that the beer has lost some balancing hop due to age.'),
    (NEWID(), 'Pepper', 'The flavour of alcohol. Suggest a strong beer.'),
    (NEWID(), 'Piney', 'Characteristic of some hops, especially American varieties.'),
    (NEWID(), 'Plums', 'Yeast character, found in South German wheat beers.'),
    (NEWID(), 'Raisiny', 'Typical in beers made with very dark malts and to a high alcohol content, for example, imperial stouts. This flavour develops in fermentation.'),
    (NEWID(), 'Resiny', 'Typical hop characteristic'),
    (NEWID(), 'Roses', 'Can arise from hops. Also from yeast development during bottle-conditioning, especially in some Belgian beers.'),
    (NEWID(), 'Sherry', 'Dry, fino sherry flavours are typical in Belgian lambic beers. Sweet sherry can arise in strong, bottle-conditioned beers that have been aged. See also Madeira.'),
    (NEWID(), 'Smoky', 'Appropriate in "malt whisky beer", smoked beer, and some dry stouts.'),
    (NEWID(), 'Sour', 'Appropriate in Berliner Weisse, lambic, or Flemish brown or red specialities, but not in other styles of beers.'),
    (NEWID(), 'Strawberries', 'In extremely restrained form, and in balance with malt and hop, an appropriate fermentation characteristic in some British ales.'),
    (NEWID(), 'Tea', 'A strongish tea, of the type made in England (Indian, especially Darjeeling, with milk) is a good aroma metaphor for malt.'),
    (NEWID(), 'Toast', 'Malt characteristic in some dark ales, porters, and stouts.'),
    (NEWID(), 'Tobacco', 'Fragrant tobacco smoke can be evoked by the Tettnang hop, grown near Lake Constance in Germany and used in many lagers.'),
    (NEWID(), 'Toffee', 'Malt characteristic, especially in Vienna-style, Marzen, and Oktoberfest amber lagers. Very appetizing if not overwhelming.'),
    (NEWID(), 'Vinegary', 'See Acidity.'),
    (NEWID(), 'Winey', 'Typical of lambic and some other Belgian styles aged in wood.'),
    (NEWID(), 'Yeast', 'The aroma of fresh yeast, like bread rising, is typical of some ales. Can appear as a \"bite\" in some from Yorkshire, England.')

MERGE [dbo].[Flavour] AS TARGET
USING #beersApiFlavour AS SOURCE
ON TARGET.[UId] = SOURCE.[UId]
WHEN MATCHED THEN
    UPDATE SET
        TARGET.[Name] = SOURCE.[Name],
        TARGET.[Description] = SOURCE.[Description]
WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [UId],
        [Name],
        [Description]
        ) VALUES (
            SOURCE.[UId],
            SOURCE.[Name],
            SOURCE.[Description]
        )
;

END

GO
