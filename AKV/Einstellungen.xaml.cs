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
		public Einstellungen(Window parent) : base(parent)
		{
			InitializeComponent();
		}

		private void unterKontenAktiv_Click(object sender, RoutedEventArgs e)
		{
			if (this.unterKontenAktiv.IsChecked.ToBoolean())
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
		private void updateAktiv_Click(object sender, RoutedEventArgs e)
		{
			if (this.updateAktiv.IsChecked.ToBoolean())
			{
				this.updateTageContainer.IsEnabled = true;
				this.checkUpdate.IsEnabled = true;
			}
			else
			{
				this.updateTageContainer.IsEnabled = false;
				this.checkUpdate.IsEnabled = false;
			}
		}

		private void speichern_Click(object sender, RoutedEventArgs e)
		{
			//Allgemein
			UserSettings.BeimStartKontoOeffnen = this.beimStartKontoOeffnen.IsChecked.ToBoolean();
			UserSettings.ZeigeGesamtBetrag = this.zeigeGesamtBetrag.IsChecked.ToBoolean();
			//Unterkonten
			UserSettings.UnterKonten = this.unterKontenAktiv.IsChecked.ToBoolean();
			UserSettings.KostenPerUnterKonto = this.kostenPerUnterKonto.IsChecked.ToBoolean();
			UserSettings.UnterKontoSummieren = this.unterKontoSummieren.IsChecked.ToBoolean();
			//Updates
			UserSettings.Updates = this.updateAktiv.IsChecked.ToBoolean();
			int tage = this.updateAlleXTage.Text.ToInt();
			if (tage == 0)
				tage = 1;
			UserSettings.UpdateAlleXTage = tage;

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
			//Allgemein
			this.beimStartKontoOeffnen.IsChecked = UserSettings.BeimStartKontoOeffnen;
			this.zeigeGesamtBetrag.IsChecked = UserSettings.ZeigeGesamtBetrag;
			//Unterkonten
			this.unterKontenAktiv.IsChecked = UserSettings.UnterKonten;
			if (this.unterKontenAktiv.IsChecked.ToBoolean())
			{
				this.kostenPerUnterKonto.IsEnabled = true;
				this.unterKontoSummieren.IsEnabled = true;
			}
			this.kostenPerUnterKonto.IsChecked = UserSettings.KostenPerUnterKonto;
			this.unterKontoSummieren.IsChecked = UserSettings.UnterKontoSummieren;
			//Updates
			this.updateAktiv.IsChecked = UserSettings.Updates;
			if (this.updateAktiv.IsChecked.ToBoolean())
			{
				this.updateTageContainer.IsEnabled = true;
				this.checkUpdate.IsEnabled = true;
			}
			this.updateAlleXTage.Text = UserSettings.UpdateAlleXTage.ToString();
		}

		private void checkUpdate_Click(object sender, RoutedEventArgs e)
		{
			Core.Updater up = new Core.Updater();
			if (up.CheckForUpdate())
			{
				this.Update(up);
			}
		}
	}
}