namespace AKVCore
{
	using ApS.Databases.Firebird;

	public class ErrorEventArgs : ErrorInUpdate
	{
		public ErrorEventArgs(string errorMsg) : base(errorMsg)
		{

		}
	}
}