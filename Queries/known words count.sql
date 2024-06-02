select count(*)
from [Idioma].[WordUser]
where LanguageUserId = 1
and [Status] in (5,7)