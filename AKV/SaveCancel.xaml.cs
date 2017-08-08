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

namespace AKV
{
	/// <summary>
	/// Interaktionslogik für SaveCancel.xaml
	/// </summary>
	public partial class SaveCancel : UserControl
	{
		public event EventHandler<RoutedEventArgs> Speichern;
		public event EventHandler<RoutedEventArgs> Abbrechen;

		public SaveCancel()
		{
			InitializeComponent();
		}
		
		private void speichern_Click(object sender, RoutedEventArgs e)
		{
			this.Speichern?.Invoke(sender, e);
		}

		private void abbrechen_Click(object sender, RoutedEventArgs e)
		{
			this.Abbrechen?.Invoke(sender, e);
		}
	}
}