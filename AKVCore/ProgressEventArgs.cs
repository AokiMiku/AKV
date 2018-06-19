using System;

namespace AKVCore
{
	public class ProgressEventArgs : EventArgs
	{
		public int ProgressMaxValue { get; protected set; }
		public int ProgressCurrentValue { get; protected set; }

		public ProgressEventArgs(int ProgressCurrentValue, int ProgressMaxValue)
		{
			this.ProgressCurrentValue = ProgressCurrentValue;
			this.ProgressMaxValue = ProgressMaxValue;
		}
	}
}