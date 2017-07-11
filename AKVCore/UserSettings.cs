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
		#region Allgemein
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

		public static DateTime LetzterStartAm
		{
			get { return Core.CoreSettings.GetSetting("LetzterStartAm").ToDateTime(); }
			set { Core.CoreSettings.SetSetting("LetzterStartAm", value); }
		}

		public static bool ZeigeGesamtBetrag
		{
			get { return Core.CoreSettings.GetSetting("ZeigeGesamtBetrag").ToBoolean(); }
			set { Core.CoreSettings.SetSetting("ZeigeGesamtBetrag", value); }
		}
		#endregion Allgemein

		#region UnterKonten
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

		public static bool UnterKontoSummieren
		{
			get { return Core.CoreSettings.GetSetting("UnterKontoSummieren").ToBoolean(); }
			set { Core.CoreSettings.SetSetting("UnterKontoSummieren", value); }
		}
		#endregion UnterKonten

		#region Updates
		public static bool Updates
		{
			get { return Core.CoreSettings.GetSetting("Updates").ToBoolean(); }
			set { Core.CoreSettings.SetSetting("Updates", value); }
		}

		public static int UpdateAlleXTage
		{
			get { return Core.CoreSettings.GetSetting("UpdateAlleXTage").ToInt(); }
			set { Core.CoreSettings.SetSetting("UpdateAlleXTage", value); }
		}

		public static DateTime LetztesUpdateAm
		{
			get { return Core.CoreSettings.GetSetting("LetztesUpdateAm").ToDateTime(); }
			set { Core.CoreSettings.SetSetting("LetztesUpdateAm", value); }
		}
		#endregion Updates
	}
}