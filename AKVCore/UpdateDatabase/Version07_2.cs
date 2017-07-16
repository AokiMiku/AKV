namespace AKVCore.Datenbankversionen
{
	using ApS.Firebird;
	internal class Version07_2 : UpdateDatabase
	{
		#region Fields/Properties
		#region public

		#endregion public
		#region private/protected
		private string KostenAddNotiz = "EXECUTE BLOCK AS BEGIN IF (NOT EXISTS(SELECT 1 FROM RDB$RELATION_FIELDS R LEFT JOIN RDB$FIELDS F ON R.RDB$FIELD_SOURCE = F.RDB$FIELD_NAME WHERE R.RDB$RELATION_NAME = 'KONTO' AND R.RDB$FIELD_NAME = 'NOTIZ')) THEN BEGIN EXECUTE STATEMENT 'ALTER TABLE KONTO ADD NOTIZ BLOB SUB_TYPE 1;'; END END;";
		private string KostenAddSchuldkonto = "EXECUTE BLOCK AS BEGIN IF (NOT EXISTS(SELECT 1 FROM RDB$RELATION_FIELDS R LEFT JOIN RDB$FIELDS F ON R.RDB$FIELD_SOURCE = F.RDB$FIELD_NAME WHERE R.RDB$RELATION_NAME = 'KONTO' AND R.RDB$FIELD_NAME = 'SCHULDKONTO')) THEN BEGIN EXECUTE STATEMENT 'ALTER TABLE KONTO ADD SCHULDKONTO BOOLEAN DEFAULT 0 NOT NULL;'; END END;";
		#endregion private/protected
		#endregion Fields/Properties

		#region Constructors
		public Version07_2() : base()
		{
			base.AddStatements(this.KostenAddNotiz);
			base.AddStatements(this.KostenAddSchuldkonto);
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