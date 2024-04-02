drop table if exists user_data;
create table user_data (
	id serial not null primary key,
	name varchar not null
);

drop table if exists language_template;
CREATE TABLE language_template (
    id serial not null primary key,    
    name VARCHAR(40) NOT NULL ,
    dict_1_uri VARCHAR(200) NOT NULL  ,
    dict_2_uri VARCHAR(200) NULL  ,
    sentence_translation_uri VARCHAR(200) NULL  ,
    character_substitutions VARCHAR(500) NOT NULL  ,
    regexp_split_sentences VARCHAR(500) NOT NULL  ,
    exceptions_split_sentences VARCHAR(500) NOT NULL  ,
    regexp_word_characters VARCHAR(500) NOT NULL  ,
    remove_spaces bool not null  ,
    split_each_char bool not null  ,
    right_to_left bool not null  ,
    show_romanization bool not null default false,
    parser_type varchar(20) not null default 'spacedel' 
);
drop table if exists language_user;
CREATE TABLE language (
    id serial not null primary key,
    language_template_id int not null,
    user_data_id INTEGER NOT NULL,
    name VARCHAR(40) NOT NULL ,
    dict_1_uri VARCHAR(200) NOT NULL  ,
    dict_2_uri VARCHAR(200) NULL  ,
    sentence_translation_uri VARCHAR(200) NULL  ,
    character_substitutions VARCHAR(500) NOT NULL  ,
    regexp_split_sentences VARCHAR(500) NOT NULL  ,
    exceptions_split_sentences VARCHAR(500) NOT NULL  ,
    regexp_word_characters VARCHAR(500) NOT NULL  ,
    remove_spaces bool not null  ,
    split_each_char bool not null  ,
    right_to_left bool not null  ,
    show_romanization bool not null default false,
    parser_type varchar(20) not null default 'spacedel' ,  
    constraint fk_language_user_language_template
      foreign key(language_template_id) 
        references user_data(id),  
    constraint fk_language_user_data
      foreign key(user_data_id) 
        references language_template(id)
);