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
	using System.Windows.Navigation;
	using System.Windows.Shapes;

	using ApS;
	using AKVCore;
	using System.Windows.Controls.Primitives;

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Base4Windows
	{
		private int currentKonto_nr = 0;

		private int currentUnterKonto_nr = 0;

		public MainWindow()
		{
			InitializeComponent();
			if (!Core.CoreSettings.GetSetting("Version02Updated").ToBoolean())
			{
				this.core.UpdateDatabase(Versionen.Version02);
			}
			if (!Core.CoreSettings.GetSetting("Version05Updated").ToBoolean())
			{
				this.core.UpdateDatabase(Versionen.Version05);
			}
			if (!Core.CoreSettings.GetSetting("Version06Updated").ToBoolean())
			{
				this.core.UpdateDatabase(Versionen.Version06);
			}
			if (!Core.CoreSettings.GetSetting("Version06_1Updated").ToBoolean())
			{
				this.core.UpdateDatabase(Versionen.Version06_1);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.ActualizeKonten();
			this.Actualize();

			if (UserSettings.BeimStartKontoOeffnen && !string.IsNullOrEmpty(UserSettings.ZuletztOffenesKonto))
			{
				this.konten.SelectedItem = UserSettings.ZuletztOffenesKonto;
			}
		}

		private void ActualizeKonten()
		{
			this.konten.Items.Clear();
			Core.KontoCore core = new Core.KontoCore();
			Konto konto = core.GetAlleKonten();
			while (!konto.EoF)
			{
				this.konten.Items.Add(konto.Name);
				konto.Skip();
			}
		}

		private void ActualizeUnterKonten()
		{
			object select = this.unterKonten.SelectedItem;
			this.unterKonten.Items.Clear();
			Core.UnterKontoCore core = new Core.UnterKontoCore();
			core.Konto_Nr = this.currentKonto_nr;
			UnterKonto konto = core.GetAlleKonten();
			while (!konto.EoF)
			{
				this.unterKonten.Items.Add(konto.Name);
				konto.Skip();
			}
			this.unterKonten.SelectedItem = select;
		}

		private void konten_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.konten.SelectedItem != null)
			{
				Konto konto = new Konto();
				konto.Where = "Name = '" + this.konten.SelectedItem + "'";
				konto.Read();

				this.currentKonto_nr = konto.Nummer;

				this.Actualize();
			}
		}

		private void Actualize()
		{
			if (!UserSettings.UnterKonten && this.frontend.RowDefinitions[1].Height == new GridLength(50))
			{
				this.frontend.RowDefinitions[1].Height = new GridLength(0);
			}
			else if (this.frontend.RowDefinitions[1].Height == new GridLength(0))
			{
				this.frontend.RowDefinitions[1].Height = new GridLength(50);
				this.ActualizeUnterKonten();
			}

			if (this.konten.SelectedItem != null)
			{
				this.verblBetrag.Visibility = Visibility.Visible;
				this.betrag.Visibility = Visibility.Visible;
				this.gesBetrag.Visibility = Visibility.Visible;
				this.gesamtBetrag.Visibility = Visibility.Visible;
				this.editKonto.IsEnabled = true;
				this.delKonto.IsEnabled = true;
				Core.KontoCore kontoCore = new Core.KontoCore();
				this.kosten.ItemsSource = kontoCore.GetAlleKosten(this.currentKonto_nr);
				this.addKosten.IsEnabled = true;

				this.kosten.Columns.Where(x => x.Header.ToString() == "Nummer").First().Visibility = Visibility.Hidden;

				Konto konto = new Konto();
				konto.Where = "Nummer = " + this.currentKonto_nr;
				konto.Read();

				if (!konto.EoF)
				{
					if (konto.Saldo > 0)
					{
						this.betrag.Foreground = Brushes.Red;
						this.verblBetrag.Foreground = Brushes.Red;
					}
					else
					{
						this.betrag.Foreground = Brushes.LightGreen;
						this.verblBetrag.Foreground = Brushes.LightGreen;
					}
					kontoCore.Name = konto.Name;
					this.betrag.Content = konto.Saldo.ToString("0.00 €");
					this.gesamtBetrag.Content = kontoCore.GesamtBetrag().ToString("0.00 €");
				}

				if (UserSettings.UnterKonten)
				{
					Core.UnterKontoCore uKontoCore = new Core.UnterKontoCore();
					if (this.unterKonten.SelectedItem != null)
					{
						this.editUnterKonto.IsEnabled = true;
						this.delUnterKonto.IsEnabled = true;

						if (UserSettings.KostenPerUnterKonto)
						{
							this.kosten.ItemsSource = uKontoCore.GetAlleKosten(this.currentUnterKonto_nr);
						}

						if (UserSettings.UnterKontoSummieren)
						{
							UnterKonto uKonto = new UnterKonto();
							uKonto.Where = "Nummer = " + this.currentUnterKonto_nr;
							uKonto.Read();

							if (!uKonto.EoF)
							{
								if (uKonto.Saldo > 0)
								{
									this.betrag.Foreground = Brushes.Red;
									this.verblBetrag.Foreground = Brushes.Red;
								}
							}
							else
							{
								this.betrag.Foreground = Brushes.LightGreen;
								this.verblBetrag.Foreground = Brushes.LightGreen;
							}
							uKontoCore.Name = uKonto.Name;
							this.betrag.Content = uKonto.Saldo.ToString("0.00 €");
							this.gesamtBetrag.Content = uKontoCore.GesamtBetrag().ToString("0.00 €");
						}
					}
				}
				else
				{
					this.kosten.Columns.Where(x => x.Header.ToString() == "UnterKategorie").First().Visibility = Visibility.Hidden;
				}
			}
			else
			{
				this.editKonto.IsEnabled = false;
				this.delKonto.IsEnabled = false;
				this.editUnterKonto.IsEnabled = false;
				this.delUnterKonto.IsEnabled = false;
				this.kosten.ItemsSource = null;
				this.addKosten.IsEnabled = false;
				this.verblBetrag.Visibility = Visibility.Hidden;
				this.betrag.Visibility = Visibility.Hidden;
				this.gesBetrag.Visibility = Visibility.Hidden;
				this.gesamtBetrag.Visibility = Visibility.Hidden;
			}
		}

		private void kostenBezahlen_Click(object sender, RoutedEventArgs e)
		{
			if (!this.kosten.HasItems || this.kosten.SelectedItem == null)
				return;

			new KostensatzBezahlen().Start(((Kosten4Table)this.kosten.SelectedItem).Nummer);

			this.Actualize();
		}

		private void kosten_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.kostenBezahlen.IsEnabled = (this.kosten.SelectedItem != null && !((Kosten4Table)this.kosten.SelectedItem).Bezahlt);
			if (this.kosten.SelectedItem != null)
			{
				this.editKosten.IsEnabled = true;
				this.delKosten.IsEnabled = true;
			}
			else
			{
				this.editKosten.IsEnabled = false;
				this.delKosten.IsEnabled = false;
			}
		}

		private void unterKonten_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.unterKonten.SelectedItem != null)
			{
				UnterKonto konto = new UnterKonto();
				konto.Where = "Name = '" + this.unterKonten.SelectedItem + "'";
				konto.Read();

				this.currentUnterKonto_nr = konto.Nummer;

				this.Actualize();
			}
		}

		private void addKonto_Click(object sender, RoutedEventArgs e)
		{
			string name = new NeuesKonto().Start();

			this.ActualizeKonten();
			this.konten.SelectedItem = name;
		}

		private void editKonto_Click(object sender, RoutedEventArgs e)
		{
			int i = this.konten.SelectedIndex;
			new NeuesKonto().Start(this.currentKonto_nr);

			this.ActualizeKonten();
			this.konten.SelectedIndex = i;
		}

		private void delKonto_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(this, "Möchten Sie diese Kategorie wirklich löschen?", "", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				Core.KontoCore core = new Core.KontoCore();
				core.Delete(this.currentKonto_nr);
			}
			else
				return;

			this.ActualizeKonten();
			this.Actualize();
		}

		private void addKosten_Click(object sender, RoutedEventArgs e)
		{
			new NeuerKostensatz().Start(this.currentKonto_nr);

			this.Actualize();
		}

		private void editKosten_Click(object sender, RoutedEventArgs e)
		{
			new NeuerKostensatz().Start(this.currentKonto_nr, ((Kosten4Table)this.kosten.SelectedItem).Nummer);

			this.Actualize();
		}

		private void delKosten_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(this, "Möchten Sie diesen Kostensatz wirklich löschen?", "", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				Core.KostenCore core = new Core.KostenCore();
				core.Delete(((Kosten4Table)this.kosten.SelectedItem).Nummer);
			}
			else
				return;

			int i = this.konten.SelectedIndex;
			this.ActualizeKonten();
			this.konten.SelectedIndex = i;
			this.Actualize();
		}

		private void addUnterKonto_Click(object sender, RoutedEventArgs e)
		{
			string name = new NeuesUnterKonto().Start(this.currentKonto_nr);

			int i = this.konten.SelectedIndex;
			this.ActualizeKonten();
			this.konten.SelectedIndex = i;
			this.ActualizeUnterKonten();
			this.unterKonten.SelectedItem = name;
		}

		private void editUnterKonto_Click(object sender, RoutedEventArgs e)
		{
			int i = this.unterKonten.SelectedIndex;
			new NeuesUnterKonto().Start(this.currentKonto_nr, this.currentUnterKonto_nr);

			this.ActualizeUnterKonten();
			this.unterKonten.SelectedIndex = i;
		}

		private void delUnterKonto_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(this, "Möchten Sie diese Unter-Kategorie wirklich löschen?", "", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				Core.UnterKontoCore core = new Core.UnterKontoCore();
				core.Delete(this.currentUnterKonto_nr);
			}
			else
				return;

			this.ActualizeUnterKonten();
			this.Actualize();
		}

		private void kosten_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => SetBackground(e.Row)));
		}

		private void SetBackground(DataGridRow Row)
		{
			Kosten4Table item = Row.Item as Kosten4Table;

			if (item == null)
				return;

			if (!item.Bezahlt && (item.BezahlenBis != "--.--.----" && DateTime.Parse(item.BezahlenBis) > DateTime.Now.Date))
				Row.Background = Brushes.Yellow;
			else if (!item.Bezahlt && (item.BezahlenBis != "--.--.----" && DateTime.Parse(item.BezahlenBis) == DateTime.Now.Date))
				Row.Background = Brushes.Yellow;
			else if (!item.Bezahlt && (item.BezahlenBis != "--.--.----" && DateTime.Parse(item.BezahlenBis) > DateTime.Now.Date))
				Row.Background = Brushes.Red;
			else if (!item.Bezahlt && item.BezahlenBis == "--.--.----")
				Row.Background = Brushes.Yellow;
			else if (item.Bezahlt)
				Row.Background = Brushes.LightGreen;
		}

		private void einstellungen_Click(object sender, RoutedEventArgs e)
		{
			Einstellungen einstellungen = new Einstellungen();
			einstellungen.ShowDialog();

			this.Actualize();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (UserSettings.BeimStartKontoOeffnen && this.konten.SelectedItem != null)
			{
				UserSettings.ZuletztOffenesKonto = this.konten.SelectedItem.ToString();
			}
			UserSettings.LetzterStartAm = DateTime.Now.Date;
		}
	}
}