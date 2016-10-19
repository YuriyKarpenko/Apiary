using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apiary
{
	class Settings : IT.SettingsBase<Settings>
	{
		public string ConnectionString
		{
			get { return this.GetConnectionString("db"); }
		}
	}
}
