alter table bookstats add column totalwordCount int;
alter table bookstats add column totalunknownCount  int;
alter table bookstats add column totalnew1Count  int;
alter table bookstats add column totalnew2Count  int;
alter table bookstats add column totallearning3Count  int;
alter table bookstats add column totallearning4Count  int;
alter table bookstats add column totallearnedCount  int;
alter table bookstats add column totalwellknownCount  int;
alter table bookstats add column totalignoredCount  int;
alter table bookstats add column totalknownPercent int;
alter table bookstats add column distinctwordCount int;
alter table bookstats add column distinctunknownCount  int;
alter table bookstats add column distinctnew1Count  int;
alter table bookstats add column distinctnew2Count  int;
alter table bookstats add column distinctlearning3Count  int;
alter table bookstats add column distinctlearning4Count  int;
alter table bookstats add column distinctlearnedCount  int;
alter table bookstats add column distinctwellknownCount  int;
alter table bookstats add column distinctignoredCount  int;
alter table bookstats add column distinctknownPercent int;

create table User (Id int not null primary key, Name varchar(250) );
insert into User (Id, Name) values (1,'Dan');

CREATE TABLE languages_new (
    LgID INTEGER NOT NULL  ,
    UserId INTEGER NOT NULL,
    LgName VARCHAR(40) NOT NULL  ,
    LgDict1URI VARCHAR(200) NOT NULL  ,
    LgDict2URI VARCHAR(200) NULL  ,
    LgGoogleTranslateURI VARCHAR(200) NULL  ,
    LgCharacterSubstitutions VARCHAR(500) NOT NULL  ,
    LgRegexpSplitSentences VARCHAR(500) NOT NULL  ,
    LgExceptionsSplitSentences VARCHAR(500) NOT NULL  ,
    LgRegexpWordCharacters VARCHAR(500) NOT NULL  ,
    LgRemoveSpaces TINYINT NOT NULL  ,
    LgSplitEachChar TINYINT NOT NULL  ,
    LgRightToLeft TINYINT NOT NULL  ,
    LgShowRomanization TINYINT NOT NULL DEFAULT 0 ,
    LgParserType VARCHAR(20) NOT NULL DEFAULT 'spacedel' ,
    PRIMARY KEY (LgID),
    FOREIGN KEY(UserId) REFERENCES User (Id) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into languages_new (LgID, UserId, LgName, LgDict1URI, LgDict2URI, 
        LgGoogleTranslateURI, LgCharacterSubstitutions, LgRegexpSplitSentences, 
        LgExceptionsSplitSentences, LgRegexpWordCharacters, LgRemoveSpaces, 
        LgSplitEachChar, LgRightToLeft, LgShowRomanization, LgParserType)
    select LgID, 1, LgName, LgDict1URI, LgDict2URI, 
        LgGoogleTranslateURI, LgCharacterSubstitutions, LgRegexpSplitSentences, 
        LgExceptionsSplitSentences, LgRegexpWordCharacters, LgRemoveSpaces, 
        LgSplitEachChar, LgRightToLeft, LgShowRomanization, LgParserType
    from languages;

CREATE TABLE books_new (
    BkID INTEGER NOT NULL  ,
    UserId INTEGER NOT NULL,
    BkLgID INTEGER NOT NULL  ,
    BkTitle VARCHAR(200) NOT NULL  ,
    BkSourceURI VARCHAR(1000) NULL  ,
    BkArchived TINYINT NOT NULL DEFAULT 0 ,
    BkCurrentTxID INTEGER NOT NULL DEFAULT 0 , 
    BkWordCount INT, 
    BkAudioFilename TEXT NULL, 
    BkAudioCurrentPos REAL NULL, 
    BkAudioBookmarks TEXT NULL,
    PRIMARY KEY (BkID),
    FOREIGN KEY(BkLgID) REFERENCES languages_new (LgID) ON UPDATE NO ACTION ON DELETE CASCADE,
    FOREIGN KEY(UserId) REFERENCES User (Id) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into books_new (UserId, BkID,BkLgID,BkTitle,BkSourceURI,BkArchived,BkCurrentTxID,
        BkAudioFilename,BkAudioCurrentPos,BkAudioBookmarks) 
    select 1,BkID,BkLgID,BkTitle,BkSourceURI,BkArchived,BkCurrentTxID,
        BkAudioFilename,BkAudioCurrentPos,BkAudioBookmarks 
    from books;

CREATE TABLE words_new (
    WoID INTEGER NOT NULL PRIMARY KEY ,
    UserId INTEGER NOT NULL,
    WoLgID INTEGER NOT NULL  ,
    WoText VARCHAR(250) NOT NULL  ,
    WoTextLC VARCHAR(250) NOT NULL  ,
    WoStatus TINYINT NOT NULL  ,
    WoTranslation VARCHAR(500) NULL  ,
    WoRomanization VARCHAR(100) NULL  ,
    WoTokenCount TINYINT NOT NULL DEFAULT 0 ,
    WoCreated DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ,
    WoStatusChanged DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY(WoLgID) REFERENCES languages_new (LgID) ON UPDATE NO ACTION ON DELETE CASCADE,
    FOREIGN KEY(UserId) REFERENCES User (Id) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into words_new(WoID,UserId,WoLgID,WoText,WoTextLC,WoStatus,WoTranslation,
        WoRomanization,WoTokenCount,WoCreated,WoStatusChanged)
    select WoID,1,WoLgID,WoText,WoTextLC,WoStatus,WoTranslation,
        WoRomanization,WoTokenCount,WoCreated,WoStatusChanged 
    from words;

CREATE TABLE tags2_new (
    T2ID INTEGER NOT NULL  ,
    T2Text VARCHAR(20) NOT NULL  ,
    T2Comment VARCHAR(200) NOT NULL DEFAULT '' ,
    PRIMARY KEY (T2ID)
);
insert into tags2_new(T2ID,T2Text,T2Comment) 
    select T2ID,T2Text,T2Comment 
    from tags2;

CREATE TABLE booktags_new (
    BtBkID INTEGER NOT NULL  ,
    BtT2ID INTEGER NOT NULL  ,
    PRIMARY KEY (BtBkID, BtT2ID),
    FOREIGN KEY(BtT2ID) REFERENCES tags2_new (T2ID) ON UPDATE NO ACTION ON DELETE CASCADE,
    FOREIGN KEY(BtBkID) REFERENCES books_new (BkID) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into booktags_new(BtBkID,BtT2ID) 
    select BtBkID,BtT2ID 
    from booktags;


CREATE TABLE tags_new (
    TgID INTEGER NOT NULL  ,
    TgText VARCHAR(20) NOT NULL  ,
    TgComment VARCHAR(200) NOT NULL DEFAULT '' ,
    PRIMARY KEY (TgID)
);
insert into tags_new(TgID,TgText,TgComment) 
    select TgID,TgText,TgComment 
    from tags;

CREATE TABLE wordtags_new (
    WtWoID INTEGER NOT NULL  ,
    WtTgID INTEGER NOT NULL  ,
    PRIMARY KEY (WtWoID, WtTgID),
    FOREIGN KEY(WtWoID) REFERENCES words_new (WoID) ON UPDATE NO ACTION ON DELETE CASCADE,
    FOREIGN KEY(WtTgID) REFERENCES tags_new (TgID) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into wordtags_new(WtWoID,WtTgID) 
    select WtWoID,WtTgID 
    from wordtags;

CREATE TABLE texts_new (
    TxID INTEGER NOT NULL  ,
    TxBkID INTEGER NOT NULL  ,
    TxOrder INTEGER NOT NULL  ,
    TxText TEXT NOT NULL  ,
    TxReadDate datetime null, TxWordCount INTEGER null,
    PRIMARY KEY (TxID),
    FOREIGN KEY(TxBkID) REFERENCES books_new (BkID) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into texts_new (TxID,TxBkID,TxOrder,TxText,TxReadDate) 
    select TxID,TxBkID,TxOrder,TxText,TxReadDate 
    from texts;

CREATE TABLE sentences_new (
    SeID INTEGER NOT NULL  ,
    SeTxID INTEGER NOT NULL  ,
    SeOrder SMALLINT NOT NULL  ,
    SeText TEXT NULL  ,
    PRIMARY KEY (SeID),
    FOREIGN KEY(SeTxID) REFERENCES texts_new (TxID) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into sentences_new(SeID,SeTxID,SeOrder,SeText) 
    select SeID,SeTxID,SeOrder,SeText 
    from sentences;

CREATE TABLE bookstats_new (
    BkID INTEGER NOT NULL  ,
    wordcount INTEGER NULL  ,
    distinctterms INTEGER NULL  ,
    distinctunknowns INTEGER NULL  ,
    unknownpercent INTEGER NULL  , totalwordCount int, totalunknownCount  int, totalnew1Count  int, totalnew2Count  int, totallearning3Count  int, totallearning4Count  int, totallearnedCount  int, totalwellknownCount  int, totalignoredCount  int, totalknownPercent int, distinctwordCount int, distinctunknownCount  int, distinctnew1Count  int, distinctnew2Count  int, distinctlearning3Count  int, distinctlearning4Count  int, distinctlearnedCount  int, distinctwellknownCount  int, distinctignoredCount  int, distinctknownPercent int,
    PRIMARY KEY (BkID),
    FOREIGN KEY(BkID) REFERENCES books_new (BkID) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into bookstats_new (BkID,wordcount,distinctterms,distinctunknowns,unknownpercent,
        totalwordCount,totalunknownCount,totalnew1Count,totalnew2Count,totallearning3Count,
        totallearning4Count,totallearnedCount,totalwellknownCount,totalignoredCount,
        totalknownPercent,distinctwordCount,distinctunknownCount,distinctnew1Count,
        distinctnew2Count,distinctlearning3Count,distinctlearning4Count,distinctlearnedCount,
        distinctwellknownCount,distinctignoredCount,distinctknownPercent)
    select BkID,wordcount,distinctterms,distinctunknowns,unknownpercent,
        totalwordCount,totalunknownCount,totalnew1Count,totalnew2Count,totallearning3Count,
        totallearning4Count,totallearnedCount,totalwellknownCount,totalignoredCount,
        totalknownPercent,distinctwordCount,distinctunknownCount,distinctnew1Count,
        distinctnew2Count,distinctlearning3Count,distinctlearning4Count,distinctlearnedCount,
        distinctwellknownCount,distinctignoredCount,distinctknownPercent
    from bookstats;

CREATE TABLE wordparents_new (
    WpWoID INTEGER NOT NULL  ,
    WpParentWoID INTEGER NOT NULL  ,
    FOREIGN KEY(WpParentWoID) REFERENCES words_new (WoID) ON UPDATE NO ACTION ON DELETE CASCADE,
    FOREIGN KEY(WpWoID) REFERENCES words_new (WoID) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into wordparents_new(WpWoID,WpParentWoID) 
    select WpWoID,WpParentWoID 
    from wordparents;

CREATE TABLE settings_new (
    UserId INTEGER NOT NULL,
    StKey VARCHAR(40) NOT NULL,
    StKeyType TEXT NOT NULL,
    StValue TEXT NULL,
    PRIMARY KEY (UserId, StKey),
    FOREIGN KEY(UserId) REFERENCES User (Id) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into settings_new(StKey,UserId,StKeyType,StValue) 
    select StKey,1,StKeyType,StValue 
    from settings;

CREATE TABLE statuses_new (
    StID INTEGER NOT NULL  ,
    StText VARCHAR(20) NOT NULL  ,
    StAbbreviation VARCHAR(5) NOT NULL  ,
    PRIMARY KEY (StID)
);
insert into statuses_new(StID,StText,StAbbreviation) 
    select StID,StText,StAbbreviation 
    from statuses;




CREATE TABLE wordflashmessages_new (
  WfID INTEGER NOT NULL,
  WfWoID INTEGER NOT NULL,
  WfMessage VARCHAR(200) NOT NULL,
  PRIMARY KEY (WfID),
  FOREIGN KEY(WfWoID) REFERENCES words_new (WoID) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into wordflashmessages_new (WfID,WfWoID,WfMessage)
    select WfID,WfWoID,WfMessage 
    from wordflashmessages;

CREATE TABLE wordimages_new (
    WiID INTEGER NOT NULL  ,
    WiWoID INTEGER NOT NULL  ,
    WiSource VARCHAR(500) NOT NULL  ,
    PRIMARY KEY (WiID),
    FOREIGN KEY(WiWoID) REFERENCES words_new (WoID) ON UPDATE NO ACTION ON DELETE CASCADE
);
insert into wordimages_new(WiID,WiWoID,WiSource) 
    select WiID,WiWoID,WiSource 
    from wordimages;




ALTER TABLE languages RENAME TO languages_old;
ALTER TABLE books RENAME TO books_old;
ALTER TABLE words RENAME TO words_old;
ALTER TABLE booktags RENAME TO booktags_old;
ALTER TABLE wordtags RENAME TO wordtags_old;
ALTER TABLE texts RENAME TO texts_old;
ALTER TABLE sentences RENAME TO sentences_old;
ALTER TABLE bookstats RENAME TO bookstats_old;
ALTER TABLE wordparents RENAME TO wordparents_old;
ALTER TABLE settings RENAME TO settings_old;
ALTER TABLE statuses RENAME TO statuses_old;
ALTER TABLE tags RENAME TO tags_old;
ALTER TABLE tags2 RENAME TO tags2_old;
ALTER TABLE wordflashmessages RENAME TO wordflashmessages_old;
ALTER TABLE wordimages RENAME TO wordimages_old;

ALTER TABLE languages_new RENAME TO languages;
ALTER TABLE books_new RENAME TO books;
ALTER TABLE words_new RENAME TO words;
ALTER TABLE booktags_new RENAME TO booktags;
ALTER TABLE wordtags_new RENAME TO wordtags;
ALTER TABLE texts_new RENAME TO texts;
ALTER TABLE sentences_new RENAME TO sentences;
ALTER TABLE bookstats_new RENAME TO bookstats;
ALTER TABLE wordparents_new RENAME TO wordparents;
ALTER TABLE settings_new RENAME TO settings;
ALTER TABLE statuses_new RENAME TO statuses;
ALTER TABLE tags_new RENAME TO tags;
ALTER TABLE tags2_new RENAME TO tags2;
ALTER TABLE wordflashmessages_new RENAME TO wordflashmessages;
ALTER TABLE wordimages_new RENAME TO wordimages;













