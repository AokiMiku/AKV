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
	using System.Net;
	using System.Diagnostics;
	using System.IO;
	using System.IO.Compression;

	using ApS;
	using ApS.Databases;
	using ApS.Databases.Firebird;
	using FirebirdSql.Data.FirebirdClient;

	public class Core
	{
		#region Fields/Properties
		#region public
		public static event EventHandler<ErrorEventArgs> ErrorOccured;
		public static event EventHandler<EventArgs> SaveCompleted;
		public static Einstellungen CoreSettings = new Einstellungen();
		#endregion public
		#region private/protected

		#endregion private/protected
		#endregion Fields/Properties

		#region Constructors
		public Core()
		{
			FbConnectionStringBuilder fbConnString = new FbConnectionStringBuilder
			{
				ServerType = FbServerType.Embedded,
				UserID = "Melissa",
				Password = " ",

				Database = Services.GetAppDir() + @"\Daten\AKV.fdb"
			};
			ApS.Databases.Settings.ConnectionString = fbConnString.ToString();

			if (File.Exists(Services.GetAppDir() + @"\Update.exe"))
				File.Delete(Services.GetAppDir() + @"\Update.exe");
		}
		#endregion Constructors

		#region Methods
		#region public
		public class KontoCore
		{
			public string Name = "";
			public decimal Saldo = 0;
			public decimal Gebuehren = 0;
			public decimal ZinsenPA = 0;
			public decimal DispoPA = 0;
			public string Notiz = "";
			public bool Schuldkonto = false;

			public Konto GetAlleKonten()
			{
				Konto konto = new Konto
				{
					Where = "Nummer is not null"
				};
				konto.Read();

				return konto;
			}

			public void Add()
			{
				Konto konto = new Konto
				{
					//Async = true;
					//AsyncCompleted += Konto_AsyncCompleted;
					Where = "Name = '" + this.Name + "'"
				};
				konto.Read();

				if (this.Schuldkonto && !this.Saldo.ToString().StartsWith("-"))
					this.Saldo *= -1;

				konto.Name = this.Name;
				konto.Saldo = this.Saldo;
				konto.Gebuehren = this.Gebuehren;
				konto.Zinsen_pa = this.ZinsenPA;
				konto.Dispo_pa = this.DispoPA;
				if (this.Schuldkonto)
					this.Notiz = "Schuldkonto" + Environment.NewLine + this.Notiz;
				konto.Notiz = this.Notiz;
				konto.Schuldkonto = this.Schuldkonto;

				if (konto.EoF)
					konto.BuildSaveStmt(SqlAction.Insert);
				else
					ErrorOccured?.Invoke(this, new ErrorEventArgs("Es existiert bereits eine Kategorie mit diesem Namen!"));

				CoreSettings.SetSetting(this.Name + "_Gesamt", this.Saldo.DecToStr());

				konto.Save();
			}

			private void Konto_AsyncCompleted(object sender, BusinessAsyncCompleted e)
			{
				Core.SaveCompleted?.Invoke(this, new EventArgs());
			}

			public void Delete(int konto_nr)
			{
				Konto konto = new Konto
				{
					//Async = true;
					//AsyncCompleted += Konto_AsyncCompleted;
					Where = "Nummer = " + konto_nr
				};
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
				Konto konto = new Konto
				{
					//Async = true;
					//AsyncCompleted += Konto_AsyncCompleted;
					Where = "Nummer = " + konto_nr
				};
				konto.Read();

				if (this.Schuldkonto && !this.Saldo.ToString().StartsWith("-"))
					this.Saldo *= -1;

				CoreSettings.SetSetting(this.Name + "_Gesamt", CoreSettings.GetSetting(konto.Name + "_Gesamt"));

				konto.Name = this.Name;
				konto.Saldo = this.Saldo;
				konto.Gebuehren = this.Gebuehren;
				konto.Zinsen_pa = this.ZinsenPA;
				konto.Dispo_pa = this.DispoPA;
				konto.Notiz = this.Notiz;
				konto.Schuldkonto = this.Schuldkonto;

				konto.Save(SqlAction.Update);
			}

			public List<Kosten4Table> GetAlleKosten(int konto_nr)
			{
				List<Kosten4Table> rtn = new List<Kosten4Table>();

				Kosten kosten = new Kosten
				{
					Where = "Konto_Nr = " + konto_nr
				};
				kosten.Read();

				while (!kosten.EoF)
				{
					rtn.Add(new Kosten4Table(kosten));
					kosten.Skip();
				}

				return rtn;
			}
		}

		public class UnterKontoCore
		{
			public string Name = "";
			public decimal Saldo = 0;
			public int Konto_Nr = 0;

			public UnterKonto GetAlleKonten()
			{
				UnterKonto konto = new UnterKonto
				{
					Where = "Konto_Nr = " + this.Konto_Nr
				};
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

				using (Konto k = new Konto())
				{
					k.Where = "Nummer = " + this.Konto_Nr;
					k.Read();

					if (!k.EoF)
					{
						if (k.Schuldkonto && !this.Saldo.ToString().StartsWith("-"))
							this.Saldo *= -1;
					}
				}

				UnterKonto konto = new UnterKonto
				{
					//Async = true;
					//AsyncCompleted += Konto_AsyncCompleted;
					Where = "Name = '" + this.Name + "' AND Konto_Nr = " + this.Konto_Nr
				};
				konto.Read();

				konto.Name = this.Name;
				konto.Saldo = this.Saldo;
				konto.Konto_Nr = this.Konto_Nr;

				if (konto.EoF)
					konto.BuildSaveStmt(SqlAction.Insert);
				else
					ErrorOccured?.Invoke(this, new ErrorEventArgs("Es existiert bereits eine Unterkategorie mit diesem Namen zu diesem Konto!"));

				CoreSettings.SetSetting(this.Name + "_Gesamt", this.Saldo.ToString());

				konto.Save();
			}

			private void Konto_AsyncCompleted(object sender, BusinessAsyncCompleted e)
			{
				Core.SaveCompleted?.Invoke(this, new EventArgs());
			}

			public void Delete(int unterKonto_nr)
			{
				UnterKonto konto = new UnterKonto
				{
					//Async = true;
					//AsyncCompleted += Konto_AsyncCompleted;
					Where = "Nummer = " + unterKonto_nr
				};
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
				UnterKonto konto = new UnterKonto
				{
					//Async = true;
					//AsyncCompleted += Konto_AsyncCompleted;
					Where = "Nummer = " + unterKonto_nr
				};
				konto.Read();

				using (Konto k = new Konto())
				{
					k.Where = "Nummer = " + this.Konto_Nr;
					k.Read();

					if (!k.EoF)
					{
						if (k.Schuldkonto && !this.Saldo.ToString().StartsWith("-"))
							this.Saldo *= -1;
					}
				}

				CoreSettings.SetSetting(this.Name + "_Gesamt", CoreSettings.GetSetting(konto.Name + "_Gesamt"));

				konto.Name = this.Name;
				konto.Saldo = this.Saldo;

				konto.Save(SqlAction.Update);
			}

			public List<Kosten4Table> GetAlleKosten(int unterKonto_nr)
			{
				List<Kosten4Table> rtn = new List<Kosten4Table>();

				Kosten kosten = new Kosten
				{
					Where = "UnterKonto_Nr = " + unterKonto_nr
				};
				kosten.Read();

				while (!kosten.EoF)
				{
					rtn.Add(new Kosten4Table(kosten));
					kosten.Skip();
				}

				return rtn;
			}
		}

		public class KostenCore
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
			public IntervallEinheiten IntervallEinheit;

			public void Add(int konto_nr)
			{
				Kosten kosten = new Kosten
				{
					//kosten.Async = true;
					//kosten.AsyncCompleted += Kosten_AsyncCompleted;
					Where = "Bezeichnung = '" + this.Bezeichnung + "' AND Konto_Nr = " + konto_nr
				};
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
				kosten.IntervallEinheit = (int)IntervallEinheit;
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

			private void Kosten_AsyncCompleted(object sender, BusinessAsyncCompleted e)
			{
				Core.SaveCompleted?.Invoke(this, new EventArgs());
			}

			public void Pay(int kosten_nr)
			{
				Kosten kosten = new Kosten
				{
					Where = "Nummer = " + kosten_nr
				};
				kosten.Read();

				if (kosten.EoF)
				{
					ErrorOccured?.Invoke(this, new ErrorEventArgs("Kostensatz nicht gefunden."));
					return;
				}

				kosten.Bezahlt = true;
				kosten.BezahltAm = this.BezahltAm;
				kosten.Save(SqlAction.Update);
				ReadData(kosten, out Konto konto, out UnterKonto uKonto);

				if (!konto.EoF)
				{
					Saldieren(kosten, konto, uKonto);
				}
				SaveKonto(konto, uKonto);
			}

			private static void ReadData(Kosten kosten, out Konto konto, out UnterKonto uKonto)
			{
				konto = new Konto
				{
					//konto.Async = true;
					//konto.AsyncCompleted += Kosten_AsyncCompleted;
					Where = "Nummer = " + kosten.Konto_Nr
				};
				konto.Read();

				uKonto = null;
				if (kosten.UnterKonto_Nr != 0)
				{
					uKonto = new UnterKonto
					{
						Where = "Nummer = " + kosten.UnterKonto_Nr
					};
					uKonto.Read();
				}
			}

			public void Delete(int kosten_nr)
			{
				Kosten kosten = new Kosten
				{
					//Async = true;
					//AsyncCompleted += Kosten_AsyncCompleted;
					Where = "Nummer = " + kosten_nr
				};
				kosten.Read();

				if (kosten.Bezahlt)
				{
					ReadData(kosten, out Konto konto, out UnterKonto uKonto);

					if (!konto.EoF)
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
					}
					SaveKonto(konto, uKonto);
				}
				kosten.Save(SqlAction.Delete);
			}

			private static void SaveKonto(Konto konto, UnterKonto uKonto)
			{
				konto.Save(SqlAction.Update);
				if (uKonto != null && uKonto.RecordCount == 1)
					uKonto.Save(SqlAction.Update);
			}

			public void Edit(int kosten_nr)
			{
				Kosten kosten = new Kosten
				{
					Where = "Nummer = " + kosten_nr
				};
				kosten.Read();

				Konto konto = new Konto
				{
					//Async = true;
					//AsyncCompleted += Kosten_AsyncCompleted;
					Where = "Nummer = " + kosten.Konto_Nr
				};
				konto.Read();

				UnterKonto uKonto = null;
				if (kosten.UnterKonto_Nr != 0)
				{
					uKonto = new UnterKonto
					{
						Where = "Nummer = " + kosten.UnterKonto_Nr
					};
					uKonto.Read();
				}

				if (this.Betrag.ToString().StartsWith("-"))
					this.Betrag *= -1;

				if (kosten.Bezahlt)
				{
					BackSaldieren(kosten, konto, uKonto);
					konto.Save(SqlAction.Update);
					konto.Read();
					SaveUKonto(uKonto);
				}

				kosten.Bezeichnung = this.Bezeichnung;
				kosten.Betrag = this.Betrag;
				kosten.BezahltAm = this.BezahltAm;
				kosten.LaufzeitBis = this.LaufzeitBis;
				kosten.Intervall = this.Intervall;
				kosten.IntervallEinheit = (int)IntervallEinheit;
				kosten.Bezahlt = this.Bezahlt;
				kosten.Notiz = this.Notiz;
				kosten.Einnahme = this.Einnahme;
				if (this.UnterKonto_Nr != -1)
					kosten.UnterKonto_Nr = this.UnterKonto_Nr;

				kosten.Save(SqlAction.Update);
				kosten.Read();

				if (kosten.Bezahlt)
				{
					Saldieren(kosten, konto, uKonto);
					SaveKonto(konto, uKonto);
				}
			}

			private static void SaveUKonto(UnterKonto uKonto)
			{
				if (uKonto != null && uKonto.RecordCount == 1)
				{
					uKonto.Save(SqlAction.Update);
					uKonto.Read();
				}
			}

			private static void BackSaldieren(Kosten kosten, Konto konto, UnterKonto uKonto)
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
			}

			private static void Saldieren(Kosten kosten, Konto konto, UnterKonto uKonto)
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

			public int GetUnterKontoNummer(int konto_nr, string unterKontoName)
			{
				UnterKonto konto = new UnterKonto
				{
					Where = "Konto_Nr = " + konto_nr + " AND Name = '" + unterKontoName + "'"
				};
				konto.Read();

				if (!konto.EoF)
					return konto.Nummer;
				else
					return -1;
			}
		}

		public static class Initializer
		{
			public static event EventHandler<ProgressEventArgs> ProgressChanged;
			public static event EventHandler<EventArgs> InitilizationFinished;

			public static void SetAllAsIntervall()
			{
				Kosten kosten = new Kosten
				{
					Where = "Intervall <> -1 AND (EXTRACT(YEAR FROM LaufzeitBis) >= '" + DateTime.Now.Year + "' OR LaufzeitBis >= '" + DateTime.Now.AddMonths(-6).ToShortDateString() + "') AND Bezahlt = 1"
				};
				kosten.Read();

				if (!kosten.EoF)
				{
					int progressCurrentValue = 0;
					ProgressChanged?.Invoke(null, new ProgressEventArgs(progressCurrentValue, kosten.RecordCount));

					while (!kosten.EoF)
					{
						if (!IsIntervallSet(kosten.Nummer))
						{
							kosten.Bezeichnung = kosten.Bezeichnung;
							kosten.Betrag = kosten.Betrag;
							kosten.Intervall = kosten.Intervall;
							kosten.IntervallEinheit = kosten.IntervallEinheit;
							kosten.Konto_Nr = kosten.Konto_Nr;
							kosten.Einnahme = kosten.Einnahme;
							if (kosten.UnterKonto_Nr != 0)
								kosten.UnterKonto_Nr = kosten.UnterKonto_Nr;
							kosten.Bezahlt = false;
							kosten.BezahltAm = ApS.Settings.NullDate;
							kosten.Notiz = "Automatische Verlängerung vom System aufgrund eines gesetzten Intervalls.";

							IntervallEinheiten einheit = (IntervallEinheiten)kosten.IntervallEinheit;
							switch (einheit)
							{
								case IntervallEinheiten.AlleXTage:
									kosten.LaufzeitBis = kosten.LaufzeitBis.AddDays(kosten.Intervall);
									break;
								case IntervallEinheiten.AlleXWochen:
									kosten.LaufzeitBis = kosten.LaufzeitBis.AddDays(kosten.Intervall * 7);
									break;
								case IntervallEinheiten.AlleXMonate:
									kosten.LaufzeitBis = kosten.LaufzeitBis.AddMonths(kosten.Intervall);
									break;
								case IntervallEinheiten.AlleXJahre:
									kosten.LaufzeitBis = kosten.LaufzeitBis.AddYears(kosten.Intervall);
									break;
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
									kosten.LaufzeitBis = new DateTime(kosten.LaufzeitBis.AddYears(1).Year, (int)einheit - 3, kosten.Intervall);
									break;
								case IntervallEinheiten.Null:
									break;
								default:
									break;
							}
							kosten.Save(SqlAction.Insert);
							SetIntervall(kosten.Nummer);
						}
						ProgressChanged?.Invoke(null, new ProgressEventArgs(++progressCurrentValue, kosten.RecordCount));
						kosten.Skip();
					}
				}
				InitilizationFinished?.Invoke(null, new EventArgs());
			}

			private static bool IsIntervallSet(int kosten_nr)
			{
				if (CoreSettings.GetSetting("Intervall_" + kosten_nr).ToBoolean())
					return true;
				else
					return false;
			}

			private static void SetIntervall(int kosten_nr)
			{
				CoreSettings.SetSetting("Intervall_" + kosten_nr, true);
			}
		}

		public class Updater : ApS.Update.Updater
		{
			public void UpdateDatabase()
			{
				if (!Core.CoreSettings.GetSetting("Version02Updated").ToBoolean())
				{
					this.UpdateDatabase(Versionen.Version02);
				}
				if (!Core.CoreSettings.GetSetting("Version05Updated").ToBoolean())
				{
					this.UpdateDatabase(Versionen.Version05);
				}
				if (!Core.CoreSettings.GetSetting("Version06Updated").ToBoolean())
				{
					this.UpdateDatabase(Versionen.Version06);
				}
				if (!Core.CoreSettings.GetSetting("Version06_1Updated").ToBoolean())
				{
					this.UpdateDatabase(Versionen.Version06_1);
				}
				if (!Core.CoreSettings.GetSetting("Version07Updated").ToBoolean())
				{
					this.UpdateDatabase(Versionen.Version07);
				}
				if (!Core.CoreSettings.GetSetting("Version07_2Updated").ToBoolean())
				{
					this.UpdateDatabase(Versionen.Version07_2);
				}
			}

			private void UpdateDatabase(Versionen vers)
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
					case Versionen.Version07:
						Datenbankversionen.Version07 v07 = new Datenbankversionen.Version07();
						v07.ErrorOccured += VersionenErrorOccured;
						v07.RunStatements();
						v07.ErrorOccured -= VersionenErrorOccured;
						CoreSettings.SetSetting("Version07Updated", true);
						break;
					case Versionen.Version07_2:
						Datenbankversionen.Version07_2 v07_2 = new Datenbankversionen.Version07_2();
						v07_2.ErrorOccured += VersionenErrorOccured;
						v07_2.RunStatements();
						v07_2.ErrorOccured -= VersionenErrorOccured;
						CoreSettings.SetSetting("Version07_2Updated", true);
						break;
					default:
						break;
				}
			}
			private void VersionenErrorOccured(object sender, ErrorInUpdate e)
			{
				ErrorOccured?.Invoke(this, new ErrorEventArgs(e.ErrorMessage));
			}
			
		}
		#endregion public
		#region private/protected

		#endregion private/protected
		#endregion Methods
	}

	public enum Versionen
	{
		Version01,
		Version02,
		Version03,
		Version04,
		Version05,
		Version06,
		Version06_1,
		Version07,
		Version07_2
	}
}