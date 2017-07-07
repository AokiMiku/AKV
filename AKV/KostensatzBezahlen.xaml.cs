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
	using AKVCore;
	using ApS;

	/// <summary>
	/// Interaktionslogik für KostensatzBezahlen.xaml
	/// </summary>
	public partial class KostensatzBezahlen : Base4Windows
	{
		int kostenNr = 0;
		Core.KostenCore kostenCore;
		public KostensatzBezahlen()
		{
			InitializeComponent();
		}

		private void bezahlen_Click(object sender, RoutedEventArgs e)
		{
			DateTime bezahltAm = Settings.NullDate;

			if (this.bezahltAm.SelectedDate != null)
				bezahltAm = (DateTime)this.bezahltAm.SelectedDate;

			this.kostenCore.BezahltAm = bezahltAm;

			this.kostenCore.Pay(this.kostenNr);

			this.DialogResult = true;
			this.Close();
		}

		private void abbrechen_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		public void Start(int kostenNr)
		{
			this.kostenCore = new Core.KostenCore();
			this.kostenNr = kostenNr;
			this.ShowDialog();
		}

		private void Controls_KeyUp(object sender, KeyEventArgs e)
		{
			if (sender is DatePicker && e.Key != Key.Enter)
			{
				DatePicker picker = sender as DatePicker;

				if (e.Key.KeyIsNumericOrDecimal())
					picker.Text = "";
				if (e.Key == Key.V)
					picker.SelectedDate = DateTime.Now.AddDays(-2).Date;
				else if (e.Key == Key.G)
					picker.SelectedDate = DateTime.Now.AddDays(-1).Date;
				else if (e.Key == Key.H)
					picker.SelectedDate = DateTime.Now.Date;
				else if (e.Key == Key.M)
					picker.SelectedDate = DateTime.Now.AddDays(1).Date;
				else if (e.Key == Key.U)
					picker.SelectedDate = DateTime.Now.AddDays(2).Date;
			}

			if (e.Key == Key.Enter)
			{
				this.bezahlen_Click(sender, new RoutedEventArgs());
			}
		}
	}
}