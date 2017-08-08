namespace AKV
{
	using System;
	using System.Windows.Input;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	using AKV;
	using AKVCore;

	public static class AKVWindowExtensions
	{
		public static void SpeicherFensterInformationen(this Window Fenster)
		{
			Fenster.SpeicherFensterGroesse();
			Fenster.SpeicherFensterPosition();
		}

		public static void SpeicherFensterGroesse(this Window Fenster)
		{
			Core.CoreSettings.SetSetting(Fenster.ToString() + "_Measurements_Width", Fenster.Width.ToString());
			Core.CoreSettings.SetSetting(Fenster.ToString() + "_Measurements_Height", Fenster.Height.ToString());
		}

		public static void SpeicherFensterPosition(this Window Fenster)
		{
			Core.CoreSettings.SetSetting(Fenster.ToString() + "_Position_X", Fenster.Top.ToString());
			Core.CoreSettings.SetSetting(Fenster.ToString() + "_Position_Y", Fenster.Left.ToString());
		}

		public static bool LadeFensterInformationen(this Window Fenster)
		{
			Fenster.LadeFensterGroesse();
			return Fenster.LadeFensterPosition();
		}

		public static void LadeFensterGroesse(this Window Fenster)
		{
			double Width = 0;
			double Height = 0;
			if (double.TryParse(Core.CoreSettings.GetSetting(Fenster.ToString() + "_Measurements_Width"), out Width))
				Fenster.Width = Width;
			if (double.TryParse(Core.CoreSettings.GetSetting(Fenster.ToString() + "_Measurements_Height"), out Height))
				Fenster.Height = Height;
		}

		public static bool LadeFensterPosition(this Window Fenster)
		{
			double Top = 0;
			double Left = 0;
			if (double.TryParse(Core.CoreSettings.GetSetting(Fenster.ToString() + "_Position_X"), out Top))
				Fenster.Top = Top;
			if (double.TryParse(Core.CoreSettings.GetSetting(Fenster.ToString() + "_Position_Y"), out Left))
				Fenster.Left = Left;

			if (Top == 0 || Left == 0)
				return false;
			else
				return true;
		}

		public static bool KeyIsNumericOrDecimal(this Key key)
		{
			Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
								Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9,
								Key.Decimal};

			if (keys.Contains(key))
				return true;
			else
				return false;
		}
	}

	public enum FensterModus
	{
		Neu,
		Edit
	}
}

namespace System.Windows.Controls
{
    public static class Extensions
    {
         public static void PerformClick(this Button btn)
         {
             btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
         }
    }
}