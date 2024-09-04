create view Idioma.vw_BookUserStat as 

with params as (
	select 
	  AvailableWordUserStatusUNKNOWN = 8
	, AvailableWordUserStatusLEARNED = 5
	, AvailableWordUserStatusIGNORED = 6
	, AvailableWordUserStatusWELLKNOWN = 7
	, AvailableBookUserStatISCOMPLETE = 1
	, AvailableBookUserStatLASTPAGEREAD = 2
	, AvailableBookUserStatPROGRESS = 3
	, AvailableBookUserStatPROGRESSPERCENT = 4
	, AvailableBookUserStatDISTINCTKNOWNPERCENT = 5
	, AvailableBookUserStatTOTALKNOWNPERCENT = 6
	, AvailableBookUserStatDISTINCTWORDCOUNT = 7
	, AvailableBookUserStatTOTALWORDCOUNT = 8
), AllWords as (
	select 
		  lu.Id as LanguageUserId
		, b.Id as BookId
		, b.Title as BookTitle
		, p.Ordinal as PageNumber
		, pu.ReadDate as PageReadDate
		, pp.Ordinal as ParagraphNumber
		, s.Ordinal as SentenceNumber
		, t.Ordinal as TokenNumber
		, w.TextLowerCase
		, case when wu.[Status] is null then params.AvailableWordUserStatusUNKNOWN else wu.[Status] end as WordUserStatus
	from Idioma.Book b
	join params on b.Id = b.Id
	left join Idioma.Page p on p.BookId = b.Id
	left join Idioma.Paragraph pp on pp.PageId = p.Id
	left join Idioma.Sentence s on s.ParagraphId = pp.Id
	left join Idioma.Token t on t.SentenceId = s.Id
	left join Idioma.Word w on w.Id = t.WordId
	left join Idioma.LanguageUser lu on lu.LanguageId = b.LanguageId
	left join Idioma.BookUser bu on bu.BookId = b.Id and bu.LanguageUserId = lu.Id
	left join Idioma.PageUser pu on pu.PageId = p.Id and pu.BookUserId = bu.Id
	left join Idioma.WordUser wu on wu.WordId = w.Id and wu.LanguageUserId = lu.Id
), AllBooksByLanguageUser as (
	select 
		  lu.Id as LanguageUserId
		, b.Id as BookId
		, b.Title as BookTitle
	from Idioma.Book b
	left join Idioma.LanguageUser lu on b.LanguageId = lu.LanguageId
), PageStats as (
	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, abblu.BookTitle
		, p.Ordinal as PageNumber
		, case when pu.ReadDate is null then 0 else p.Ordinal end as ReadPageNumber
		--, ROW_NUMBER() over (partition by abblu.LanguageUserId, abblu.BookId order by p.Ordinal desc) as RowNumber
	from AllBooksByLanguageUser abblu
	left join Idioma.BookUser bu on abblu.BookId = bu.BookId and bu.LanguageUserId = abblu.LanguageUserId
	left join Idioma.Page p on p.BookId = abblu.BookId
	left join Idioma.PageUser pu on pu.PageId = p.Id and pu.BookUserId = bu.Id
), LastPageRead as (
	select 
		  LanguageUserId
		, BookId
		, max(ReadPageNumber) as LastPageRead 
	from PageStats
	group by LanguageUserId, BookId
), TotalPages as (
	select 
		  LanguageUserId
		, BookId
		, max(PageNumber) as TotalPages
	from PageStats
	group by LanguageUserId, BookId

), BookStats as (
	select
		  lpr.LanguageUserId
		, lpr.BookId
		, lpr.LastPageRead
		, tp.TotalPages
	from LastPageRead lpr
	left join TotalPages tp on lpr.BookId = tp.BookId and tp.LanguageUserId = lpr.LanguageUserId
),DistinctWords as (
	select 
		  LanguageUserId
		, BookId
		, TextLowerCase
		, WordUserStatus
	from AllWords
	group by LanguageUserId, BookId, TextLowerCase, WordUserStatus
), DistinctCountByStatus as (
	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, WordUserStatus
		, count(*) as CountAtStatus
	from AllBooksByLanguageUser abblu
	left join DistinctWords dw on abblu.LanguageUserId = dw.LanguageUserId and dw.BookId = abblu.BookId
	group by 
		  abblu.LanguageUserId
		, abblu.BookId
		, WordUserStatus
), TotalCountByStatus as (
	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, WordUserStatus
		, count(*) as CountAtStatus
	from AllBooksByLanguageUser abblu
	left join AllWords aw on abblu.LanguageUserId = aw.LanguageUserId and aw.BookId = abblu.BookId
	group by 
		  abblu.LanguageUserId
		, abblu.BookId
		, WordUserStatus
), KnownDistinct as (
	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, case when dcbs.BookId is null then 0 else sum(CountAtStatus) end as KnownCount
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join DistinctCountByStatus dcbs on abblu.LanguageUserId = dcbs.LanguageUserId and abblu.BookId = dcbs.BookId
	and WordUserStatus in (params.AvailableWordUserStatusLEARNED, params.AvailableWordUserStatusIGNORED, params.AvailableWordUserStatusWELLKNOWN)
	group by
		  abblu.LanguageUserId
		, abblu.BookId
		, dcbs.BookId
), KnownTotal as (
	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, case when tcbs.BookId is null then 0 else sum(CountAtStatus) end as KnownCount
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join TotalCountByStatus tcbs on abblu.LanguageUserId = tcbs.LanguageUserId and abblu.BookId = tcbs.BookId
	and WordUserStatus in (params.AvailableWordUserStatusLEARNED, params.AvailableWordUserStatusIGNORED, params.AvailableWordUserStatusWELLKNOWN)
	group by
		  abblu.LanguageUserId
		, abblu.BookId
		, tcbs.BookId
), DistinctCounts as (
	select  
		  kd.LanguageUserId
		, kd.BookId
		, kd.KnownCount
		, count(dw.TextLowerCase) as TotalCount
	from KnownDistinct kd
	left join DistinctWords dw on dw.LanguageUserId = kd.LanguageUserId and dw.BookId = kd.BookId
	group by
		  kd.LanguageUserId
		, kd.BookId
		, kd.KnownCount
), TotalCounts as (
	select  
		  kd.LanguageUserId
		, kd.BookId
		, kd.KnownCount
		, count(dw.TextLowerCase) as TotalCount
	from KnownTotal kd
	left join AllWords dw on dw.LanguageUserId = kd.LanguageUserId and dw.BookId = kd.BookId
	group by
		  kd.LanguageUserId
		, kd.BookId
		, kd.KnownCount
), KeyResults as (
	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, params.AvailableBookUserStatISCOMPLETE as [Key]
		, case when LastPageRead = TotalPages then 'true' else 'false' end as [Value]
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join BookStats bs on bs.LanguageUserId = abblu.LanguageUserId and bs.BookId = abblu.BookId

	union all 

	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, params.AvailableBookUserStatLASTPAGEREAD as [Key]
		, case when LastPageRead is null then '0' else cast(LastPageRead as nvarchar(250)) end as [Value]
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join BookStats bs on bs.LanguageUserId = abblu.LanguageUserId and bs.BookId = abblu.BookId

	union all 

	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, params.AvailableBookUserStatPROGRESS as [Key]
		, case when TotalPages is null then '0 / 0' else cast(LastPageRead as nvarchar(250)) + ' / ' + cast(TotalPages as nvarchar(250)) end as [Value]
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join BookStats bs on bs.LanguageUserId = abblu.LanguageUserId and bs.BookId = abblu.BookId

	union all 

	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, params.AvailableBookUserStatPROGRESSPERCENT as [Key]
		,case when TotalPages is null then '0'
			else
				cast(
				floor(
					cast(LastPageRead as numeric(7,0))  / cast(TotalPages as numeric(7,0))
					* 100
					)
				as nvarchar(250))
			end as [Value]
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join BookStats bs on bs.LanguageUserId = abblu.LanguageUserId and bs.BookId = abblu.BookId

	union all 

	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, params.AvailableBookUserStatDISTINCTKNOWNPERCENT as [Key]
		,case when TotalCount is null then '0'
			else
				cast(
				floor(
					cast(KnownCount as numeric(7,0))  / cast(TotalCount as numeric(7,0))
					* 100
					)
				as nvarchar(250))
			end as [Value]
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join DistinctCounts dc on dc.LanguageUserId = abblu.LanguageUserId and dc.BookId = abblu.BookId

	union all 

	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, params.AvailableBookUserStatTOTALKNOWNPERCENT as [Key]
		,case when TotalCount is null then '0'
			else
				cast(
				floor(
					cast(KnownCount as numeric(7,0))  / cast(TotalCount as numeric(7,0))
					* 100
					)
				as nvarchar(250))
			end as [Value]
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join TotalCounts tc on tc.LanguageUserId = abblu.LanguageUserId and tc.BookId = abblu.BookId

	union all 

	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, params.AvailableBookUserStatDISTINCTWORDCOUNT as [Key]
		,case when TotalCount is null then '0'
			else
				cast(TotalCount as nvarchar(250))
			end as [Value]
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join DistinctCounts dc on dc.LanguageUserId = abblu.LanguageUserId and dc.BookId = abblu.BookId

	union all 

	select 
		  abblu.LanguageUserId
		, abblu.BookId
		, params.AvailableBookUserStatTOTALWORDCOUNT as [Key]
		,case when TotalCount is null then '0'
			else
				cast(TotalCount as nvarchar(250))
			end as [Value]
	from AllBooksByLanguageUser abblu
	join params on abblu.BookId = abblu.BookId
	left join TotalCounts tc on tc.LanguageUserId = abblu.LanguageUserId and tc.BookId = abblu.BookId
)

select * from KeyResults




