SELECT *
FROM Girls

SELECT Id, [Name], MangaId, CreatorId
FROM Girls

SELECT [Id] as 'Key', [Name] 'Not a Name', [MangaId], [CreatorId]
FROM [Girls]

UPDATE [Girls]
SET CreatorId = 3
WHERE Id = 2005

SELECT *
FROM Girls
WHERE MangaId IS NOT NULL 
	AND CreatorId IS NULL

SELECT 
	G.[Name] as Girl
	, M.Title Manga
	, U.[Login] [User]
FROM Girls G
	LEFT JOIN Mangas M ON G.MangaId = M.Id
	LEFT JOIN Users U ON U.Id = M.AuthorId
	
SELECT MIN(Id) OriginId, ImageSrc, COUNT(*)
FROM Girls
GROUP BY ImageSrc
HAVING COUNT(*) > 1

SELECT G.Id
	,G.Name
	,G.ImageSrc
	,CASE WHEN OriginId IS NULL THEN 'Uniq' ELSE 'NotUniq' END as UniqStatus
	,CASE WHEN OriginId IS NULL OR OriginId = G.Id THEN 'Original' ELSE 'Duplicate' END as DuplicateStatus
	,CASE WHEN OriginId IS NOT NULL AND OriginId <> G.Id THEN OriginId ELSE NULL END as OriginId
	,CASE WHEN OriginId IS NOT NULL AND OriginId <> G.Id THEN G2.Name ELSE NULL END as OriginName
FROM Girls G
	LEFT JOIN (
		SELECT MIN(Id) OriginId, ImageSrc, COUNT(*) CountOfDuplication
		FROM Girls
		GROUP BY ImageSrc
		HAVING COUNT(*) > 1
	) DI ON G.ImageSrc = DI.ImageSrc
	LEFT JOIN Girls G2 ON DI.OriginId = G2.Id


DELETE Girls WHERE Id = 1000000

INSERT Girls (ImageSrc, Name, CreatorId, MangaId)
VALUES (
		'https://cdn.zbaseglobal.com/saasbox/resources/webp/anime-girl2__1be987e18df1c2c84cdff1d04406bf3a.webp'
		, 'GrilFromDb 2'
		, 1002
		, 1
	),
	(
		'https://cdn.zbaseglobal.com/saasbox/resources/webp/anime-girl2__1be987e18df1c2c84cdff1d04406bf3a.webp'
		, 'GrilFromDb 3'
		, 1002
		, 1
	),
	(
		'https://cdn.zbaseglobal.com/saasbox/resources/webp/anime-girl2__1be987e18df1c2c84cdff1d04406bf3a.webp'
		, 'GrilFromDb 4'
		, 1002
		, 1
	)
