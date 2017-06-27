namespace AKVCore
{
	public class UnterKonto : Business
	{
		#region Konstruktoren
		public UnterKonto() : base("UnterKonto", "", "", "", "", ApS.SqlAction.Null)
		{

		}
		public UnterKonto(string where, ApS.SqlAction action) : base("UnterKonto", where, "", "", "", action)
		{

		}
		public UnterKonto(string where, string spalten, ApS.SqlAction action) : base("UnterKonto", where, spalten, "", "", action)
		{

		}
		public UnterKonto(string where, string spalten, string orderBy, string groupBy, ApS.SqlAction action) : base("UnterKonto", where, spalten, orderBy, groupBy, action)
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