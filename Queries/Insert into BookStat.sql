
with AllWords as (
	select 
		  b.Id as BookId
		, b.Title as BookTitle
		, p.Ordinal as PageNumber
		, pp.Ordinal as ParagraphNumber
		, s.Ordinal as SentenceNumber
		, t.Ordinal as TokenNumber
		, w.TextLowerCase
	from Idioma.Book b
	left join Idioma.Page p on p.BookId = b.Id
	left join Idioma.Paragraph pp on pp.PageId = p.Id
	left join Idioma.Sentence s on s.ParagraphId = pp.Id
	left join Idioma.Token t on t.SentenceId = s.Id
	left join Idioma.Word w on w.Id = t.WordId
), DistinctWords as (
	select 
		  BookId
		, TextLowerCase
	from AllWords
	group by BookId, TextLowerCase
), KeyResults as (
	select 
		  BookId
		, 1 as [Key] -- TOTALPAGES
		, max(PageNumber) as [Value]
	from AllWords
	group by BookId

	union all 

	select 
		BookId
		, 2 as [Key] -- TOTALWORDCOUNT
		, count(*) as [Value]
	from AllWords
	group by BookId

	union all 

	select 
		BookId
		, 3 as [Key] -- DISTINCTWORDCOUNT
		, count(*) as [Value]
	from DistinctWords
	group by BookId
)
insert into Idioma.BookStat (BookId, [Key], [Value])
select * from KeyResults
;


