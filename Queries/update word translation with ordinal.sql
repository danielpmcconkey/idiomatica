update [Idiomatica].[Idioma].[WordTranslation] 
set Ordinal = ( 1001 - (
	  (case when PartOfSpeech = 2 then 0 else 1000 end)
	  + 
	  (case 
		when Translation like '%present subjunctive%' then 50
		when Translation like '%imperfect subjunctive%' then 40		
		when Translation like '%future subjunctive%' then 30
		when Translation like '%present%' then 100
		when Translation like '%preterite%' then 90
		when Translation like '%imperfect%' then 90
		when Translation like '%conditional%' then 70
		when Translation like '%future%' then 60
		when Translation like '%affirmative imperative%' then 20
		when Translation like '%negative imperative%' then 10
		else 0
		end )
	+ (case
		when Translation like '%"yo"%' then 9
		when Translation like '%"tú"%' then 8
		when Translation like '%"él"%' then 7
		when Translation like '%"ella"%' then 6
		when Translation like '%"usted"%' then 5
		when Translation like '%"nosotros"%' then 4
		when Translation like '%"vosotros"%' then 3
		when Translation like '%"ellos"%' then 2
		when Translation like '%"ellas"%' then 1
		when Translation like '%"ustedes"%' then 0
		else 0
		end)
	) )