insert into rola(nazwa) values ('administrator'), ('zwykły');
insert into slowko(eng,pl) values ('cat','kot'), ('dog','pies'), ('parrot','papuga'), ('elephant','słoń'), ('giraffe','żyrafa');
insert into ustawieniaZagadki(opis) values ('Z języka polskiego na angielski'), ('Z języka angielskiego na polski');
insert into uzytkownik(nazwa, haslo, rola, ustawienia) values ('admin','test', 1, 2), ('uzytkownik','1234', 2, 1);
insert into statystykaUzytkownika(nazwa) values ('admin'), ('uzytkownik');
