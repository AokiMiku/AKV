namespace AKVCore.Datenbankversionen
{
	using ApS.Databases.Firebird;
	internal class Version05 : UpdateDatabase
	{
		#region Fields/Properties
		#region public

		#endregion public
		#region private/protected
		private readonly string KostenAddEinnahme = "EXECUTE BLOCK AS BEGIN IF (NOT EXISTS(SELECT 1 FROM RDB$RELATION_FIELDS R LEFT JOIN RDB$FIELDS F ON R.RDB$FIELD_SOURCE = F.RDB$FIELD_NAME WHERE R.RDB$RELATION_NAME = 'KOSTEN' AND R.RDB$FIELD_NAME = 'EINNAHME')) THEN BEGIN EXECUTE STATEMENT 'ALTER TABLE KOSTEN ADD EINNAHME BOOLEAN DEFAULT 0 NOT NULL;'; END END;";
		#endregion private/protected
		#endregion Fields/Properties

		#region Constructors
		public Version05() : base()
		{
			base.AddStatements(this.KostenAddEinnahme);
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