alter table languages add column TotalWordsRead int default 0;
alter table books add column IsComplete tinyint default 0;
alter table books add column TotalPages int default 0;
alter table books add column LastPageRead int default 0;