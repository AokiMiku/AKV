namespace AKVCore
{
	using ApS.Databases;

	public class UnterKonto : Business
	{
		#region Konstruktoren
		public UnterKonto() : base("UnterKonto", "", "", "", "", SqlAction.Null)
		{

		}
		public UnterKonto(string where, SqlAction action) : base("UnterKonto", where, "", "", "", action)
		{

		}
		public UnterKonto(string where, string spalten, SqlAction action) : base("UnterKonto", where, spalten, "", "", action)
		{

		}
		public UnterKonto(string where, string spalten, string orderBy, string groupBy, SqlAction action) : base("UnterKonto", where, spalten, orderBy, groupBy, action)
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
		public int Konto_Nr
		{
			get { return this.GetInt("KONTO_NR"); }
			set { this.Put("KONTO_NR", value); }
		}
		#endregion DbZugriffe
	}
}