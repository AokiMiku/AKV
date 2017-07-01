CREATE TABLE Konto
(
	Nummer 			integer NOT NULL PRIMARY KEY,
	Name			varchar(100) NOT NULL,
	Saldo			decimal(18,2) NOT NULL,
	Gebuehren		decimal(5,2),
	Zinsen_pa		decimal(6,4),
	Dispo_pa		decimal(6,4)
);

CREATE TABLE UnterKonto
(
	Nummer			integer NOT NULL PRIMARY KEY,
	Name			varchar(100) NOT NULL,
	Saldo			decimal(18,2),
	Konto_Nr		integer NOT NULL
);

CREATE TABLE Kosten
(
	Nummer			integer NOT NULL PRIMARY KEY,
	Bezeichnung		varchar(100) NOT NULL,
	Betrag			decimal(18,2) NOT NULL,
	Intervall		integer NOT NULL,
	IntervallEinheit varchar(100) NOT NULL,
	Bezahlt			BOOLEAN DEFAULT 0 NOT NULL,
	BezahltAm		date,
	LaufzeitBis		date,
	Einnahme		BOOLEAN DEFAULT 0 NOT NULL,
	Konto_Nr		integer NOT NULL,
	UnterKonto_Nr	integer,
	Notiz			BLOB sub_type 1
);

CREATE TABLE Einstellungen
(
	Nummer			integer NOT NULL PRIMARY KEY,
	SettingKey				varchar(80) NOT NULL,
	SettingValue			varchar(80) NOT NULL
);