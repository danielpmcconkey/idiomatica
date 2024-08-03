SELECT TOP (1000) 
	  1 as One
	, fc.Id as FlashCardId
	, fc.NextReview as FlashCardNextReview
	, fc.Status as FlashCardStatus
	, fc.WordUserId as WordUserId
	, w.Text
	, wu.Translation
	, pt.LanguageCode
	, pt.TranslationText as paragraphTranslationText
	, s.Text
FROM [Idioma].[FlashCard] fc
left join [Idiomatica].[Idioma].[FlashCardParagraphTranslationBridge] fcptb on fc.Id = fcptb.FlashCardId
left join [Idioma].[ParagraphTranslation] pt on fcptb.ParagraphTranslationId = pt.Id
left join [Idioma].[WordUser] wu on fc.WordUserId = wu.Id
left join [Idioma].[Word] w on wu.WordId = w.Id
left join [Idioma].[Paragraph] pp on pt.ParagraphId = pp.Id
left join [Idioma].[Sentence] s on pp.Id = s.ParagraphId
left join [Idioma].[LanguageUser] lu on wu.LanguageUserId = lu.Id
where 1=1
--and fc.Id = 29
and lu.Id = 3516
--and pt.LanguageCode = 'EN-US'


select 
*
from [Idioma].[WordUser] wu
left join [Idioma].[Word] w on wu.WordId = w.Id
left join [Idioma].[FlashCard] fc on wu.Id = fc.WordUserId
where wu.LanguageUserId = 3516
and fc.Id is null
