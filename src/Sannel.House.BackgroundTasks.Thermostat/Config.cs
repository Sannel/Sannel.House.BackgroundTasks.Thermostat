using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.BackgroundTasks.Thermostat
{
	internal class Config
	{
		public Uri ServiceApiUrl { get; internal set; }
		public string AppSecret { get; internal set; }
		public string Username { get; internal set; }
		public SecureString Password { get; internal set; }
	}
}
