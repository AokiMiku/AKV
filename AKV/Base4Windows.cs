namespace AKV
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public class Base4Windows : Window
	{
		protected AKVCore.Core core = new AKVCore.Core();
		protected FensterModus modus = FensterModus.Neu;

		public Base4Windows()
		{
			this.Closing += Window_Closing;
			this.Loaded += Window_Loaded;
			//this.GotFocus += Window_GotFocus;
			ApS.Settings.ErrorLogFile = ApS.Services.GetAppDir() + "\\Logs\\Error_" + ApS.Services.GetTimeStamp() + ".log";
			Application.Current.DispatcherUnhandledException += UnhandledException;

			AKVCore.Core.ErrorOccured += Core_ErrorOccured;
		}

		private void Core_ErrorOccured(object sender, AKVCore.ErrorEventArgs e)
		{
			MessageBox.Show(this, e.ErrorMessage, "Fehler", MessageBoxButton.OK);
		}

		private void UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			ApS.Services.WriteErrorLog(e.Exception);
			MessageBox.Show(this, e.Exception.Message + Environment.NewLine + "Details wurden in ein Errorlog geschrieben.", "Fehler", MessageBoxButton.OK);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				this.SpeicherFensterInformationen();
			}
			catch (Exception ex)
			{
				if (ApS.Services.GetAppDir().ToUpper().Contains("DEBUG"))
					MessageBox.Show(this, ex.Message + Environment.NewLine + ex.StackTrace, "Fehler", MessageBoxButton.OK);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.LadeFensterInformationen();
			}
			catch (Exception ex)
			{
				if (ApS.Services.GetAppDir().ToUpper().Contains("DEBUG"))
					MessageBox.Show(this, ex.Message + Environment.NewLine + ex.StackTrace, "Fehler", MessageBoxButton.OK);
			}
		}
	}
}