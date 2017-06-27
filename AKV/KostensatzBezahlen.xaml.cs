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
	}
}