SET IDENTITY_INSERT Activiteit ON

INSERT INTO Activiteit (Id, Naam, Beschrijving)
VALUES(1, 'Fietsen', 'Een toertje fietsen door de bossen.')
INSERT INTO Activiteit (Id, Naam, Beschrijving)
VALUES(2, 'Schaatsen', 'Een leuke namiddag op de schaatsbaan met muziek.')
INSERT INTO Activiteit (Id, Naam, Beschrijving)
VALUES(3, 'Disco-bowling', 'Een avondje bowlen met discolichten en sfeermuziek.')
INSERT INTO Activiteit (Id, Naam, Beschrijving)
VALUES(4, 'Spelletjes', 'Een namiddag gezelschapsspelletjes als het weer niet mee zit.')
INSERT INTO Activiteit (Id, Naam, Beschrijving)
VALUES(5, 'Padel', 'Een padel toernooi om de competitiebeesten bezig te houden.')
INSERT INTO Activiteit (Id, Naam, Beschrijving)
VALUES(6, 'Danslessen', 'Voor personen van elk dansniveau en genre. Hier kunnen ze ontpoppen tot echte dancing queens.')
INSERT INTO Activiteit (Id, Naam, Beschrijving)
VALUES(7, 'Museum bezoek', 'Op bezoek naar een museum om de lokale geschiedenis te verkennen')
INSERT INTO Activiteit (Id, Naam, Beschrijving)
VALUES(8, 'Skydiven', 'Voor de echte adrenaline junkies is er natuurlijk ook nog deze activiteit')

SET IDENTITY_INSERT Activiteit OFF

SET IDENTITY_INSERT Bestemming ON

INSERT INTO Bestemming (Id, Beschrijving, Code, MaxLeeftijd, MinLeeftijd, Naam)
VALUES 
(1, 'Een prachtige bestemming met schilderachtige landschappen en rustige stranden.', 'DEST-001', 60, 18, 'Maldiven');

INSERT INTO Bestemming (Id, Beschrijving, Code, MaxLeeftijd, MinLeeftijd, Naam)
VALUES 
(2, 'Een historische stad vol cultuur, musea en adembenemende architectuur.', 'DEST-002', 70, 16, 'Rome');

INSERT INTO Bestemming (Id, Beschrijving, Code, MaxLeeftijd, MinLeeftijd, Naam)
VALUES 
(3, 'Een avontuurlijke reis naar de bergen met wandelroutes en skiën.', 'DEST-003', 50, 10, 'Alpen');

INSERT INTO Bestemming (Id, Beschrijving, Code, MaxLeeftijd, MinLeeftijd, Naam)
VALUES 
(4, 'Ontdek de schoonheid van tropische regenwouden en kleurrijke fauna.', 'DEST-004', 55, 12, 'Amazone');

INSERT INTO Bestemming (Id, Beschrijving, Code, MaxLeeftijd, MinLeeftijd, Naam)
VALUES 
(5, 'Een levendige stad met een bruisend nachtleven en iconische attracties.', 'DEST-005', 40, 18, 'New York');

INSERT INTO Bestemming (Id, Beschrijving, Code, MaxLeeftijd, MinLeeftijd, Naam)
VALUES 
(6, 'Een serene bestemming met prachtige meren en pittoreske dorpjes.', 'DEST-006', 80, 10, 'Zwitserse Alpen');

INSERT INTO Bestemming (Id, Beschrijving, Code, MaxLeeftijd, MinLeeftijd, Naam)
VALUES 
(7, 'Ervaar de magie van een woestijnavontuur met kamelenritten en sterrenhemel.', 'DEST-007', 65, 15, 'Sahara');

INSERT INTO Bestemming (Id, Beschrijving, Code, MaxLeeftijd, MinLeeftijd, Naam)
VALUES 
(8, 'Een exotisch eiland met helderblauw water en luxe resorts.', 'DEST-008', 50, 18, 'Bali');

