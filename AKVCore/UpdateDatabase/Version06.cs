namespace AKVCore.Datenbankversionen
{
	using ApS.Databases.Firebird;

	internal class Version06 : UpdateDatabase
	{
		#region Fields/Properties
		#region public

		#endregion public
		#region private/protected
		private readonly string NewTableUnterKonto = "EXECUTE BLOCK AS BEGIN IF (NOT EXISTS(SELECT 1 FROM RDB$RELATION_FIELDS R LEFT JOIN RDB$FIELDS F ON R.RDB$FIELD_SOURCE = F.RDB$FIELD_NAME WHERE R.RDB$RELATION_NAME = 'UNTERKONTO')) THEN BEGIN EXECUTE STATEMENT 'CREATE TABLE UnterKonto (Nummer integer NOT NULL PRIMARY KEY, Name varchar(100) NOT NULL, Saldo decimal(18,2), Konto_Nr integer NOT NULL);'; EXECUTE STATEMENT 'CREATE SEQUENCE GEN_UNTERKONTO_ID;'; EXECUTE STATEMENT 'CREATE TRIGGER UnterKonto_BI FOR UnterKonto ACTIVE BEFORE INSERT POSITION 0 AS BEGIN IF (new.nummer is null) THEN new.nummer = GEN_ID(GEN_UNTERKONTO_ID,1); END;'; END END;";
		private readonly string KostenAddUnterKonto_Nr = "EXECUTE BLOCK AS BEGIN IF (NOT EXISTS(SELECT 1 FROM RDB$RELATION_FIELDS R LEFT JOIN RDB$FIELDS F ON R.RDB$FIELD_SOURCE = F.RDB$FIELD_NAME WHERE R.RDB$RELATION_NAME = 'KOSTEN' AND R.RDB$FIELD_NAME = 'UNTERKONTO_NR')) THEN BEGIN EXECUTE STATEMENT 'ALTER TABLE KOSTEN ADD UNTERKONTO_NR INTEGER;'; EXECUTE STATEMENT 'ALTER TABLE KOSTEN ADD FOREIGN KEY (UnterKonto_Nr) REFERENCES UnterKonto (Nummer) ON DELETE SET NULL ON UPDATE CASCADE;'; END END;";
		private readonly string KostenAddNotiz = "EXECUTE BLOCK AS BEGIN IF (NOT EXISTS(SELECT 1 FROM RDB$RELATION_FIELDS R LEFT JOIN RDB$FIELDS F ON R.RDB$FIELD_SOURCE = F.RDB$FIELD_NAME WHERE R.RDB$RELATION_NAME = 'KOSTEN' AND R.RDB$FIELD_NAME = 'NOTIZ')) THEN BEGIN EXECUTE STATEMENT 'ALTER TABLE KOSTEN ADD NOTIZ BLOB SUB_TYPE 1;'; END END;";
		#endregion private/protected
		#endregion Fields/Properties

		#region Constructors
		public Version06() : base()
		{
			base.AddStatements(this.NewTableUnterKonto);
			base.AddStatements(this.KostenAddUnterKonto_Nr);
			base.AddStatements(this.KostenAddNotiz);
		}
		#endregion Constructors

		#region Methods
		#region public

		#endregion public
		#region private/protected

		#endregion private/protected
		#endregion Methods
	}
}