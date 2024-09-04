with allWords as (
	select 
		  b.Id as BookId
		, b.Title
		, w.Id as WordId
		, w.Status
	from Book b
	left join Page p on p.BookId = b.Id
	left join Paragraph pp on pp.PageId = p.Id
	left join Sentence s on s.ParagraphId = pp.id
	left join Token t on t.SentenceId = s.Id
	left join Word w on t.WordId = w.Id
	left join LanguageUser lu on b.LanguageUserId = lu.Id
	where lu.UserId = 1
) , allDistinctWordsByBook as (
	select 
		  b.Id as BookId
		, b.Title
		, w.Id as WordId
		, w.Status
	from Book b
	left join Page p on p.BookId = b.Id
	left join Paragraph pp on pp.PageId = p.Id
	left join Sentence s on s.ParagraphId = pp.id
	left join Token t on t.SentenceId = s.Id
	left join Word w on t.WordId = w.Id
	left join LanguageUser lu on b.LanguageUserId = lu.Id
	where lu.UserId = 1
	group by 
		  b.Id
		, b.Title
		, w.Id
		, w.Status
), distinctCounts as (
	select 
		  ad.BookId
		, ad.Title
		, ad.Status
		, count(ad.WordId) as DistinctCount
	from allDistinctWordsByBook ad
	group by 
		ad.BookId
		, ad.Title
		, ad.Status
)
, totalCounts as (
	select 
		  aw.BookId
		, aw.Title
		, aw.Status
		, count(aw.WordId) as TotalCount
	from allWords aw
	group by 
		  aw.BookId
		, aw.Title
		, aw.Status
)
select 
	  dc.BookId
	, dc.Title
	, dc.Status
	, dc.DistinctCount
	, tc.TotalCount
from distinctCounts dc
left join totalCounts tc on dc.BookId = tc.BookId and dc.Status = tc.Status
order by dc.BookId, dc.Status
;


-- BookId	Key	Value
--14	TOTALWELLKNOWNCOUNT	2270
--BookId	Key	Value
--14	DISTINCTWELLKNOWNCOUNT	715