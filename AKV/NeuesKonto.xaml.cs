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
	public partial class NeuesKonto : Base4Windows
	{
		int konto_nr;
		Core.KontoCore kontoCore;
		public NeuesKonto()
		{
			InitializeComponent();

			this.frontend.RowDefinitions[2].Height = new GridLength(0);
			this.frontend.RowDefinitions[3].Height = new GridLength(0);
			this.frontend.RowDefinitions[4].Height = new GridLength(0);
		}

		private void speichern_Click(object sender, RoutedEventArgs e)
		{
			decimal sald = 0;
			decimal gebu = 0;
			decimal zins = 0;
			decimal disp = 0;

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
			else
			{
				MessageBox.Show(this, "Betrag darf nicht leer sein.", "Fehler", MessageBoxButton.OK);
				this.saldo.Focus();
				return;
			}
			if (!string.IsNullOrEmpty(this.gebuehren.Text))
				if (!decimal.TryParse(this.gebuehren.Text, out gebu))
				{
					MessageBox.Show(this, "Ungültiger Wert im Feld Gebühren.", "Fehler", MessageBoxButton.OK);
					this.gebuehren.Focus();
					return;
				}
			if (!string.IsNullOrEmpty(this.zinsenPA.Text))
				if (!decimal.TryParse(this.zinsenPA.Text, out zins))
				{
					MessageBox.Show(this, "Ungültiger Wert im Feld Zinsen.", "Fehler", MessageBoxButton.OK);
					this.zinsenPA.Focus();
					return;
				}
			if (!string.IsNullOrEmpty(this.dispoPA.Text))
				if (!decimal.TryParse(this.dispoPA.Text, out disp))
				{
					MessageBox.Show(this, "Ungültiger Wert im Feld Dispo.", "Fehler", MessageBoxButton.OK);
					this.dispoPA.Focus();
					return;
				}

			this.kontoCore.Name = this.name.Text;
			this.kontoCore.Saldo = sald;
			this.kontoCore.Gebuehren = gebu;
			this.kontoCore.ZinsenPA = zins;
			this.kontoCore.DispoPA = disp;

			if (this.modus == FensterModus.Neu)
				this.kontoCore.Add();
			else
				this.kontoCore.Edit(this.konto_nr);

			this.DialogResult = true;
			this.Close();
		}

		private void abbrechen_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		public string Start(int konto_nr = -1)
		{
			this.kontoCore = new Core.KontoCore();
			if (konto_nr != -1)
			{
				this.konto_nr = konto_nr;
				this.modus = FensterModus.Edit;
				this.Title = "Konto editieren";
				Konto konto = new Konto();
				konto.Where = "Nummer = " + konto_nr;
				konto.Read();

				this.name.Text = konto.Name;
				this.saldo.Text = konto.Saldo.ToString();
				this.zinsenPA.Text = konto.Zinsen_pa.ToString();
				this.dispoPA.Text = konto.Dispo_pa.ToString();
				this.gebuehren.Text = konto.Gebuehren.ToString();
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
