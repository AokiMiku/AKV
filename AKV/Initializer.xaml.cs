namespace AKV
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Shapes;

	/// <summary>
	/// Interaktionslogik für Initializer.xaml
	/// </summary>
	public partial class Initializer : Window
	{
		public Initializer()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			AKVCore.Core.Initializer.ProgressChanged += Initializer_ProgressChanged;
			AKVCore.Core.Initializer.InitilizationFinished += Initializer_InitilizationFinished;
		}

		private void Initializer_InitilizationFinished(object sender, EventArgs e)
		{
			this.Dispatcher.Invoke(() => this.Close());
		}

		private void Initializer_ProgressChanged(object sender, AKVCore.ProgressEventArgs e)
		{
			this.Dispatcher.Invoke(() => this.progressInitialization.Maximum = e.ProgressMaxValue);
			this.Dispatcher.Invoke(() => this.progressInitialization.Value = e.ProgressCurrentValue);
		}

		private void progressInitialization_Loaded(object sender, RoutedEventArgs e)
		{
			ApS.Threads.AddThread("SetAllAsIntervall", new System.Threading.Thread(new System.Threading.ThreadStart(() => AKVCore.Core.Initializer.SetAllAsIntervall())), true);
		}
	}
}
