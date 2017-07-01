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

	using ApS;
	using AKVCore;
	/// <summary>
	/// Interaktionslogik für Einstellungen.xaml
	/// </summary>
	public partial class Einstellungen : Base4Windows
	{
		public Einstellungen()
		{
			InitializeComponent();
		}

		private void unterKontenAktiv_Click(object sender, RoutedEventArgs e)
		{
			if (this.unterKontenAktiv.IsChecked == true)
			{
				this.kostenPerUnterKonto.IsEnabled = true;
				this.unterKontoSummieren.IsEnabled = true;
			}
			else
			{
				this.kostenPerUnterKonto.IsEnabled = false;
				this.unterKontoSummieren.IsEnabled = false;
			}
		}

		private void speichern_Click(object sender, RoutedEventArgs e)
		{
			UserSettings.BeimStartKontoOeffnen = this.beimStartKontoOeffnen.IsChecked == true;
			UserSettings.ZeigeGesamtBetrag = this.zeigeGesamtBetrag.IsChecked == true;
			UserSettings.UnterKonten = this.unterKontenAktiv.IsChecked == true;
			UserSettings.KostenPerUnterKonto = this.kostenPerUnterKonto.IsChecked == true;
			UserSettings.UnterKontoSummieren = this.unterKontoSummieren.IsChecked == true;

			this.DialogResult = true;
			this.Close();
		}

		private void abbrechen_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void Base4Windows_Loaded(object sender, RoutedEventArgs e)
		{
			this.beimStartKontoOeffnen.IsChecked = UserSettings.BeimStartKontoOeffnen;
			this.zeigeGesamtBetrag.IsChecked = UserSettings.ZeigeGesamtBetrag;
			this.unterKontenAktiv.IsChecked = UserSettings.UnterKonten;
			if (this.unterKontenAktiv.IsChecked == true)
			{
				this.kostenPerUnterKonto.IsEnabled = true;
				this.unterKontoSummieren.IsEnabled = true;
			}
			this.kostenPerUnterKonto.IsChecked = UserSettings.KostenPerUnterKonto;
			this.unterKontoSummieren.IsChecked = UserSettings.UnterKontoSummieren;
		}
	}
}