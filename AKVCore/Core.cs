namespace AKVCore
{
	using System;
	using System.Data;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Globalization;
	using System.Threading;
	using System.Threading.Tasks;

	using ApS;
	using ApS.Firebird;
	using FirebirdSql.Data.FirebirdClient;

	public class Core
	{
		#region Fields/Properties
		#region public
		public static event EventHandler<ErrorEventArgs> ErrorOccured;
		public static Einstellungen CoreSettings = new Einstellungen();
		#endregion public
		#region private/protected

		#endregion private/protected
		#endregion Fields/Properties

		#region Constructors
		public Core()
		{
			FbConnectionStringBuilder fbConnString = new FbConnectionStringBuilder();
			fbConnString.ServerType = FbServerType.Embedded;
			fbConnString.UserID = "Melissa";
			fbConnString.Password = " ";

			fbConnString.Database = Services.GetAppDir() + @"\Daten\AKV.fdb";
			ApS.Settings.ConnectionString = fbConnString.ToString();
		}
		#endregion Constructors

		#region Methods
		#region public
		public void UpdateDatabase(Versionen vers)
		{
			switch (vers)
			{
				case Versionen.Version01:
					break;
				case Versionen.Version02:
					Datenbankversionen.Version02 v02 = new Datenbankversionen.Version02();
					v02.ErrorOccured += VersionenErrorOccured;
					v02.RunStatements();
					v02.ErrorOccured -= VersionenErrorOccured;
					CoreSettings.SetSetting("Version02Updated", true);
					break;
				case Versionen.Version03:
					break;
				case Versionen.Version04:
					break;
				case Versionen.Version05:
					Datenbankversionen.Version05 v05 = new Datenbankversionen.Version05();
					v05.ErrorOccured += VersionenErrorOccured;
					v05.RunStatements();
					v05.ErrorOccured -= VersionenErrorOccured;
					CoreSettings.SetSetting("Version05Updated", true);
					break;
				case Versionen.Version06:
					Datenbankversionen.Version06 v06 = new Datenbankversionen.Version06();
					v06.ErrorOccured += VersionenErrorOccured;
					v06.RunStatements();
					v06.ErrorOccured -= VersionenErrorOccured;
					CoreSettings.SetSetting("Version06Updated", true);
					break;
				case Versionen.Version06_1:
					Datenbankversionen.Version06_1 v06_1 = new Datenbankversionen.Version06_1();
					v06_1.ErrorOccured += VersionenErrorOccured;
					v06_1.RunStatements();
					v06_1.ErrorOccured -= VersionenErrorOccured;
					CoreSettings.SetSetting("Version06_1Updated", true);
					break;
				default:
					break;
			}
		}

		private void VersionenErrorOccured(object sender, ErrorInUpdate e)
		{
			ErrorOccured?.Invoke(this, new ErrorEventArgs(e.ErrorMessage));
		}

		public class KontoCore : Core
		{ 
			public string Name = "";
			public decimal Saldo = 0;
			public decimal Gebuehren = 0;
			public decimal ZinsenPA = 0;
			public decimal DispoPA = 0;

			public Konto GetAlleKonten()
			{
				Konto konto = new Konto();
				konto.Where = "Nummer is not null";
				konto.Read();

				return konto;
			}

			public void Add()
			{
				Konto konto = new Konto();
				konto.Where = "Name = '" + this.Name + "'";
				konto.Read();

				konto.Name = this.Name;
				konto.Saldo = this.Saldo;
				konto.Gebuehren = this.Gebuehren;
				konto.Zinsen_pa = this.ZinsenPA;
				konto.Dispo_pa = this.DispoPA;

				if (konto.EoF)
					konto.BuildSaveStmt(SqlAction.Insert);
				else
					ErrorOccured?.Invoke(this, new ErrorEventArgs("Es existiert bereits eine Kategorie mit diesem Namen!"));

				konto.Save();

				CoreSettings.SetSetting(this.Name + "_Gesamt", this.Saldo.DecToStr());
			}
			public void Delete(int konto_nr)
			{
				Konto konto = new Konto();
				konto.Where = "Nummer = " + konto_nr;
				konto.Save(SqlAction.Delete);
			}

			public decimal GesamtBetrag()
			{
				string x = CoreSettings.GetSetting(this.Name + "_Gesamt");
				bool negativ = x.StartsWith("-");
				if (negativ)
				{
					x = x.Substring(1);
					return decimal.Parse((string.IsNullOrEmpty(x) ? 0.ToString() : x), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) * -1;
				}
				else
					return decimal.Parse((string.IsNullOrEmpty(x) ? 0.ToString() : x), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
			}

			public void Edit(int konto_nr)
			{
				Konto konto = new Konto();
				konto.Where = "Nummer = " + konto_nr;
				konto.Read();

				CoreSettings.SetSetting(this.Name + "_Gesamt", CoreSettings.GetSetting(konto.Name + "_Gesamt"));

				konto.Name = this.Name;
				konto.Saldo = this.Saldo;
				konto.Gebuehren = this.Gebuehren;
				konto.Zinsen_pa = this.ZinsenPA;
				konto.Dispo_pa = this.DispoPA;

				konto.Save(SqlAction.Update);
			}

			public List<Kosten4Table> GetAlleKosten(int konto_nr)
			{
				List<Kosten4Table> rtn = new List<Kosten4Table>();

				Kosten kosten = new Kosten();
				kosten.Where = "Konto_Nr = " + konto_nr;
				kosten.Read();

				while (!kosten.EoF)
				{
					rtn.Add(new Kosten4Table(kosten));
					kosten.Skip();
				}

				return rtn;
			}
		}

		public class UnterKontoCore : Core
		{

			public string Name = "";
			public decimal Saldo = 0;
			public int Konto_Nr = 0;

			public UnterKonto GetAlleKonten()
			{
				UnterKonto konto = new UnterKonto();
				konto.Where = "Konto_Nr = " + this.Konto_Nr;
				konto.Read();

				return konto;
			}

			public void Add()
			{
				if (this.Konto_Nr == 0)
				{
					ErrorOccured?.Invoke(this, new ErrorEventArgs("Es wurde keine Kategorie ausgewählt, zu der diese Unterkategorie gehören soll!"));
					return;
				}
				UnterKonto konto = new UnterKonto();
				konto.Where = "Name = '" + this.Name + "' AND Konto_Nr = " + this.Konto_Nr;
				konto.Read();

				konto.Name = this.Name;
				konto.Saldo = this.Saldo;
				konto.Konto_Nr = this.Konto_Nr;

				if (konto.EoF)
					konto.BuildSaveStmt(SqlAction.Insert);
				else
					ErrorOccured?.Invoke(this, new ErrorEventArgs("Es existiert bereits eine Unterkategorie mit diesem Namen zu diesem Konto!"));

				konto.Save();

				CoreSettings.SetSetting(this.Name + "_Gesamt", this.Saldo.ToString());
			}
			public void Delete(int unterKonto_nr)
			{
				UnterKonto konto = new UnterKonto();
				konto.Where = "Nummer = " + unterKonto_nr;
				konto.Save(SqlAction.Delete);
			}

			public decimal GesamtBetrag()
			{
				string x = CoreSettings.GetSetting(this.Name + "_Gesamt");
				bool negativ = x.StartsWith("-");
				if (negativ)
				{
					x = x.Substring(1);
					return decimal.Parse((string.IsNullOrEmpty(x) ? 0.ToString() : x), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) * -1;
				}
				else
					return decimal.Parse((string.IsNullOrEmpty(x) ? 0.ToString() : x), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
			}

			public void Edit(int unterKonto_nr)
			{
				UnterKonto konto = new UnterKonto();
				konto.Where = "Nummer = " + unterKonto_nr;
				konto.Read();

				CoreSettings.SetSetting(this.Name + "_Gesamt", CoreSettings.GetSetting(konto.Name + "_Gesamt"));

				konto.Name = this.Name;
				konto.Saldo = this.Saldo;

				konto.Save(SqlAction.Update);
			}

			public List<Kosten4Table> GetAlleKosten(int unterKonto_nr)
			{
				List<Kosten4Table> rtn = new List<Kosten4Table>();

				Kosten kosten = new Kosten();
				kosten.Where = "UnterKonto_Nr = " + unterKonto_nr;
				kosten.Read();

				while (!kosten.EoF)
				{
					rtn.Add(new Kosten4Table(kosten));
					kosten.Skip();
				}

				return rtn;
			}
		}

		public class KostenCore : Core
		{
			public event EventHandler<EventArgs> KostenExist;
			public bool KostenExistProcessed = false;
			public bool KostenExistInsert = false;

			public string Bezeichnung = "";
			public decimal Betrag = 0;
			public int Intervall = -1;
			public bool Bezahlt = false;
			public DateTime BezahltAm = ApS.Settings.NullDate;
			public DateTime LaufzeitBis = ApS.Settings.NullDate;
			public bool Einnahme = false;
			public string Notiz = "";
			public int UnterKonto_Nr = -1;

			public void Add(int konto_nr)
			{
				Kosten kosten = new Kosten();
				kosten.Where = "Bezeichnung = '" + this.Bezeichnung + "' AND Konto_Nr = " + konto_nr;
				kosten.Read();

				if (!kosten.EoF)
				{
					this.KostenExist?.Invoke(this, new EventArgs());
					while (!this.KostenExistProcessed)
					{
						Thread.Sleep(500);
					}
					this.KostenExistProcessed = false;
					if (!this.KostenExistInsert)
						return;
				}

				if (this.Betrag.ToString().StartsWith("-"))
					this.Betrag *= -1;

				kosten.Bezeichnung = this.Bezeichnung;
				kosten.Betrag = this.Betrag;
				kosten.Intervall = this.Intervall;
				kosten.LaufzeitBis = this.LaufzeitBis;
				kosten.Einnahme = this.Einnahme;
				kosten.Notiz = this.Notiz;
				kosten.Konto_Nr = konto_nr;
				if (this.UnterKonto_Nr != -1)
					kosten.UnterKonto_Nr = this.UnterKonto_Nr;

				kosten.Save(SqlAction.Insert);

				if (this.Bezahlt)
				{
					kosten.Where = "Bezeichnung = '" + this.Bezeichnung + "' AND Konto_Nr = " + konto_nr + " AND Betrag = " + this.Betrag.DecToStr() + " AND Intervall = " + this.Intervall + " AND Bezahlt = 0 AND Einnahme = " + (this.Einnahme ? 1 : 0);
					kosten.Read();
					if (!kosten.EoF)
					{
						this.Pay(kosten.Nummer);
					}
				}
			}

			public void Pay(int kosten_nr)
			{
				Kosten kosten = new Kosten();
				kosten.Where = "Nummer = " + kosten_nr;
				kosten.Read();

				if (kosten.EoF)
				{
					ErrorOccured?.Invoke(this, new ErrorEventArgs("Kostensatz nicht gefunden."));
					return;
				}

				kosten.Bezahlt = true;
				kosten.BezahltAm = this.BezahltAm;
				kosten.Save(SqlAction.Update);

				Konto konto = new Konto();
				konto.Where = "Nummer = " + kosten.Konto_Nr;
				konto.Read();

				UnterKonto uKonto = null;
				if (kosten.UnterKonto_Nr != 0)
				{
					uKonto = new UnterKonto();
					uKonto.Where = "Nummer = " + kosten.UnterKonto_Nr;
					uKonto.Read();
				}

				if (!konto.EoF)
				{
					if (kosten.Einnahme)
					{
						konto.Saldo = konto.Saldo + kosten.Betrag;
						if (uKonto != null && uKonto.RecordCount == 1)
							uKonto.Saldo = uKonto.Saldo + kosten.Betrag;
					}
					else
					{
						konto.Saldo = konto.Saldo - kosten.Betrag;
						if (uKonto != null && uKonto.RecordCount == 1)
							uKonto.Saldo = uKonto.Saldo - kosten.Betrag;
					}
				}
				konto.Save(SqlAction.Update);
				if (uKonto != null && uKonto.RecordCount == 1)
					uKonto.Save(SqlAction.Update);
			}

			public void Delete(int kosten_nr)
			{
				Kosten kosten = new Kosten();
				kosten.Where = "Nummer = " + kosten_nr;
				kosten.Read();


				if (kosten.Bezahlt)
				{
					Konto konto = new Konto();
					konto.Where = "Nummer = " + kosten.Konto_Nr;
					konto.Read();

					UnterKonto uKonto = null;
					if (kosten.UnterKonto_Nr != 0)
					{
						uKonto = new UnterKonto();
						uKonto.Where = "Nummer = " + kosten.UnterKonto_Nr;
						uKonto.Read();
					}

					if (!konto.EoF)
					{
						if (kosten.Einnahme)
						{
							konto.Saldo = konto.Saldo + kosten.Betrag;
							if (uKonto != null && uKonto.RecordCount == 1)
								uKonto.Saldo = uKonto.Saldo + kosten.Betrag;
						}
						else
						{
							konto.Saldo = konto.Saldo - kosten.Betrag;
							if (uKonto != null && uKonto.RecordCount == 1)
								uKonto.Saldo = uKonto.Saldo - kosten.Betrag;
						}
					}
					konto.Save(SqlAction.Update);
					if (uKonto != null && uKonto.RecordCount == 1)
						uKonto.Save(SqlAction.Update);
				}
				kosten.Save(SqlAction.Delete);

			}
			
			public void Edit(int kosten_nr)
			{
				Kosten kosten = new Kosten();
				kosten.Where = "Nummer = " + kosten_nr;
				kosten.Read();

				Konto konto = new Konto();
				konto.Where = "Nummer = " + kosten.Konto_Nr;
				konto.Read();

				UnterKonto uKonto = null;
				if (kosten.UnterKonto_Nr != 0)
				{
					uKonto = new UnterKonto();
					uKonto.Where = "Nummer = " + kosten.UnterKonto_Nr;
					uKonto.Read();
				}

				if (this.Betrag.ToString().StartsWith("-"))
					this.Betrag *= -1;

				if (kosten.Bezahlt)
				{
					if (kosten.Einnahme)
					{
						konto.Saldo = konto.Saldo - kosten.Betrag;
						if (uKonto != null && uKonto.RecordCount == 1)
							uKonto.Saldo = uKonto.Saldo - kosten.Betrag;
					}
					else
					{
						konto.Saldo = konto.Saldo + kosten.Betrag;
						if (uKonto != null && uKonto.RecordCount == 1)
							uKonto.Saldo = uKonto.Saldo + kosten.Betrag;
					}
					konto.Save(SqlAction.Update);
					konto.Read();
					if (uKonto != null && uKonto.RecordCount == 1)
					{
						uKonto.Save(SqlAction.Update);
						uKonto.Read();
					}
				}


				kosten.Bezeichnung = this.Bezeichnung;
				kosten.Betrag = this.Betrag;
				kosten.BezahltAm = this.BezahltAm;
				kosten.LaufzeitBis = this.LaufzeitBis;
				kosten.Intervall = this.Intervall;
				kosten.Bezahlt = this.Bezahlt;
				if (string.IsNullOrEmpty(this.Notiz))
					kosten.Notiz = null;
				else
					kosten.Notiz = this.Notiz;
				kosten.Einnahme = this.Einnahme;
				if (this.UnterKonto_Nr != -1)
					kosten.UnterKonto_Nr = this.UnterKonto_Nr;

				kosten.Save(SqlAction.Update);
				kosten.Read();

				if (kosten.Bezahlt)
				{
					if (kosten.Einnahme)
					{
						konto.Saldo = konto.Saldo + kosten.Betrag;
						if (uKonto != null && uKonto.RecordCount == 1)
							uKonto.Saldo = uKonto.Saldo + kosten.Betrag;
					}
					else
					{
						konto.Saldo = konto.Saldo - kosten.Betrag;
						if (uKonto != null && uKonto.RecordCount == 1)
							uKonto.Saldo = uKonto.Saldo - kosten.Betrag;
					}
					konto.Save(SqlAction.Update);
					if (uKonto != null && uKonto.RecordCount == 1)
						uKonto.Save(SqlAction.Update);
				}
			}

			public int GetUnterKontoNummer(int konto_nr, string unterKontoName)
			{
				UnterKonto konto = new UnterKonto();
				konto.Where = "Konto_Nr = " + konto_nr + " AND Name = '" + unterKontoName + "'";
				konto.Read();

				if (!konto.EoF)
					return konto.Nummer;
				else
					return -1;
			}
		}

		#endregion public
		#region private/protected

		#endregion private/protected
		#endregion Methods
	}

	public class ErrorEventArgs : ErrorInUpdate
	{
		public ErrorEventArgs(string errorMsg) : base(errorMsg)
		{

		}
	}

	public enum Versionen
	{
		Version01,
		Version02,
		Version03,
		Version04,
		Version05,
		Version06,
		Version06_1
	}
}