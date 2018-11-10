namespace AKVCore.Datenbankversionen
{
	using ApS.Databases.Firebird;

	internal class Version02 : UpdateDatabase
	{
		#region Fields/Properties
		#region public

		#endregion public
		#region private/protected
		private readonly string EinstellungenValueKeyLength80 = "EXECUTE BLOCK AS BEGIN IF (EXISTS(SELECT 1 FROM RDB$RELATION_FIELDS R LEFT JOIN RDB$FIELDS F ON R.RDB$FIELD_SOURCE = F.RDB$FIELD_NAME WHERE R.RDB$RELATION_NAME = 'EINSTELLUNGEN' AND R.RDB$FIELD_NAME = 'SETTINGKEY' AND F.RDB$FIELD_LENGTH = 40)) THEN BEGIN EXECUTE STATEMENT 'ALTER TABLE EINSTELLUNGEN ALTER SETTINGKEY TYPE VARCHAR(80);'; EXECUTE STATEMENT 'ALTER TABLE EINSTELLUNGEN ALTER SETTINGVALUE TYPE VARCHAR(80);'; END END;";
		#endregion private/protected
		#endregion Fields/Properties

		#region Constructors
		public Version02() : base()
		{
			base.AddStatements(this.EinstellungenValueKeyLength80);
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