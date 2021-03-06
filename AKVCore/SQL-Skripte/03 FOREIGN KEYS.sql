ALTER TABLE Kosten
	ADD FOREIGN KEY (Konto_Nr)
	REFERENCES Konto (Nummer)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	
ALTER TABLE KOSTEN 
	ADD FOREIGN KEY (UnterKonto_Nr) 
	REFERENCES UnterKonto (Nummer) 
	ON DELETE SET NULL 
	ON UPDATE CASCADE;
	
ALTER TABLE UNTERKONTO 
	ADD CONSTRAINT FK_UNTERKONTO_KONTONR 
	FOREIGN KEY (KONTO_NR) 
	REFERENCES KONTO (NUMMER) 
	ON DELETE CASCADE 
	ON UPDATE CASCADE;