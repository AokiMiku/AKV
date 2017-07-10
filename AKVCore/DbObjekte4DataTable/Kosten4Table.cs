namespace AKVCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Kosten4Table
	{
		private int nummer;
		private string bezeichnung;
		private bool bezahlt;
		private decimal betrag;
		private string bezahltAm;
		private string bezahlenBis;
		private bool einnahme;
		private string notiz;
		private int unterKonto_nr;
		private int intervall;
		private IntervallEinheiten einheit;
		public int Nummer
		{
			get { return this.nummer; }
			private set { this.nummer = value; }
		}
		public string Bezeichnung
		{
			get { return this.bezeichnung; }
			private set { this.bezeichnung = value; }
		}
		public bool Bezahlt
		{
			get { return this.bezahlt; }
			private set { this.bezahlt = value; }
		}
		public decimal Betrag
		{
			get { return Math.Round(this.betrag, 2); }
			private set { this.betrag = value; }
		}
		public string BezahltAm
		{
			get
			{
				if (string.IsNullOrEmpty(this.bezahltAm))
					return "--.--.----";
				return this.bezahltAm;
			}
			private set { this.bezahltAm = value; }
		}
		public string BezahlenBis
		{
			get
			{
				if (string.IsNullOrEmpty(this.bezahlenBis))
					return "--.--.----";
				return this.bezahlenBis;
			}
			private set { this.bezahlenBis = value; }
		}
		public string Intervall
		{
			get
			{
				switch (this.einheit)
				{
					case IntervallEinheiten.AlleXTage:
					case IntervallEinheiten.AlleXWochen:
					case IntervallEinheiten.AlleXMonate:
					case IntervallEinheiten.AlleXJahre:
						if (this.intervall == 1)
							return this.einheit.EinheitToSingularString();
						else
							return this.einheit.EinheitToString().Replace(" X ", " " + this.intervall + " ");
					case IntervallEinheiten.Januar:
					case IntervallEinheiten.Februar:
					case IntervallEinheiten.März:
					case IntervallEinheiten.April:
					case IntervallEinheiten.Mai:
					case IntervallEinheiten.Juni:
					case IntervallEinheiten.Juli:
					case IntervallEinheiten.August:
					case IntervallEinheiten.September:
					case IntervallEinheiten.Oktober:
					case IntervallEinheiten.November:
					case IntervallEinheiten.Dezember:
						return this.intervall + this.einheit.EinheitToString();
					case IntervallEinheiten.Null:
					default:
						return "";
				}
			}
		}
		public string Art
		{
			get
			{
				if (this.einnahme)
					return "Einnahme";
				else
					return "Ausgabe";
			}
		}
		public string UnterKategorie
		{
			get
			{
				UnterKonto konto = new UnterKonto();
				konto.Where = "Nummer = " + this.unterKonto_nr;
				konto.Read();

				if (!konto.EoF)
					return konto.Name;
				else
					return "";
			}
		}
		public string Notiz
		{
			get
			{
				return this.notiz;
			}
			private set { this.notiz = value; }
		}

		public Kosten4Table(Kosten kosten)
		{
			this.Nummer = kosten.Nummer;
			this.Bezeichnung = kosten.Bezeichnung;
			this.Bezahlt = kosten.Bezahlt;
			this.Betrag = kosten.Betrag;
			if (kosten.BezahltAm != ApS.Settings.NullDate)
				this.BezahltAm = kosten.BezahltAm.ToShortDateString();
			if (kosten.LaufzeitBis != ApS.Settings.NullDate)
				this.BezahlenBis = kosten.LaufzeitBis.ToShortDateString();
			this.einnahme = kosten.Einnahme || kosten.Betrag.ToString().StartsWith("-"); //für Daten, die erfasst wurden vor Version 0.5, auf - prüfen
			this.Notiz = kosten.Notiz;
			this.unterKonto_nr = kosten.UnterKonto_Nr;
			this.intervall = kosten.Intervall;
			if (this.intervall > 0)
				this.einheit = (IntervallEinheiten)kosten.IntervallEinheit;
			else
				this.einheit = IntervallEinheiten.Null;
		}
	}
}