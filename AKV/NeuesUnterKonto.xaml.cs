namespace AKV
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Shapes;
	using AKVCore;

	/// <summary>
	/// Interaktionslogik für NeuesKonto.xaml
	/// </summary>
	public partial class NeuesUnterKonto : Base4Windows
	{
		int unterKonto_nr;
		int konto_nr;
        Core.UnterKontoCore kontoCore;

		public NeuesUnterKonto(Window parent) : base(parent)
		{
			InitializeComponent();
		}

		private void speichern_Click(object sender, RoutedEventArgs e)
		{
			decimal sald = 0;

			if (string.IsNullOrEmpty(this.name.Text))
			{
				MessageBox.Show(this, "Name darf nicht leer sein.", "Fehler", MessageBoxButton.OK);
				this.name.Focus();
				return;
			}
			if (!string.IsNullOrEmpty(this.saldo.Text))
			{
				this.saldo.Text = this.saldo.Text.Replace(',', '.');
				bool negativ = this.saldo.Text.StartsWith("-");
				if (negativ)
					this.saldo.Text = this.saldo.Text.Substring(1);

				if (!decimal.TryParse(this.saldo.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out sald))
				{
					MessageBox.Show(this, "Ungültiger Wert im Feld Betrag.", "Fehler", MessageBoxButton.OK);
					this.saldo.Focus();
					return;
				}
				else if (negativ)
					sald *= -1;
			}

			this.kontoCore.Name = this.name.Text;
			this.kontoCore.Saldo = sald;
			this.kontoCore.Konto_Nr = this.konto_nr;

			if (this.modus == FensterModus.Neu)
				this.kontoCore.Add();
			else
				this.kontoCore.Edit(this.unterKonto_nr);

			this.DialogResult = true;
			this.Close();
		}

		private void abbrechen_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		public string Start(int konto_nr, int unterKonto_nr = -1)
		{
			this.konto_nr = konto_nr;
            this.kontoCore = new Core.UnterKontoCore();
			if (unterKonto_nr != -1)
			{
				this.unterKonto_nr = unterKonto_nr;
				this.modus = FensterModus.Edit;
				this.Title = "Unter-Kategorie editieren";
				UnterKonto konto = new UnterKonto();
				konto.Where = "Nummer = " + unterKonto_nr;
				konto.Read();

				this.name.Text = konto.Name;
				this.saldo.Text = konto.Saldo.ToString();
			}
			this.ShowDialog();

			if (this.DialogResult == true)
			{
				return this.name.Text;
			}
			else
				return "";
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

			if (!string.IsNullOrEmpty(this.name.Text) && !string.IsNullOrEmpty(this.saldo.Text) && e.Key == Key.Enter)
			{
				this.speichern_Click(sender, new RoutedEventArgs());
			}
		}
	}
}
