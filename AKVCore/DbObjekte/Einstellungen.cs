namespace AKVCore
{
	using ApS;
	public class Einstellungen : Business
	{
		#region Konstruktoren
		public Einstellungen() : base("Einstellungen", "", "", "", "", SqlAction.Null)
		{

		}
		public Einstellungen(string where, SqlAction action) : base("Einstellungen", where, "", "", "", action)
		{

		}
		public Einstellungen(string where, string spalten, SqlAction action) : base("Einstellungen", where, spalten, "", "", action)
		{

		}
		public Einstellungen(string where, string spalten, string orderBy, string groupBy, SqlAction action) : base("Einstellungen", where, spalten, orderBy, groupBy, action)
		{

		}
		#endregion Konstruktoren
		
		public string GetSetting(string key)
		{
			this.Where = "SettingKey = '" + key + "'";
			this.Read();
			return this.SettingValue;
		}

		public void SetSetting(string key, string value)
		{
			this.Where = "SettingKey = '" + key + "'";
			this.Read();

			this.SettingKey = key;
			this.SettingValue = value;

			if (this.EoF)
				this.BuildSaveStmt(SqlAction.Insert);
			else
				this.BuildSaveStmt(SqlAction.Update);

			this.SaveStmtsOnly();
		}

		public void SetSetting(string key, bool value)
		{
			this.SetSetting(key, value.ToString());
		}

		#region DbZugriffe
		public string SettingKey
		{
			get { return this.GetString("SETTINGKEY"); }
			set { this.Put("SETTINGKEY", value); }
		}
		public string SettingValue
		{
			get { return this.GetString("SETTINGVALUE"); }
			set { this.Put("SETTINGVALUE", value); }
		}
		#endregion DbZugriffe
	}
}