using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApS.Databases;
using ApS.Databases.Firebird;

namespace AKVCore
{
	public class Business : ApS.Databases.Firebird.Business
	{
		#region Felder

		#endregion Felder

		#region Konstruktoren
		public Business(string tabelle) : base(tabelle)
		{

		}
		public Business(string tabelle, string where, string spalten, string orderBy, string groupBy, SqlAction action) : this(tabelle)
		{
			this.Where = where;
			this.spalten = spalten;
			this.orderBy = orderBy;
			this.groupBy = groupBy;
			this.saveKind = this.SQLActionToDefines(action);
		}
		#endregion Konstruktoren

		#region Methoden

		#endregion Methoden
	}
}
