namespace AKV
{
	using System.Windows;

	using AKVCore;
	/// <summary>
	/// Interaktionslogik für Updater.xaml
	/// </summary>
	public partial class Updater : Window
	{
		private Core.Updater updater;

		public Updater()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

		}

		public void Start(Core.Updater updater)
		{
			this.updater = updater;
			this.updater.UpdateProgressChanged += Core_UpdateProgressChanged;
			this.updater.DownloadCompleted += Updater_DownloadCompleted;
			this.ShowDialog();
		}

		private void Updater_DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			this.updater.InstallUpdate();
			Application.Current.Shutdown();
		}

		private void Core_UpdateProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
		{
			this.progressUpdater.Value = e.ProgressPercentage;
		}

		private void progressUpdater_Loaded(object sender, RoutedEventArgs e)
		{
			this.updater.DownloadUpdateAsync();
		}
	}
}