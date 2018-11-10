namespace AKVCore
{
	using ApS.Databases;

	public class Konto : Business
	{
		#region Konstruktoren
		public Konto() : base("Konto", "", "", "", "", SqlAction.Null)
		{

		}
		public Konto(string where, SqlAction action) : base("Konto", where, "", "", "", action)
		{

		}
		public Konto(string where, string spalten, SqlAction action) : base("Konto", where, spalten, "", "", action)
		{

		}
		public Konto(string where, string spalten, string orderBy, string groupBy, SqlAction action) : base("Konto", where, spalten, orderBy, groupBy, action)
		{

		}
		#endregion Konstruktoren

		#region DbZugriffe
		public string Name
		{
			get { return this.GetString("NAME"); }
			set { this.Put("NAME", value); }
		}
		public decimal Saldo
		{
			get { return this.GetDecimal("SALDO"); }
			set { this.Put("SALDO", value); }
		}
		public decimal Gebuehren
		{
			get { return this.GetDecimal("GEBUEHREN"); }
			set { this.Put("GEBUEHREN", value); }
		}
		public decimal Zinsen_pa
		{
			get { return this.GetDecimal("ZINSEN_PA"); }
			set { this.Put("ZINSEN_PA", value); }
		}
		public decimal Dispo_pa
		{
			get { return this.GetDecimal("DISPO_PA"); }
			set { this.Put("DISPO_PA", value); }
		}
		public string Notiz
		{
			get { return this.GetString("NOTIZ"); }
			set { this.Put("NOTIZ", value); }
		}
		public bool Schuldkonto
		{
			get { return this.GetBool("SCHULDKONTO"); }
			set { this.Put("SCHULDKONTO", value); }
		}
		#endregion DbZugriffe
	}
}