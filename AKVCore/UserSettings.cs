namespace AKVCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using ApS;
	public static class UserSettings
	{
		public static bool UnterKonten
		{
			get { return Core.CoreSettings.GetSetting("UnterKonten").ToBoolean(); }
			set { Core.CoreSettings.SetSetting("UnterKonten", value); }
		}

		public static bool KostenPerUnterKonto
		{
			get { return Core.CoreSettings.GetSetting("KostenPerUnterKonto").ToBoolean(); }
			set { Core.CoreSettings.SetSetting("KostenPerUnterKonto", value); }
		}

		public static bool BeimStartKontoOeffnen
		{
			get { return Core.CoreSettings.GetSetting("BeimStartKontoOeffnen").ToBoolean(); }
			set { Core.CoreSettings.SetSetting("BeimStartKontoOeffnen", value); }
		}

		public static string ZuletztOffenesKonto
		{
			get { return Core.CoreSettings.GetSetting("ZuletztOffenesKonto"); }
			set { Core.CoreSettings.SetSetting("ZuletztOffenesKonto", value); }
		}
	}
}
