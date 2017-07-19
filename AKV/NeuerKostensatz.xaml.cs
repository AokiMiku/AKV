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
	using ApS;

	/// <summary>
	/// Interaktionslogik für NeuerKostensatz.xaml
	/// </summary>
	public partial class NeuerKostensatz : Base4Windows
	{
		int konto_nr = 0;
		int kosten_nr = 0;
		Core.KostenCore kostenCore;

		public NeuerKostensatz(Window parent) : base(parent)
		{
			InitializeComponent();
		}

		private void Core_KostenExist(object sender, EventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(this, "In dieser Kategorie existiert bereits ein Kostensatz mit diesem Namen." + Environment.NewLine + " Möchten Sie ihn trotzdem neu hinzufügen?", "Kostensatz existiert bereits.", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
				this.kostenCore.KostenExistInsert = true;
			else
				this.kostenCore.KostenExistInsert = false;
			this.kostenCore.KostenExistProcessed = true;
		}

		private void speichern_Click(object sender, RoutedEventArgs e)
		{
			decimal betrag = -1;
			int intervall = -1;
			bool bezahlt = false;
			DateTime bezahltAm = Settings.NullDate;
			DateTime laufzeitBis = Settings.NullDate;
			bool einnahme = false;
			int unterKonto_nr = -1;

			if (string.IsNullOrEmpty(this.bezeichnung.Text))
			{
				MessageBox.Show(this, "Bezeichnung darf nicht leer sein.", "Fehler", MessageBoxButton.OK);
				this.bezeichnung.Focus();
				return;
			}
			if (!string.IsNullOrEmpty(this.betrag.Text))
			{
				this.betrag.Text = this.betrag.Text.Replace(',', '.');
				bool negativ = this.betrag.Text.StartsWith("-");
				if (negativ)
					this.betrag.Text = this.betrag.Text.Substring(1);

				if (!decimal.TryParse(this.betrag.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out betrag))
				{
					MessageBox.Show(this, "Ungültiger Wert im Feld Betrag.", "Fehler", MessageBoxButton.OK);
					this.betrag.Focus();
					return;
				}
				else if (negativ)
					betrag *= -1;
			}
			else
			{
				MessageBox.Show(this, "Betrag darf nicht leer sein.", "Fehler", MessageBoxButton.OK);
				this.betrag.Focus();
				return;
			}
			if (!string.IsNullOrEmpty(this.intervall.Text))
			{
				if (!int.TryParse(this.intervall.Text, out intervall))
				{
					MessageBox.Show(this, "Ungültiger Wert im Feld Intervall.", "Fehler", MessageBoxButton.OK);
					this.intervall.Focus();
					return;
				}
			}
			else
			{
				intervall = -1;
			}

			IntervallEinheiten einheit = IntervallEinheiten.Null;
            if (this.intervallEinheiten.SelectedIndex != -1)
				einheit = (IntervallEinheiten)this.intervallEinheiten.SelectedIndex;

			bezahlt = this.bezahltAm.SelectedDate != null;

			if (bezahlt)
			{
				if (this.bezahltAm.SelectedDate == null)
				{
					MessageBox.Show(this, "Es muss ein \"Bezahlt am\"-Datum ausgewählt sein, wenn als bezahlt markiert.", "Fehler", MessageBoxButton.OK);
					this.bezahltAm.Focus();
					return;
				}
				else
					bezahltAm = (DateTime)this.bezahltAm.SelectedDate;
			}

			if (this.laufzeitBis.SelectedDate != null)
				laufzeitBis = (DateTime)this.laufzeitBis.SelectedDate;

			if (this.einnahme.IsChecked.ToBoolean())
				einnahme = true;
			else if (this.ausgabe.IsChecked.ToBoolean())
				einnahme = false;
			else
			{
				MessageBox.Show(this, "Der Kostensatz muss als \"Einnahme\" oder \"Ausgabe\" markiert sein.", "Fehler", MessageBoxButton.OK);
				return;
			}

			if (UserSettings.UnterKonten)
			{
				if (this.unterKonten.SelectedItem != null)
				{
					unterKonto_nr = this.kostenCore.GetUnterKontoNummer(this.konto_nr, this.unterKonten.SelectedItem.ToString());
				}
				else if (this.unterKonten.Items.Count > 0)
				{
					MessageBox.Show(this, "Wenn Unter-Kategorien aktiviert sind, muss eine vergeben werden.", "Fehler", MessageBoxButton.OK);
					return;
				}
			}

			this.kostenCore.Bezeichnung = this.bezeichnung.Text;
			this.kostenCore.Betrag = betrag;
			this.kostenCore.Intervall = intervall;
			this.kostenCore.IntervallEinheit = einheit;
			this.kostenCore.Bezahlt = bezahlt;
			this.kostenCore.BezahltAm = bezahltAm;
			this.kostenCore.LaufzeitBis = laufzeitBis;
			this.kostenCore.Einnahme = einnahme;
			this.kostenCore.Notiz = this.notiz.Text;
			this.kostenCore.UnterKonto_Nr = unterKonto_nr;

			Core.SaveCompleted += Core_SaveCompleted;
			if (this.modus == FensterModus.Neu)
				this.kostenCore.Add(this.konto_nr);
			else
				this.kostenCore.Edit(this.kosten_nr);

		}

		private void Core_SaveCompleted(object sender, EventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void abbrechen_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		public void Start(int konto_nr, int kosten_nr = -1, int uKonto_nr = -1)
		{
			this.kostenCore = new Core.KostenCore();
			this.konto_nr = konto_nr;

			using (Konto k = new Konto())
			{
				k.Where = "Nummer = " + this.konto_nr;
				k.Read();

				if (!k.EoF)
				{
					if (k.Schuldkonto)
					{
						this.ausgabe.Content = "Schulden hinzu";
						this.einnahme.Content = "Schulden abbezahlen";
					}
				}
			}

			this.intervallEinheiten.Items.Add(IntervallEinheiten.AlleXTage.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.AlleXWochen.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.AlleXMonate.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.AlleXJahre.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.Januar.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.Februar.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.März.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.April.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.Mai.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.Juni.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.Juli.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.August.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.September.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.Oktober.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.November.EinheitToString());
			this.intervallEinheiten.Items.Add(IntervallEinheiten.Dezember.EinheitToString());

			if (!UserSettings.UnterKonten)
			{
				this.frontend.RowDefinitions[7].Height = new GridLength(0);
			}
			else
			{
				this.unterKonten.Items.Clear();
				Core.UnterKontoCore core = new Core.UnterKontoCore();
				core.Konto_Nr = this.konto_nr;
				UnterKonto konto = core.GetAlleKonten();
				while (!konto.EoF)
				{
					this.unterKonten.Items.Add(konto.Name);
					if (uKonto_nr > 0 && uKonto_nr == konto.Nummer)
						this.unterKonten.SelectedItem = konto.Name;
					konto.Skip();
				}
			}

			if (kosten_nr != -1)
			{
				this.kosten_nr = kosten_nr;
				this.modus = FensterModus.Edit;
				this.Title = "Kostensatz editieren";
				Kosten kosten = new Kosten();
				kosten.Where = "Nummer = " + kosten_nr;
				kosten.Read();

				if (kosten.Bezahlt)
				{
					MessageBoxResult result = MessageBox.Show(this, "Kostensatz wurde bereits als bezahlt markiert. Wollen Sie ihn trotzdem ändern?", "Ist bereits bezahlt", MessageBoxButton.YesNo);
					if (result != MessageBoxResult.Yes)
						return;
				}

				this.bezeichnung.Text = kosten.Bezeichnung;
				this.betrag.Text = kosten.Betrag.ToString();
				if (kosten.Intervall != -1)
					this.intervall.Text = kosten.Intervall.ToString();
				if (kosten.IntervallEinheit != -1)
					this.intervallEinheiten.SelectedIndex = kosten.IntervallEinheit;
				//this.bezahlt.IsChecked = kosten.Bezahlt;
				if (kosten.BezahltAm != ApS.Settings.NullDate)
					this.bezahltAm.SelectedDate = kosten.BezahltAm;
				if (kosten.LaufzeitBis != ApS.Settings.NullDate)
					this.laufzeitBis.SelectedDate = kosten.LaufzeitBis;
				if (kosten.Einnahme)
					this.einnahme.IsChecked = true;
				else
					this.ausgabe.IsChecked = true;
				this.notiz.Text = kosten.Notiz;

				if (kosten.UnterKonto_Nr > 0)
				{
					UnterKonto konto = new UnterKonto();
					konto.Where = "Nummer = " + kosten.UnterKonto_Nr;
					konto.Read();

					if (!konto.EoF)
						this.unterKonten.SelectedItem = konto.Name;
				}
			}


			this.kostenCore.KostenExist += Core_KostenExist;
			this.ShowDialog();

			this.kostenCore.KostenExist -= Core_KostenExist;
		}

		private void Controls_KeyUp(object sender, KeyEventArgs e)
		{
			if (sender is DatePicker)
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

			if (!string.IsNullOrEmpty(this.bezeichnung.Text) && !string.IsNullOrEmpty(this.betrag.Text) && e.Key == Key.Enter)
			{
				this.speichern_Click(sender, new RoutedEventArgs());
			}
		}
	}
}