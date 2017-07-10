namespace AKVCore
{
	using System;

	public class Kosten : Business
	{
		#region Konstruktoren
		public Kosten() : base("Kosten", "", "", "", "", ApS.SqlAction.Null)
		{

		}
		public Kosten(string where, ApS.SqlAction action) : base("Kosten", where, "", "", "", action)
		{

		}
		public Kosten(string where, string spalten, ApS.SqlAction action) : base("Kosten", where, spalten, "", "", action)
		{

		}
		public Kosten(string where, string spalten, string orderBy, string groupBy, ApS.SqlAction action) : base("Kosten", where, spalten, orderBy, groupBy, action)
		{

		}
		#endregion Konstruktoren

		#region DbZugriffe
		public string Bezeichnung
		{
			get { return this.GetString("BEZEICHNUNG"); }
			set { this.Put("BEZEICHNUNG", value); }
		}
		public decimal Betrag
		{
			get { return this.GetDecimal("BETRAG"); }
			set { this.Put("BETRAG", value); }
		}
		public int Intervall
		{
			get { return this.GetInt("INTERVALL"); }
			set { this.Put("INTERVALL", value); }
		}
		public int IntervallEinheit
		{
			get { return this.GetInt("INTERVALLEINHEIT"); }
			set { this.Put("INTERVALLEINHEIT", value); }
		}
		public bool Bezahlt
		{
			get { return this.GetBool("BEZAHLT"); }
			set { this.Put("BEZAHLT", value); }
		}
		public DateTime BezahltAm
		{
			get { return this.GetDateTime("BEZAHLTAM"); }
			set { this.Put("BEZAHLTAM", value); }
		}
		public DateTime LaufzeitBis
		{
			get { return this.GetDateTime("LAUFZEITBIS"); }
			set { this.Put("LAUFZEITBIS", value); }
		}
		public bool Einnahme
		{
			get { return this.GetBool("EINNAHME"); }
			set { this.Put("EINNAHME", value); }
		}
		public int Konto_Nr
		{
			get { return this.GetInt("KONTO_NR"); }
			set { this.Put("KONTO_NR", value); }
		}
		public int UnterKonto_Nr
		{
			get { return this.GetInt("UNTERKONTO_NR"); }
			set { this.Put("UNTERKONTO_NR", value); }
		}
		public string Notiz
		{
			get { return this.GetString("NOTIZ"); }
			set { this.Put("NOTIZ", value); }
		}
		#endregion DbZugriffe
	}

	public enum IntervallEinheiten
	{
		AlleXTage,
		AlleXWochen,
		AlleXMonate,
		AlleXJahre,
		Januar,
		Februar,
		März,
		April,
		Mai,
		Juni,
		Juli,
		August,
		September,
		Oktober,
		November,
		Dezember,
		Null
	}
	public static class IntervallEinheitenExtensions
	{
		public static string EinheitToString(this IntervallEinheiten einheit)
		{
			switch (einheit)
			{
				case IntervallEinheiten.AlleXTage:
					return "Alle X Tage";
				case IntervallEinheiten.AlleXWochen:
					return "Alle X Wochen";
				case IntervallEinheiten.AlleXMonate:
					return "Alle X Monate";
				case IntervallEinheiten.AlleXJahre:
					return "Alle X Jahre";
				case IntervallEinheiten.Januar:
					return ". Januar";
				case IntervallEinheiten.Februar:
					return ". Februar";
				case IntervallEinheiten.März:
					return ". März";
				case IntervallEinheiten.April:
					return ". April";
				case IntervallEinheiten.Mai:
					return ". Mai";
				case IntervallEinheiten.Juni:
					return ". Juni";
				case IntervallEinheiten.Juli:
					return ". Juli";
				case IntervallEinheiten.August:
					return ". August";
				case IntervallEinheiten.September:
					return ". September";
				case IntervallEinheiten.Oktober:
					return ". Oktober";
				case IntervallEinheiten.November:
					return ". November";
				case IntervallEinheiten.Dezember:
					return ". Dezember";
				default:
					return "";
			}
		}

		public static string EinheitToSingularString(this IntervallEinheiten einheit)
		{
			switch (einheit)
			{
				case IntervallEinheiten.AlleXTage:
					return "Jeden Tag";
				case IntervallEinheiten.AlleXWochen:
					return "Jede Woche";
				case IntervallEinheiten.AlleXMonate:
					return "Jeden Monat";
				case IntervallEinheiten.AlleXJahre:
					return "Jedes Jahr";
				case IntervallEinheiten.Januar:
				case IntervallEinheiten.Februar:
				case IntervallEinheiten.März:
				case IntervallEinheiten.April:
				case IntervallEinheiten.Mai:
				case IntervallEinheiten.Juni:
				case IntervallEinheiten.Juli:
				case IntervallEinheiten.August:
				case IntervallEinheiten.September:
				case IntervallEinheiten.Oktober:
				case IntervallEinheiten.November:
				case IntervallEinheiten.Dezember:
				default:
					return einheit.EinheitToString();
			}
		}
	}
}