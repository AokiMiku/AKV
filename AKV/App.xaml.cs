using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Policy;

namespace AKV
{
	/// <summary>
	/// Interaktionslogik für "App.xaml"
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			ApS.Version.MajorVersion = 0;
			ApS.Version.MinorVersion = 7;
			ApS.Version.PatchNumber = 5;
		}
	}
}