SET IDENTITY_INSERT Bestemming OFF

SET IDENTITY_INSERT Groepreis ON

INSERT INTO Groepreis (Id, BestemmingId, Begindatum, Einddatum, Prijs)
VALUES (1, 1, '2024-12-01', '2024-12-10', 2500.00);

INSERT INTO Groepreis (Id, BestemmingId, Begindatum, Einddatum, Prijs)
VALUES (2, 2, '2024-11-15', '2024-11-22', 1800.00);

INSERT INTO Groepreis (Id, BestemmingId, Begindatum, Einddatum, Prijs)
VALUES (3, 3, '2025-01-05', '2025-01-12', 2200.00);

INSERT INTO Groepreis (Id, BestemmingId, Begindatum, Einddatum, Prijs)
VALUES (4, 4, '2024-10-10', '2024-10-20', 3500.00);

INSERT INTO Groepreis (Id, BestemmingId, Begindatum, Einddatum, Prijs)
VALUES (5, 5, '2024-07-01', '2024-07-14', 1500.00);

INSERT INTO Groepreis (Id, BestemmingId, Begindatum, Einddatum, Prijs)
VALUES (6, 6, '2024-08-15', '2024-08-25', 2700.00);

INSERT INTO Groepreis (Id, BestemmingId, Begindatum, Einddatum, Prijs)
VALUES (7, 7, '2024-09-01', '2024-09-10', 3100.00);

INSERT INTO Groepreis (Id, BestemmingId, Begindatum, Einddatum, Prijs)
VALUES (8, 8, '2024-11-01', '2024-11-08', 2000.00);

SET IDENTITY_INSERT Groepreis OFF

SET IDENTITY_INSERT Onkost ON

INSERT INTO Onkost (Id, Titel, Omschrijving, Bedrag, Datum, Foto, GroepsreisId)
VALUES (1, 'Hotelkosten', 'Betaling voor accommodatie tijdens groepsreis.', 1200.00, '2024-12-01', NULL, 1);

INSERT INTO Onkost (Id, Titel, Omschrijving, Bedrag, Datum, Foto, GroepsreisId)
VALUES (2, 'Vervoer', 'Busvervoer voor deelnemers naar de bestemming.', 800.00, '2024-11-15', NULL, 2);

INSERT INTO Onkost (Id, Titel, Omschrijving, Bedrag, Datum, Foto, GroepsreisId)
VALUES (3, 'Eten en drinken', 'Maaltijdkosten tijdens de groepsreis.', 450.00, '2024-11-18', NULL, 3);

INSERT INTO Onkost (Id, Titel, Omschrijving, Bedrag, Datum, Foto, GroepsreisId)
VALUES (4, 'Toegangskaarten', 'Kosten voor toegangskaarten voor activiteiten.', 300.00, '2024-12-05', NULL, 4);

INSERT INTO Onkost (Id, Titel, Omschrijving, Bedrag, Datum, Foto, GroepsreisId)
VALUES (5, 'Marketing', 'Advertentiekosten om de groepsreis te promoten.', 1500.00, '2024-10-01', NULL, 5);

INSERT INTO Onkost (Id, Titel, Omschrijving, Bedrag, Datum, Foto, GroepsreisId)
VALUES (6, 'Gidskosten', 'Kosten voor een lokale gids tijdens de excursies.', 700.00, '2024-12-02', NULL, 6);

INSERT INTO Onkost (Id, Titel, Omschrijving, Bedrag, Datum, Foto, GroepsreisId)
VALUES (7, 'Verzekering', 'Groepsverzekering voor alle deelnemers.', 400.00, '2024-11-01', NULL, 7);

INSERT INTO Onkost (Id, Titel, Omschrijving, Bedrag, Datum, Foto, GroepsreisId)
VALUES (8, 'Overige uitgaven', 'Onvoorziene uitgaven tijdens de groepsreis.', 250.00, '2024-12-09', NULL, 8);

