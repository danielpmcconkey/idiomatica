
with AllTags as (
	select 
		  b.Id as BookId
		, bt.Tag
	from Idioma.Book b
	left join Idioma.BookTag bt on b.Id = bt.BookId
	group by b.Id, bt.Tag
), AllResults as (
	select
		  ROW_NUMBER() over (order by b.Id) as RowNumber
		, case when bu.Id is null or bu.IsArchived = 1 then cast(0 as bit) else cast (1 as bit) end as IsInShelf
		, bu.Id as BookUserId
		, lu.UserId
		, b.Id as BookId
		, l.[Name] as LanguageName
		, bus_ISCOMPLETE.ValueString as IsComplete
		, b.Title
		, cast(bsTotalPages.[Value] as int) as TotalPages
		, bus_PROGRESS.ValueString as Progress
		, bus_PROGRESSPERCENT.ValueNumeric as ProgressPercent
		, cast(bsTotalWordCount.[Value] as int) as TotalWordCount
		, cast(bsDistinctWordCount.[Value] as int) as DistinctWordCount
		, bus_DISTINCTKNOWNPERCENT.ValueNumeric as DistinctKnownPercent
		, bu.IsArchived
		, STRING_AGG(at.Tag, ',') AS Tags
	from Idioma.Book b
	left join  Idioma.Language l on b.LanguageId = l.Id
	left join Idioma.BookStat bsTotalPages on bsTotalPages.BookId = b.Id and bsTotalPages.[Key] = 1
	left join Idioma.BookStat bsTotalWordCount on bsTotalWordCount.BookId = b.Id and bsTotalWordCount.[Key] = 2
	left join Idioma.BookStat bsDistinctWordCount on bsDistinctWordCount.BookId = b.Id and bsDistinctWordCount.[Key] = 3
	left join AllTags at on at.BookId = b.Id
	left join  Idioma.LanguageUser lu on lu.LanguageId = b.LanguageId and lu.UserId = 1
	left join Idioma.BookUser bu on bu.BookId = b.Id and bu.LanguageUserId = lu.Id
	left join [Idioma].[BookUserStat] bus_ISCOMPLETE on bus_ISCOMPLETE.BookId = b.Id and bus_ISCOMPLETE.LanguageUserId = lu.Id and bus_ISCOMPLETE.[Key] = 1 --AvailableBookUserStat.ISCOMPLETE
	left join [Idioma].[BookUserStat] bus_PROGRESS on bus_PROGRESS.BookId = b.Id and bus_PROGRESS.LanguageUserId = lu.Id and bus_PROGRESS.[Key] = 3 --AvailableBookUserStat.PROGRESS
	left join [Idioma].[BookUserStat] bus_PROGRESSPERCENT on bus_PROGRESSPERCENT.BookId = b.Id and bus_PROGRESSPERCENT.LanguageUserId = lu.Id and bus_PROGRESSPERCENT.[Key] = 4 --AvailableBookUserStat.PROGRESSPERCENT
	left join [Idioma].[BookUserStat] bus_TOTALWORDCOUNT on bus_TOTALWORDCOUNT.BookId = b.Id and bus_TOTALWORDCOUNT.LanguageUserId = lu.Id and bus_TOTALWORDCOUNT.[Key] = 8 --AvailableBookUserStat.TOTALWORDCOUNT
	left join [Idioma].[BookUserStat] bus_TOTALKNOWNPERCENT on bus_TOTALKNOWNPERCENT.BookId = b.Id and bus_TOTALKNOWNPERCENT.LanguageUserId = lu.Id and bus_TOTALKNOWNPERCENT.[Key] = 6 --AvailableBookUserStat.TOTALKNOWNPERCENT
	left join [Idioma].[BookUserStat] bus_DISTINCTWORDCOUNT on bus_DISTINCTWORDCOUNT.BookId = b.Id and bus_DISTINCTWORDCOUNT.LanguageUserId = lu.Id and bus_DISTINCTWORDCOUNT.[Key] = 7 --AvailableBookUserStat.DISTINCTWORDCOUNT
	left join [Idioma].[BookUserStat] bus_DISTINCTKNOWNPERCENT on bus_DISTINCTKNOWNPERCENT.BookId = b.Id and bus_DISTINCTKNOWNPERCENT.LanguageUserId = lu.Id and bus_DISTINCTKNOWNPERCENT.[Key] = 5 --AvailableBookUserStat.DISTINCTKNOWNPERCENT
	where l.LanguageCode = 'EN-US'
	group by
		  b.Id
		, bu.Id
		, lu.UserId
		, l.[Name]
		, bus_ISCOMPLETE.ValueString
		, b.Title
		, bsTotalPages.[Value]
		, bus_PROGRESS.ValueString
		, bus_PROGRESSPERCENT.ValueNumeric
		, bsTotalWordCount.[Value]
		, bsDistinctWordCount.[Value]
		, bus_DISTINCTKNOWNPERCENT.ValueNumeric
		, bu.IsArchived
)
select * from AllResults
where RowNumber >= 5 
and RowNumber < 55 + 5




