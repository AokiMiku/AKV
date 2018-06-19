using ApS.Firebird;

namespace AKVCore
{
	public class ErrorEventArgs : ErrorInUpdate
	{
		public ErrorEventArgs(string errorMsg) : base(errorMsg)
		{

		}
	}
}