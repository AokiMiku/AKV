namespace AKVCore.Datenbankversionen
{
	using ApS.Firebird;
	internal class Version07 : UpdateDatabase
	{
		#region Fields/Properties
		#region public

		#endregion public
		#region private/protected
		private string AddKostenIntervallEinheit = "EXECUTE BLOCK AS BEGIN IF (NOT EXISTS(SELECT 1 FROM RDB$RELATION_FIELDS R LEFT JOIN RDB$FIELDS F ON R.RDB$FIELD_SOURCE = F.RDB$FIELD_NAME WHERE R.RDB$RELATION_NAME = 'KOSTEN' AND R.RDB$FIELD_NAME = 'INTERVALLEINHEIT')) THEN BEGIN EXECUTE STATEMENT 'ALTER TABLE KOSTEN ADD INTERVALLEINHEIT INTEGER NOT NULL;'; END END;";
		#endregion private/protected
		#endregion Fields/Properties

		#region Constructors
		public Version07() : base()
		{
			base.AddStatements(this.AddKostenIntervallEinheit);
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