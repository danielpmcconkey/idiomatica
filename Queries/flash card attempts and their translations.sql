select 
	  wr.Ordinal as WordRank
	, w.TextLowerCase
	, wt.PartOfSpeech
	, wt.Translation as FormalTranslation
	, wu.Translation as InformalTranslation
	, wu.Status as WordUserStatus
	, fca.AttemptedWhen
	, fca.Status as AttemptStatus
from [Idioma].[WordRank] wr
left join [Idioma].[Word] w on wr.WordId = w.Id
left join [Idioma].[WordTranslation] wt on w.Id = wt.WordId
left join [Idioma].[Language] l on w.LanguageId = l.Id
left join [Idioma].[LanguageUser] lu on l.Id = lu.LanguageId
left join [Idioma].[User] u on lu.UserId = u.Id
left join [Idioma].[WordUser] wu on w.Id = wu.WordId and lu.Id = wu.LanguageUserId
left join [Idioma].[FlashCard] fc on wu.Id = fc.WordUserId
left join [Idioma].[FlashCardAttempt] fca on fc.Id = fca.FlashCardId
where wr.Ordinal <= 1000
and l.Name = 'Spanish'
and u.Name = 'Dan McConkey'
order by wr.Ordinal