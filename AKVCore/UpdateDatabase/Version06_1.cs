namespace AKVCore.Datenbankversionen
{
	using ApS.Firebird;
	internal class Version06_1 : UpdateDatabase
	{
		#region Fields/Properties
		#region public

		#endregion public
		#region private/protected
		private string UnterKontoForeignKey = "EXECUTE BLOCK AS BEGIN IF (NOT EXISTS(SELECT 1 FROM RDB$RELATION_CONSTRAINTS WHERE RDB$CONSTRAINT_NAME = 'FK_UNTERKONTO_KONTONR')) THEN BEGIN EXECUTE STATEMENT 'ALTER TABLE UNTERKONTO ADD CONSTRAINT FK_UNTERKONTO_KONTONR FOREIGN KEY (KONTO_NR) REFERENCES KONTO (NUMMER) ON DELETE CASCADE ON UPDATE CASCADE;'; END END;";
		#endregion private/protected
		#endregion Fields/Properties

		#region Constructors
		public Version06_1() : base()
		{
			base.AddStatements(this.UnterKontoForeignKey);
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