SET IDENTITY_INSERT Onkost OFF

SET IDENTITY_INSERT Programma ON

INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (1, 2, 1);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (2, 3, 2);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (3, 4, 3);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (4, 5, 4);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (5, 6, 5);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (6, 7, 6);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (7, 8, 7);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (8, 7, 8);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (9, 2, 2);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (10, 3, 3);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (11, 4, 4);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (12, 5, 5);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (13, 6, 6);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (14, 7, 7);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (15, 8, 8);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (16, 7, 1);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (17, 2, 3);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (18, 3, 4);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (19, 4, 5);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (20, 5, 6);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (21, 6, 7);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (22, 7, 8);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (23, 8, 1);
INSERT INTO Programma (Id, ActiviteitId, GroepsreisId) VALUES (24, 7, 2);

SET IDENTITY_INSERT Programma OFF

SET IDENTITY_INSERT AspNetUsers ON

INSERT INTO AspNetUsers(Id, Discriminator, Naam, Voornaam, Straat, Huisnummer, Gemeente, Postcode, GeboorteDatum, Huisdokter, ContractNummer, IsMonitor, TelefoonNummer, RekeningNummer, IsActief, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES 
(1, 'ApplicationUs', 'Jansen', 'Emma', 'Kerkstraat', '12', 'Antwerpen', '2000', '1990-05-15', 'Dr. Peeters', 'CN123456', 0, '0489123456', 'BE12345678901234', 1, 'emma.jansen', 'EMMA.JANSEN', 'emma.jansen@example.com', 'EMMA.JANSEN@EXAMPLE.COM', 1, 'hashed_password1', 'security_stamp1', 'concurrency_stamp1', '0489123456', 1, 0, NULL, 1, 0),
(2, 'ApplicationUs', 'De Smet', 'Liam', 'Dorpstraat', '45', 'Gent', '9000', '1985-10-20', 'Dr. Janssens', 'CN654321', 1, '0478123456', 'BE43210987654321', 1, 'liam.desmet', 'LIAM.DESMET', 'liam.desmet@example.com', 'LIAM.DESMET@EXAMPLE.COM', 1, 'hashed_password2', 'security_stamp2', 'concurrency_stamp2', '0478123456', 1, 0, NULL, 1, 0),
(3, 'ApplicationUs', 'Peeters', 'Sofie', 'Stationsstraat', '88', 'Brugge', '8000', '1995-03-10', 'Dr. Verstraete', 'CN987654', 0, '0498123456', 'BE09876543210987', 1, 'sofie.peeters', 'SOFIE.PEETERS', 'sofie.peeters@example.com', 'SOFIE.PEETERS@EXAMPLE.COM', 1, 'hashed_password3', 'security_stamp3', 'concurrency_stamp3', '0498123456', 1, 0, NULL, 1, 0),
(4, 'ApplicationUs', 'Van der Berg', 'Noah', 'Lindenlaan', '3', 'Leuven', '3000', '2000-07-25', 'Dr. De Vries', 'CN456789', 1, '0468123456', 'BE45678901234567', 1, 'noah.vanderberg', 'NOAH.VANDERBERG', 'noah.vanderberg@example.com', 'NOAH.VANDERBERG@EXAMPLE.COM', 1, 'hashed_password4', 'security_stamp4', 'concurrency_stamp4', '0468123456', 1, 0, NULL, 1, 0);

SET IDENTITY_INSERT AspNetUsers OFF

SET IDENTITY_INSERT Monitor ON

INSERT INTO Monitor(Id, GebruikerId, GroepsreisDetailsId, IsHoofdMonitor)
VALUES
(1, 2, 5, 0),
(2, 4, 5, 1),
(3, 2, 7, 1),
(4, 2, 8, 0),
(5, 4, 1, 0),
(6, 4, 3, 0),
(7, 2, 2, 0),
(8, 4, 2, 1)

SET IDENTITY_INSERT Monitor OFF

SET IDENTITY_INSERT Kind ON

INSERT INTO Kind(Id, GebruikerId, Naam, Voornaam, Geboortedatum, Allergieen, Medicatie)
VALUES
(1, 1, 'Daneels', 'Wannes', '2008-07-15', NULL, NULL),
(2, 1, 'Daneels', 'Stien', '2011-11-23', NULL, NULL),
(3, 1, 'Daneels', 'Siebe', '2011-11-23', NULL, NULL),
(4, 2, 'Buts', 'Bas', '2009-04-01', 'Pinda-noten', 'epipen'),
(5, 3, 'Moons', 'Mil', '2013-08-12', NULL, NULL),
(6, 3, 'Moons', 'Sien', '2010-12-25', NULL, NULL)

SET IDENTITY_INSERT Kind OFF

SET IDENTITY_INSERT Deelnemer ON

INSERT INTO Deelnemer(Id, KindId, GroepsreisDetailsId, Opmerking)
VALUES
(1, 1, 5, NULL),
(2, 2, 5, NULL),
(3, 2, 7, NULL),
(4, 2, 8, NULL),
(5, 3, 1, NULL),
(6, 4, 3, NULL),
(7, 4, 2, NULL),
(8, 5, 2, NULL)

SET IDENTITY_INSERT Deelnemer OFF

SET IDENTITY_INSERT Review ON

INSERT INTO Review(Id, PersoonId, BestemmingId, Tekst, Score)
VALUES
(1, 1, 5, 'Onze Wannes en ons Stien vonden het hier echte de MAX!', 5),
(2, 3, 2, 'Mijn zoon Mil is al weenend terug naar huis gekomen. Normaal ging ons Sien hier ook naar toe maar nu wil ze niet meer :/', 2),
(3, 2, 2, 'Ik ben hier zelf geweest als monitor samen met mijn zoon die mee ging als deelnemer. Ik vond het eten zeer goed en de activiteiten waren altijd gezellig.', 4)

SET IDENTITY_INSERT Review OFF


SET IDENTITY_INSERT Opleiding ON

INSERT INTO Opleiding(Id, Naam, Beschrijving, BeginDatum, EindDatum, AantalPlaatsen, OpleidingVereist)
VALUES
(1, 'Basisopleiding Reisleider', 'Een introductie in reisleiderschap, inclusief communicatie, logistiek en veiligheid.', '2024-01-15', '2024-01-20', 20, NULL),
(2, 'Gevorderde Reisleider', 'Verdiepende training voor ervaren monitoren, met focus op crisissituaties en complexe reisplanning.', '2024-02-10', '2024-02-15', 15, NULL),
(3, 'Outdoor Monitor Training', 'Specialisatie in het begeleiden van outdoor- en avontuurlijke reizen, inclusief EHBO en survivaltechnieken.', '2024-03-05', '2024-03-10', 25, NULL),
(4, 'Internationale Groepen Begeleiden', 'Training gericht op het werken met diverse internationale groepen en interculturele communicatie.', '2024-04-01', '2024-04-07', 10, 3),
(5, 'EHBO voor Monitoren', 'Volledige opleiding voor eerste hulp bij ongelukken, specifiek gericht op reisomstandigheden.', '2024-05-15', '2024-05-17', 30, NULL),
(6, 'Specialisatie: Ski- en Wintersportreizen', 'Training voor het begeleiden van groepen tijdens ski- en wintersportreizen.', '2024-06-10', '2024-06-15', 12, 5);

SET IDENTITY_INSERT Opleiding OFF

SET IDENTITY_INSERT OpleidingPersoon ON

INSERT INTO OpleidingPersoon(Id, OpleidingId, GebruikerId)
VALUES
(1, 1, 2),
(2, 2, 2),
(3, 3, 2),
(4, 4, 2),
(5, 1, 4),
(6, 2, 4),
(7, 5, 4),
(8, 6, 4)

SET IDENTITY_INSERT OpleidingPersoon OFF



