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
		public string IntervallEinheit
		{
			get { return this.GetString("INTERVALLEINHEIT"); }
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
}
